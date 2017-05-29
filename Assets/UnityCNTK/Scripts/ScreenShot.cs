using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

namespace MapNinja
{

    // Screen Recorder will save individual images of active scene in any resolution and of a specific image format
    // including raw, jpg, png, and ppm.  Raw and PPM are the fastest image formats for saving.
    //
    // You can compile these images into a video using ffmpeg:
    // ffmpeg -i screen_3840x2160_%d.ppm -y test.avi

    public class ScreenShot : MonoBehaviour
    {
        // 4k = 3840 x 2160   1080p = 1920 x 1080
        public static ScreenShot instance
        {
            get; private set;
        }
        public int captureWidth = 2048;
        public int captureHeight = 2048;

        // optional game object to hide during screenshots (usually your scene canvas hud)
        public GameObject hideGameObject;

        // optimize for many screenshots will not destroy any objects so future screenshots will be fast
        public bool optimizeForManyScreenshots = true;

        // configure with raw, jpg, png, or ppm (simple raw format)
        public enum Format { RAW, JPG, PNG, PPM };
        public Format format = Format.PPM;

        // folder to write output (defaults to data path)
        public string folder;
        // private vars for screenshot  
        private Rect rect;
        private RenderTexture renderTexture;
        private Texture2D screenShot;
        public ZMQServer server;

        public List<string> shotsPath = new List<string>();
        public UnityEvent<Material> OnScreenShotTaken;
        public delegate void OnScreenShotSaved(string path);
        public OnScreenShotSaved onScreenShotSaved;
        public string mapType = "_BumpMap";
        public Queue<string> producedPaths;
        public Queue<Screenshot> cachedScreenshots;
        private Camera cam;
        private void Start()
        {
            if (instance != null)
            {
                Debug.LogError("duplicated ScreenShot");
                Destroy(this);
            }
            else
            {
                instance = this;
            }
            cam = Camera.main;
            server = FindObjectOfType<OndemandFactory>().server;
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }
            cachedScreenshots = new Queue<Screenshot>();
            producedPaths = new Queue<string>();
        }
        private string GetFileName(Material mat, int number)
        {
            // if folder not specified by now use a good default
            if (folder == null || folder.Length == 0)
            {
                folder = Application.dataPath;
                if (Application.isEditor)
                {
                    // put screenshots in folder above asset path so unity doesn't index the files
                    var stringPath = folder + "/..";
                    folder = Path.GetFullPath(stringPath);
                }
                folder += "/screenshots";

                // make sure directoroy exists
                System.IO.Directory.CreateDirectory(folder);

            }
            var words = mat.name.Split(' ');
            string name;
            if (words.Length > 4)
            {
                //megascans
                name = words[words.Length - 4];
            }
            else
            {
                name = mat.name;

            }

            // use width, height, and counter for unique file name
            var filename = string.Format("{0}/{1}.{2}", folder, name + "_" + number, format.ToString().ToLower());


            // return unique filename
            return filename;
        }


        public void CaptureScreenshot(string filename)
        {
            // create screenshot objects if needed
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }
            Camera camera = Camera.main;
            camera.targetTexture = renderTexture;
            camera.Render();

            // read pixels will read from the currently active render texture so make our offscreen 
            // render  texture active and then read the pixels
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            // reset active camera texture and render texture
            camera.targetTexture = null;
            RenderTexture.active = null;


            // pull in our file header/data bytes for the specified image format (has to be done from main thread)
            byte[] fileHeader = null;
            byte[] fileData = null;
            if (format == Format.RAW)
            {
                fileData = screenShot.GetRawTextureData();
            }
            else if (format == Format.PNG)
            {
                fileData = screenShot.EncodeToPNG();

            }
            else if (format == Format.JPG)
            {
                fileData = screenShot.EncodeToJPG();
            }
            else // ppm
            {
                // create a file header for ppm formatted file
                string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                fileData = screenShot.GetRawTextureData();
            }
            // create new thread to save the image to file (only operation that can be done in background)

            new System.Threading.Thread(() =>
            {
                // create file and write optional header with image bytes
                var f = System.IO.File.Create(filename);
                if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                f.Write(fileData, 0, fileData.Length);
                f.Close();
                shotsPath.Add(filename);
                // Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
            }).Start();
            StartCoroutine(TrackSaveProgress(filename));
            // unhide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(true);

            // cleanup if needed
            if (optimizeForManyScreenshots == false)
            {
                Destroy(renderTexture);
                renderTexture = null;
                screenShot = null;
            }
        }

        //optimised version of capture screenshot
        public void CacheScreenshot(string filename)
        {
            cam.Render();
            screenShot.ReadPixels(rect, 0, 0, false);
            server.cachedScreenshots.Enqueue(new Screenshot(filename, null, screenShot.GetRawTextureData()));
        }


        public IEnumerator TrackSaveProgress(string fullPath)
        {
            yield return new WaitForSeconds(2);
            //Sleep and check occationally
            //Block until completed
            while (!shotsPath.Contains(fullPath))
            {
                yield return new WaitForSeconds(1);
            }
            //No race condition here as every file is only checked by one tracker
            shotsPath.Remove(fullPath);
            if (onScreenShotSaved != null)
            {
                onScreenShotSaved.Invoke(fullPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Relative path</param>
        /// <param name="texture"></param>
        public static void SaveToFile(string path, Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();


            var folder = Path.GetFullPath(Application.dataPath);
            System.IO.Directory.CreateDirectory(folder + "/Textures");

            new System.Threading.Thread(() =>
            {
                // create file and write optional header with image bytes
                var f = System.IO.File.Create(path, bytes.Length, FileOptions.Asynchronous);
                f.Write(bytes, 0, bytes.Length);
                f.Close();
                // Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
            }).Start();

        }

    }
}

