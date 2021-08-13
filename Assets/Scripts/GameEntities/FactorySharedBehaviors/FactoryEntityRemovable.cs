using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityRemovable : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryStructureRemovalEvent.AddListener(this.RemoveSelf);
    }

    void Update()
    {
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void RemoveSelf(GameObject removedGO)
    {
        Debug.Log("attemting to remove gameobject: " + removedGO.name);
        if (removedGO == this.gameObject)
        {
            Object.Destroy(this.gameObject);
        }
    }


}
