using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;
namespace UnityCNTK
{
    public class Texture2DSource : DataSource<Texture2D>
    {
        public enum TexturePredefinedSources
        {
            camera, webcam, none, custom
        }
        public TexturePredefinedSources source;
        public Camera cam;
        public int width = 64;
        public int height = 64;
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
                        rect = new Rect(0, 0, width, height);
                        dummyTexture = new Texture2D(width, height);
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
                        var rawImage = GetComponent<RawImage>();
                        dummyTexture = new Texture2D(width, height);
                        Assert.IsNotNull(rawImage);
                        rawImage.material.mainTexture = webcamTexture;
                        webcamTexture.Play();
                        GetData = new getData(() =>
                        {
                            dummyTexture.SetPixels(webcamTexture.ResampleAndCrop(width, height).GetPixels());
                            var jpgBytes = dummyTexture.EncodeToJPG();
                            var f = System.IO.File.Create("Banana.jpg");
                            f.Write(jpgBytes, 0, jpgBytes.Length);
                            f.Close();
                            return dummyTexture;
                        });
                        break;
                    }
                default:
                    {
                        Debug.Log("Texture2D Not streaming");
                        dummyTexture = new Texture2D(width, height);
                        GetData = new getData(() => dummyTexture);
                        break;
                    }
            }
        }
    }
}