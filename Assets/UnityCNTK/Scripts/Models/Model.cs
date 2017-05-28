using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Timers;
using System.Threading;
using CNTK;
using System;
using UnityEngine.Events;

namespace UnityCNTK
{
    /// <summary>
    /// Base class that 
    /// </summary>
    public class Model : ScriptableObject
    {
        public UnityEvent OnModelLoaded;
        
        private Model(){}
        
        public string Name;
        public string relativeModelPath;
        public Function function;
        private UnityEngine.Object _input;
        public UnityEngine.Object input
        {
            get { return _input; }
            set
            {
                if(isEvaluating)
                {
                    Debug.LogError("Input cannot be set when an evaluation is carrying out");
                }
                else
                {
                    _input = value;
                }
            }
        }
        public bool isEvaluating = false;

        private UnityEngine.Object _output = null;

        public UnityEngine.Object output
        {

            get { return _output; }
            set
            {
                if(isEvaluating)
                {
                    Debug.LogError("Input cannot be set when an evaluation is carrying out");
                }
                else
                {
                    _output = value;
                }
            }
        }
        /// <summary>
        /// Load the model automatically on start
        /// </summary>
        public bool LoadOnStart = true;
        protected Thread thread;
        public virtual void LoadModel()
        {
            Assert.IsNotNull(relativeModelPath);
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            Thread loadThread = new Thread(() => function = Function.Load(absolutePath, CNTKManager.device));
        }

        public virtual void Evaluate()
        {
            Assert.IsNotNull(function);
            isEvaluating = true;
            var IOValues = OnPreprocess();
            thread = new Thread(() =>
            {
                function.Evaluate(IOValues[0], IOValues[1], CNTKManager.device);
                OnEvaluated(IOValues[1]);
                isEvaluating = false;
            });
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<Dictionary<Variable, Value>> OnPreprocess()
        {
            throw new NotImplementedException();
        }

        // process output data to fit user requirement
        // you should set the convert output Value to 
        protected virtual void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            throw new NotImplementedException();
        }

        public virtual void OnPostProcessed()
        {}

        /// <summary>
        /// Unload model from memory
        /// </summary>
        public void UnloadModel()
        {
            if(function != null)
            {
                function.Dispose();
            }
        }
    }

}
