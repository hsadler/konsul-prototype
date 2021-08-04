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
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void CheckSetSelected(GameObject selectedGO)
    {
        if (selectedGO == this.harvesterBody)
        {
            this.SetSelected(!this.isSelected);
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckDeselect()
    {
        if (this.isSelected && Input.GetKeyDown(KeyCode.Q))
        {
            this.SetSelected(false);
        }
    }

    private void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        this.selectedLabel.SetActive(isSelected);
    }

}
