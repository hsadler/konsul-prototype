using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntitySelectable : MonoBehaviour
{


    public GameObject selectionIndicator;

    private bool isSelected = false;


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryEntitySelectedEvent.AddListener(this.CheckSetSelected);
        GalaxySceneManager.instance.factoryEntityDelesectAllEvent.AddListener(this.Deselect);
        this.Deselect();
    }

    void Update()
    {

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


}
