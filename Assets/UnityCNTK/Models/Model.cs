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

namespace UnityCNTK
{
    // Unifying class that combine the use of both EvalDLL and CNTKLibrary
    // The reason we have not dropped support for EvalDLL is that it expose more native function of the full API,
    // which are very useful in general.
    public class Model : ScriptableObject
    {
        public string Name;
        public string relativeModelPath;
        public Function function;
        private IConvertible _input;
        public IConvertible input
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

        private IConvertible _output;

        public IConvertible output
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
                    _input = value;
                }
            }
        }

        public Thread thread;
        public bool KeepModelLoaded = false;
        public DeviceDescriptor device;
        public virtual void LoadModel()
        {
            Assert.IsNotNull(relativeModelPath);
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            // Downloader.DownloadPretrainedModel(Downloader.pretrainedModel.VGG16, absolutePath);
        }

        public virtual void Evaluate()
        {
            var IOValues = OnPreprocess();
            thread = new Thread(() =>
            {
                function.Evaluate(IOValues[0], IOValues[1], device);
                OnEvaluated(IOValues[1]);
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public virtual List<Dictionary<Variable, Value>> OnPreprocess()
        {
            Assert.IsNotNull(device);
            Assert.IsNotNull(input);
            if (function == null) LoadModel();
            var inputVar = function.Arguments.Single();
            var inputDataMap = new Dictionary<Variable, Value>() { { inputVar, input.ToValue(device) } };
            var outputDataMap = new Dictionary<Variable, Value>() { { function.Output, null } };
            return new List<Dictionary<Variable, Value>>() { inputDataMap, outputDataMap };
        }

        // process output data to fit user requirement
        public virtual void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {

        }

    }

}
