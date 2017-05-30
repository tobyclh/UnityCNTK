using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCNTK;
using CNTK;
using System;
using UnityEngine.Assertions;

namespace UnityCNTK
{
    
    /// <summary>
    /// Streaming model streams a datasource to a model every set period
    /// </summary>
    [CreateAssetMenu(fileName = "StreamingModel", menuName = "UnityCNTk/Model/StreamingModel")]
    public class StreamingModel: Model 
    {
        public DataSource source;

		[Tooltip("Evaluation carry out in every specificed second")]
		public float evaluationPeriod = 10;
        private bool shouldStop = false;
        private bool isStreaming = false;
        public void StartStreaming()
        {
            Assert.IsFalse(isStreaming, name + " is already streaming");
            Debug.Log("Start Streaming");
            InvokeRepeating("MyStream", 0, evaluationPeriod);
        }

        private void MyStream()
        {
            Debug.Log("Grabbing data");
            var data = source.GetData();
            Evaluate(data);
            //    yield return new WaitForSeconds(evaluationPeriod);
            //}
        }
        protected override void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            Debug.Log("OnEvaluated OVERRIDED");
        }

        public void StopStreaming()
        {
            Assert.IsTrue(isStreaming, name + " is not streaming");
            shouldStop = true;
        }

    }
}

