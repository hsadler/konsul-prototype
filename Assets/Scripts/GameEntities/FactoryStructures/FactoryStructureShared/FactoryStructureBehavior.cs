using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureBehavior : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryStructureCount += 1;
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        GalaxySceneManager.instance.factoryStructureCount -= 1;
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS


}
