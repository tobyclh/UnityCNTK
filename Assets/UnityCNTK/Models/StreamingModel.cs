using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCNTK;
using CNTK;
using System;

namespace UnityCNTK{
    public class StreamingModel<T, V> : Model 
    where T:IConvertible 
    where V:IConvertible

    {

		public new T input; 
		public new V output;

		// Evaluation carry out in every 'evaluationPeriod' second
		public double evaluationPeriod = 10;

		public bool isEvaluating = false;

        public override void Evaluate(DeviceDescriptor device)
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

        public override void OnEvaluated()
        {
            throw new NotImplementedException();
        }
    }
}

