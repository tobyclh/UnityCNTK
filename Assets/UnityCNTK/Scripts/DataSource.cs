using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Assertions;
using CNTK;
namespace UnityCNTK
{
    public class DataSource : MonoBehaviour
    {
        public enum PredefinedSource
        {
            none, pos, rot, quat, velocity, acceleration, camera, custom, webcam
        }

        public PredefinedSource pSource;
        public delegate Value getData();
        public getData GetData;
        private new Transform transform;
        private Rigidbody rb;
        private CNTKManager manager;
        public int width;
        public int height;
        private Texture2D dummyTexture;
        private RenderTexture renderTexture;
        private Camera cam;
        private Rect rect;
        private IntPtr webcamTexPtr;
        void Start()
        {
            transform = base.transform;
            switch (pSource)
            {
                case PredefinedSource.pos:
                    {
                        GetData = new getData(() => transform.position.ToValue(CNTKManager.device));
                        break;
                    }
                case PredefinedSource.rot:
                    {
                        GetData = new getData(() => transform.rotation.eulerAngles.ToValue(CNTKManager.device));
                        break;
                    }
                case PredefinedSource.quat:
                    {
                        GetData = new getData(() => transform.rotation.ToValue(CNTKManager.device));
                        break;
                    }
                case PredefinedSource.velocity:
                    {
                        rb = GetComponent<Rigidbody>();
                        if (rb == null)
                        {
                            throw new MissingComponentException("Missing Rigidbody");
                        }
                        GetData = new getData(() => rb.velocity.ToValue(CNTKManager.device));
                        break;
                    }
                case PredefinedSource.camera:
                    {

                        cam = GetComponent<Camera>();
                        if (cam == null)
                        {
                            throw new MissingComponentException("Missing Camera");
                        }
                        rect = new Rect(0, 0, width, height);
                        dummyTexture = new Texture2D(width, height);
                        GetData = new getData(() =>
                        {
                            cam.targetTexture = renderTexture;
                            cam.Render();
                            dummyTexture.ReadPixels(rect, 0, 0, false);

                            return dummyTexture.ToValue(CNTKManager.device);
                        });
                        break;
                    }
                case PredefinedSource.webcam:
                    {
                        var WebcamTexture = new WebCamTexture();
                        webcamTexPtr = WebcamTexture.GetNativeTexturePtr();
                        GetData = new getData(() =>
                        {
                            dummyTexture.UpdateExternalTexture(webcamTexPtr);
                            return dummyTexture.ToValue(CNTKManager.device);
                        });
                        break;
                    }
                default:
                    {
                        Debug.Log("Predefined source not set");
                        break;
                    }
            }
        }
    }

}
