using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCNTK;
/// <summary>
/// Non-copying static class to provide optimized and editor controllable features
/// Attach this to object that provide the source
/// </summary>
public class Vector3Source : DataSource<Vector3>{
    
    public enum Vector3PredefinedSources
    {
        position, localPosition, euler, localEuler, velocity, none, custome
    }
    public Vector3PredefinedSources source = Vector3PredefinedSources.position;
    private Rigidbody rb;
    void Start()
    {
        switch (source)
        {
            case Vector3PredefinedSources.position:
                {
                    GetData = new getData(() => transform.position);
                    break;
                }
            case Vector3PredefinedSources.euler:
                {
                    GetData = new getData(() => transform.rotation.eulerAngles);
                    break;
                }
            case Vector3PredefinedSources.velocity:
                {
                    rb = GetComponent<Rigidbody>();
                    if (rb == null)
                    {
                        throw new MissingComponentException("Missing Rigidbody");
                    }
                    GetData = new getData(() => rb.velocity);
                    break;
                }
            default:
                {
                    Debug.Log("Predefined source not set");
                    break;
                }
        }
    }

}
