using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace UnityCNTK
{
    //Simple 
    public interface IConvertible
    {
        Value ToValue(DeviceDescriptor device);
    } 
    
}
