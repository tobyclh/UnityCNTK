using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCNTK;
using CNTK;
using System;
using UnityEngine.Assertions;
using System.Linq;
namespace UnityCNTK
{

    public class FERPlusModel : Model<Texture2D, int>, IStreamModel
    {
        [Tooltip("Evaluation carry out in every specificed second")]
        public float evaluationPeriod = 10;
        private bool shouldStop = false;
        private bool isStreaming = false;
        public new Texture2DSource dataSource;
        public void StartStreaming()
        {
            Assert.IsFalse(isStreaming, " is already streaming");
            Debug.Log("Start Streaming");
            InvokeRepeating("MyStream", 0, evaluationPeriod);
        }

        private async void MyStream()
        {
            //Debug.Log("Grabbing data");
            var data = dataSource.GetData();
            Evaluate(data);
        }

        public void StopStreaming()
        {
            Assert.IsTrue(isStreaming, name + " is not streaming");
            CancelInvoke("MyStream");
        }
        public override int OnPostProcess(Value output)
        {
            //Debug.Log("OnPostprocess FER+");
            var expression = output.GetDenseData<float>(function.Output);
            var e = expression[0];
            //Debug.Log("EXP Count :" + expression.Count);
            //Debug.Log("E Count :" + e.Count);
            Debug.Log(e[0].ToString() + " " + e[1].ToString() + " " + e[2].ToString() + " " + e[3].ToString() + " " + e[4].ToString() + " " + e[5].ToString() + " " +
                e[6].ToString() + " " + e[7].ToString());
            return e.IndexOf(e.Max());
        }

        public override Value OnPreprocess(Texture2D input)
        {
            //Debug.Log("OnPreprocess FER+");
            return input.ToValue(CNTKManager.device, true);
        }

        public override void OnEvaluted(int output)
        {
            Debug.Log("OUTPUT : " + output);
        }

    }
}