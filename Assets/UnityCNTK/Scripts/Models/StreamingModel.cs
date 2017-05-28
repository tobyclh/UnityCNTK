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
    public class StreamingModel: Model 
    {

		public new DataSource input;

		[Tooltip("Evaluation carry out in every specificed second")]
		public double evaluationPeriod = 10;

        public override void Evaluate()
        {
			if (isEvaluating)
			{
            	throw new TimeoutException("Evalauation not finished before the another call, ");
			}
            isEvaluating = true;
			
        }

        public override void LoadModel()
        {
            throw new NotImplementedException();
        }

        protected override void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            isEvaluating = false;
            
        }
    }
}

