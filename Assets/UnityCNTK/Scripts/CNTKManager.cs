using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;
using CNTK;
namespace UnityCNTK
{
    public class CNTKManager : MonoBehaviour
    {
        public static CNTKManager instance;
        public List<_Model> managedModels = new List<_Model>();
        public static DeviceDescriptor device;
        public enum HardwareOptions
        {
            Auto, CPU, GPU, Default
        }
        [Tooltip("Where to put all the work, Avoid 'Default' unless you are familiar with the usage")]
        public HardwareOptions hardwareOption = HardwareOptions.CPU;


        void Awake()
        {
            MakeSingleton();
        }

        void Start()
        {
            switch (hardwareOption)
            {
                case HardwareOptions.Auto:
                    {
                        SelectBestDevice();
                        break;
                    }
                case HardwareOptions.CPU:
                    {
                        device = DeviceDescriptor.CPUDevice;
                        break;
                    }
                case HardwareOptions.GPU:
                    {
                        device = DeviceDescriptor.GPUDevice(0);
                        break;
                    }
                case HardwareOptions.Default:
                    {
                        device = DeviceDescriptor.UseDefaultDevice();
                        break;
                    }
            }
            for (int i = 0; i < managedModels.Count; i++)
            {
                var model = managedModels[i];
                if (model.LoadOnStart)
                {
                    model.LoadModel();
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                foreach (var m in managedModels)
                {
                    if( m is IStreamModel)
                    {
                        ((IStreamModel)m).StartStreaming();
                        Debug.Log(" Start Streaming");
                    }

                }
            }
        }


        private void MakeSingleton()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError("Only 1 CNTK manager allowed in a scene, destroying");
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        private void SelectBestDevice()
        {
            var gpu = DeviceDescriptor.GPUDevice(0);
            if (gpu != null)
            {
                device = gpu;
            }
            else
            {
                device = DeviceDescriptor.CPUDevice;
            }
        }
    }

}
