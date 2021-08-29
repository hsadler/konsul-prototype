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


    public int GetRandomTypeFromProbabilities(IDictionary<int, float> typeToProbability)
    {
        float total = 0;
        foreach (KeyValuePair<int, float> entry in typeToProbability)
        {
            total += entry.Value;
        }
        float rand = Random.Range(0, total);
        float top = 0;
        foreach (KeyValuePair<int, float> entry in typeToProbability)
        {
            top += entry.Value;
            if (rand <= top)
            {
                return entry.Key;
            }
        }
        Debug.LogWarning("Should never get to this code. total: " + total.ToString() + " top: " + top.ToString());
        return ConstFEType.NONE;
    }


}
