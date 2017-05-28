using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCNTK;
using CNTK;
using System;

namespace UnityCNTK{
    
    /// <summary>
    /// Streaming model streams a datasource to a model every set period
    /// </summary>
    [CreateAssetMenu(fileName = "StreamingModel", menuName = "UnityCNTk/Model/StreamingModel")]
    public class StreamingModel: Model 
    {
        public DataSource source;

		[Tooltip("Evaluation carry out in every specificed second")]
		public double evaluationPeriod = 10;
        
        public void StartStreaming()
        {
            
        }

        public void StopStreaming()
        {

        }


        protected override void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            isEvaluating = false;
            
        }
    }
}

