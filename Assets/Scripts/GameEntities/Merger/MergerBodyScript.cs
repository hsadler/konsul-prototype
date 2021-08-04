using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerBodyScript : MonoBehaviour
{


    public MergerScript mScript;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Merger collided with object tag: " + other.gameObject.tag);
    }


}
