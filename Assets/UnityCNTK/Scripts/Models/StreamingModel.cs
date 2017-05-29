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
        public IEnumerator StartStreaming()
        {
            Assert.IsFalse(isStreaming, name + " is already streaming");
            while (!shouldStop)
            {
                Evaluate(source.GetData());
                yield return new WaitForSeconds(evaluationPeriod);
            }
        }

        public void StopStreaming()
        {
            Assert.IsTrue(isStreaming, name + " is not streaming");
            shouldStop = true;
        }

    }
}

