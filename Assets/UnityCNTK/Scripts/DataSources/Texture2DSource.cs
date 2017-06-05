using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
namespace UnityCNTK
{
    public class Texture2DSource : DataSource<Texture2D>
    {
        public enum TexturePredefinedSources
        {
            camera, webcam, custom
        }
        public TexturePredefinedSources source;
        public Camera cam;
        protected Texture2D dummyTexture;
        protected RenderTexture renderTexture;
        protected Rect rect;
        protected WebCamTexture webcamTexture;
        protected IntPtr webcamTexPtr;
        protected RawImage rawImage;
        void Start()
        {
            switch (source)
            {
                case TexturePredefinedSources.camera:
                    {
                        if (cam == null)
                        {
                            cam = GetComponent<Camera>();
                            if (cam == null)
                            {
                                throw new MissingComponentException("Missing Camera");
                            }
                        }
                        rect = new Rect(0, 0, cam.pixelWidth, cam.pixelHeight);
                        dummyTexture = new Texture2D(cam.pixelWidth, cam.pixelHeight);
                        GetData = new getData(() =>
                        {
                            cam.targetTexture = renderTexture;
                            cam.Render();
                            dummyTexture.ReadPixels(rect, 0, 0, false);
                            return dummyTexture;
                        });
                        break;
                    }
                case TexturePredefinedSources.webcam:
                    {
                        webcamTexture = new WebCamTexture();
                        webcamTexture.Play();
                        var rawImage = GetComponent<RawImage>();
                        rawImage.material.mainTexture = webcamTexture;
                        dummyTexture = new Texture2D(rawImage.material.mainTexture.width, rawImage.material.mainTexture.height);
                        Debug.Log("Webcam Texture is " + rawImage.material.mainTexture.width.ToString() + " " + rawImage.material.mainTexture.height.ToString());
                        Assert.IsNotNull(rawImage);
                        GetData = new getData(() =>
                        {
                            dummyTexture.SetPixels(webcamTexture.GetPixels());
                            return dummyTexture;
                        });
                        break;
                    }
                default:
                    {
                        Debug.Log("Texture2D Not streaming");
                        dummyTexture = new Texture2D(1, 1);
                        GetData = new getData(() => dummyTexture);
                        break;
                    }
            }
        }
    }
}