﻿using UnityEngine;
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
        public int width;
        public int height;
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
                        webcamTexture = new WebCamTexture(width, height);
                        webcamTexPtr = webcamTexture.GetNativeTexturePtr();
                        //testing code
                        var rawImage = GetComponent<RawImage>();
                        Assert.IsNotNull(rawImage);
                        rawImage.material.mainTexture = webcamTexture;
                        webcamTexture.Play();
                        GetData = new getData(() =>
                        {
                            dummyTexture.UpdateExternalTexture(webcamTexPtr);
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