using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureIOBehavior : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryStructureIOPlacementEvent.AddListener(this.AddIO);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void AddIO(GameObject fromGO, GameObject toGO)
    {
        // TODO: implement
        Debug.Log("Creating transit-line from: " + fromGO.name + " to: " + toGO.name);
    }


}
