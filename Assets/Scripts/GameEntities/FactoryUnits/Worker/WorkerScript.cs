using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerScript : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    }

    // INTERFACE METHODS

    public void FetchAndPlaceFactoryStructure(int structureType, Vector3 position)
    {
        // STUB
        Debug.Log("Worker is carrying out order to fetch structure type: " + structureType.ToString() +
            " and place at position: " + position.ToString());
    }

    // IMPLEMENTATION METHODS


}
