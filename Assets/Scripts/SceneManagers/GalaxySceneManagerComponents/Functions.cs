using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions
{


    public Vector3 GetIntRoundedVector3(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x),
            Mathf.Round(v.y),
            Mathf.Round(v.z)
        );
    }


}
