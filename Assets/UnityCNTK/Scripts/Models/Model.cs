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

        public virtual async Task<V> Evaluate(U input)
        {
            if (!isReady)
            {
                Debug.LogError("A model can only evaluate 1 object at a time");
            }
            isReady = false;
            var inputVal = OnPreprocess(input);
            if (function == null) LoadModel();
            Assert.IsNotNull(function);
            var output = await Task.Run(() =>
            {
                Debug.Log("Evaluation");
                var outputPair = new Dictionary<Variable, Value>() { { function.Output, null } };
                var variable = function.Arguments.Single();
                var inputPair = new Dictionary<Variable, Value>() { { variable, inputVal } };
                function.Evaluate(inputPair, outputPair, CNTKManager.device);
                var _output = OnPostProcess(outputPair.Single().Value);
                isReady = true;
                return _output;
            });
            return output;
        }

        public virtual Value OnPreprocess(U input)
        {
            throw new NotImplementedException();
        }

        public virtual V OnPostProcess(Value output)
        {
            throw new NotImplementedException();
        }

    }

}
