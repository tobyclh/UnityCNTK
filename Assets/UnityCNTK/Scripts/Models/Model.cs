using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using CNTK;
using System;
using UnityEngine.Events;

namespace UnityCNTK
{
    /// <summary>
    /// Base class for all models
    /// currently only SISO (Single In Single Out) model is supported, which should cover most cases
    /// </summary>

    public class Model<U, V> : _Model
    {
        public DataSource<U> dataSource;
        public virtual async void Evaluate(U input)
        {
            if (!isReady)
            {
                Debug.LogError("A model can only evaluate 1 object at a time");
            }
            isReady = false;
            var inputVal = OnPreprocess(input);
            Assert.IsNotNull(function);
            
            var outputPair = new Dictionary<Variable, Value>() { { function.Output, null } };
            var variable = function.Arguments.Single();
            var inputPair = new Dictionary<Variable, Value>() { { variable, inputVal } };
            await Task.Run(() =>
            {
                //Debug.Log("Evaluation");
                function.Evaluate(inputPair, outputPair, CNTKManager.device);
            });
            var _output = OnPostProcess(outputPair.Single().Value);
            OnEvaluted(_output);
            isReady = true;
        }

        public virtual Value OnPreprocess(U input)
        {
            throw new NotImplementedException();
        }

        public virtual void OnEvaluted(V output)
        {
        }

        public virtual V OnPostProcess(Value output)
        {
            throw new NotImplementedException();
        }

    }

}
