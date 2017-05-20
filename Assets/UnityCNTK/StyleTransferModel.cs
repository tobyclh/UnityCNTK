using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CNTK;
namespace UnityCNTK
{
    public class StyleTransferModel : Model
    {
        public Texture2D styleReference;
        
        [TooltipAttribute("How similar should the style reassemble the style")]
        [RangeAttribute(0,1)]
        public float styleStrength;
        public void ApplyFunction()
        {
            //Evaluate the function here
        }
    }
}