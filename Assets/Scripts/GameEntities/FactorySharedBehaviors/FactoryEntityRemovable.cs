using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityRemovable : MonoBehaviour
{


    public GameObject toRemoveIndicator;


    // UNITY HOOKS

    void Start()
    {
        this.toRemoveIndicator.SetActive(false);
        GalaxySceneManager.instance.factoryStructureRemovalEvent.AddListener(this.RemoveSelf);
    }

    void Update()
    {
    }

    // INTERFACE METHODS

    public void MarkForRemoval()
    {
        this.toRemoveIndicator.SetActive(true);
    }

    // IMPLEMENTATION METHODS

    private void RemoveSelf(GameObject removedGO)
    {
        // Debug.Log("attemting to remove gameobject: " + removedGO.name);
        if (removedGO == this.gameObject)
        {
            Object.Destroy(this.gameObject);
        }
    }


}
