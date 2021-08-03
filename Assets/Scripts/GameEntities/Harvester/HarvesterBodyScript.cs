using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterBodyScript : MonoBehaviour
{


    public HarvesterScript hScript;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other tag: " + other.gameObject.tag);
    }


}
