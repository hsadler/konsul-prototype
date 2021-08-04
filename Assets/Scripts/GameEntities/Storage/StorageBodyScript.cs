using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBodyScript : MonoBehaviour
{


    public StorageScript sScript;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Storage collided with object tag: " + other.gameObject.tag);
    }


}
