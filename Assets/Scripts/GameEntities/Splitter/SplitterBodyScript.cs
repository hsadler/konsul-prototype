using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterBodyScript : MonoBehaviour
{


    public SplitterScript sScript;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Splitter collided with object tag: " + other.gameObject.tag);
    }


}
