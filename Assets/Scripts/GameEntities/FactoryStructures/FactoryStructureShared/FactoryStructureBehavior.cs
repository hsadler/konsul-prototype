using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureBehavior : MonoBehaviour
{


    public GameObject rootGO;
    public GameObject selectionIndicator;

    private bool isSelected = false;


    // UNITY HOOKS

    void Start()
    {
        this.selectionIndicator.SetActive(false);
        GalaxySceneManager.instance.factoryStructureSelectedEvent.AddListener(this.CheckSetSelected);
        GalaxySceneManager.instance.factoryStructureDelesectAllEvent.AddListener(this.Deselect);
        GalaxySceneManager.instance.factoryStructureRemovalEvent.AddListener(this.RemoveSelf);
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

    private void CheckSetSelected(GameObject selectedGO)
    {
        if (selectedGO == this.gameObject)
        {
            this.SetSelected(true);
        }
        else
        {
            this.SetSelected(false);
        }
    }

    private void Deselect()
    {
        this.SetSelected(false);
    }

    private void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        this.selectionIndicator.SetActive(isSelected);
    }

    private void RemoveSelf(GameObject removedGO)
    {
        if (removedGO == this.gameObject)
        {
            Object.Destroy(this.rootGO);
        }
    }


}
