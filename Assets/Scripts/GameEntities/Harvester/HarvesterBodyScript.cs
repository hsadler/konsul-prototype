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
        if (other.gameObject.CompareTag("Planet"))
        {
            int resource = other.gameObject.GetComponent<PlanetBodyScript>().planetScript.ExtractResource();
            Debug.Log("extracted resource type from planet: " + resource.ToString());
        }
    }


}
