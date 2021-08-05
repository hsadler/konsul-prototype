using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour
{


    public GameObject harvesterBody;
    public GameObject selectedLabel;

    private bool isSelected = false;



    // UNITY HOOKS

    void Start()
    {
        this.selectedLabel.SetActive(false);
        GalaxySceneManager.instance.factoryStructureSelectedEvent.AddListener(this.CheckSetSelected);
        GalaxySceneManager.instance.factoryStructureDelesectAllEvent.AddListener(this.Deselect);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void CheckSetSelected(GameObject selectedGO)
    {
        if (selectedGO == this.harvesterBody)
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
        this.selectedLabel.SetActive(isSelected);
    }

}
