using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotRotationChild : MonoBehaviour
{
    Vector3 def;

    void Awake ()
    {
        def = transform.localRotation.eulerAngles;
    }
    
    void FixedUpdate ()
    {
        Vector3 _parent = transform.parent.transform.localRotation.eulerAngles;

        //èCê≥â”èä
        transform.localRotation = Quaternion.Euler(def - _parent);
    }
}

