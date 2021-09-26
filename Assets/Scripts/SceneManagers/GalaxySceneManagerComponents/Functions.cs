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


    public int GetRandomTypeFromProbabilities(IDictionary<int, int> typeToProbability)
    {
        float total = 0;
        foreach (KeyValuePair<int, int> entry in typeToProbability)
        {
            total += entry.Value;
        }
        float rand = Random.Range(0, total);
        float top = 0;
        foreach (KeyValuePair<int, int> entry in typeToProbability)
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


    public List<int> GetIntListFromIntToCountDict(IDictionary<int, int> intToCount)
    {
        // TODO: implement stub
        return new List<int>();
    }


    public IDictionary<int, int> GetIntToCountDictFromIntList(List<int> intList)
    {
        // TODO: implement stub
        return new Dictionary<int, int>();
    }


}
