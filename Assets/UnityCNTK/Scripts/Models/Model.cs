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
    // Unifying class that combine the use of both EvalDLL and CNTKLibrary
    // The reason we have not dropped support for EvalDLL is that it expose more native function of the full API,
    // which are very useful in general.
    public class Model : ScriptableObject
    {
        public UnityEvent OnModelLoaded;
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

        public bool KeepModelLoaded = false;
        public DeviceDescriptor device;
        protected Thread thread;
        public virtual void LoadModel()
        {
            Assert.IsNotNull(relativeModelPath);
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
        }

        public virtual void Evaluate()
        {
            Assert.IsNotNull(function);
            isEvaluating = true;
            var IOValues = OnPreprocess();
            thread = new Thread(() =>
            {
                function.Evaluate(IOValues[0], IOValues[1], device);
                OnEvaluated(IOValues[1]);
                if (!KeepModelLoaded) function.Dispose();
                isEvaluating = false;
            });
            thread.IsBackground = true;
            thread.Start();
        }

        // Process input data to be consumed by 
        public virtual List<Dictionary<Variable, Value>> OnPreprocess()
        {
            throw new NotImplementedException();
        }

        // process output data to fit user requirement
        // you should set the convert output Value to 
        protected virtual void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            throw new NotImplementedException();
        }

        // by now the 
        public virtual void OnPostProcessed()
        {}
    }

}
