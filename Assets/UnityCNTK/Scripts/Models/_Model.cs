using System;
using System.Collections.Generic;
using UnityEngine;
using CNTK;
using UnityEngine.Events;
using System.Threading;
using UnityEngine.Assertions;
namespace UnityCNTK
{
    /// <summary>
    /// the non-generic base class for all model
    /// only meant to be used for model management and GUI scripting, not in-game
    /// </summary>
    public class _Model : MonoBehaviour{
        public UnityEvent OnModelLoaded;
        public TextAsset rawModel;
        public Function function;
        private bool _isReady = false;
        public bool isReady { get; protected set; }
        /// <summary>
        /// Load the model automatically on start
        /// </summary>
        public bool LoadOnStart = true;
        protected Thread thread;

        public virtual void LoadModel()
        {
            Assert.IsNotNull(rawModel);
            
                Debug.Log("Started thread");
                try
                {
                    function = Function.Load(rawModel.bytes, CNTKManager.device);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                if (OnModelLoaded != null)
                    OnModelLoaded.Invoke();
                isReady = true;
                Debug.Log("Model Loaded : " +rawModel.name);
        }
        public void UnloadModel()
        {
            if (function != null)
            {
                function.Dispose();
            }
        }

    }
}
