using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using System.Timers;
using System.Threading;
using CNTK;
using System;
using UnityEngine.Events;

namespace UnityCNTK
{
    /// <summary>
    /// Base class for all models
    /// </summary>

    public class Model : ScriptableObject
    {
        public UnityEvent<Model> OnModelLoaded;
        public UnityEvent<Model, System.Object> OnPostProcessed;

        public Model() { }

        public string relativeModelPath;
        public Function function;
        public void OnEnable()
        {

        }

        public bool isEvaluating { get; protected set; }
        protected System.Object output;
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
            OnModelLoaded.Invoke(this);
        }

        public virtual void Evaluate(Value input)
        {
            if (isEvaluating)
            {
                Debug.LogError("A model can only evaluate 1 object at a time");
            }
            if (function == null) LoadModel();
            Assert.IsNotNull(function);
            Assert.IsNotNull(input);
            isEvaluating = true;
            thread = new Thread(() =>
            {
                var outputPair = new Dictionary<Variable,Value>(){{function.Output, null}};
                function.Evaluate(new Dictionary<Variable,Value>(){{function.Arguments.Single(), input}}, outputPair, CNTKManager.device);
                OnEvaluated(outputPair);
                OnPostProcessed.Invoke(this, output);
                isEvaluating = false;
            });
            thread.IsBackground = true;
            thread.Start();
        }


        /// <summary>
        /// Convert outputdatamap to user defined value, store in output
        /// See HelperClasses/Convert.cs for some example functions that do so
        /// </summary>
        /// <param name="outputDataMap">output data map that result from evaluation </param>
        protected virtual void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// Unload model from memory
        /// </summary>
        public void UnloadModel()
        {
            if (function != null)
            {
                function.Dispose();
            }
        }
    }

}
