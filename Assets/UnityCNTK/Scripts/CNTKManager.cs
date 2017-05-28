using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CNTK;
namespace UnityCNTK{
	public class CNTKManager : MonoBehaviour {
		public static CNTKManager instance;
		public List<Model> managedModels = new List<Model>();
		public static DeviceDescriptor device;
		public enum HardwareOptions
		{
			Auto, CPU, GPU, Default
		}
		[Tooltip("Where to put all the work, Avoid 'Default' unless you are familiar with the usage")]
		public HardwareOptions hardwareOption = HardwareOptions.Auto;

		
		void Awake()
		{
			MakeSingleton();
		}
		
		void Start () {
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
			foreach(var model in managedModels)
			{
				if(model.LoadOnStart)
				{
					model.LoadModel();
				}
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		private void MakeSingleton()
		{
			if (instance != null && instance != this )
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
			if(gpu != null)
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
