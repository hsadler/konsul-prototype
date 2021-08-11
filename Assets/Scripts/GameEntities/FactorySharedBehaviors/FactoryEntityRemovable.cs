using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityRemovable : MonoBehaviour
{


    public GameObject rootGO;


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
        if (removedGO == this.gameObject)
        {
            Object.Destroy(this.rootGO);
        }
    }

}
