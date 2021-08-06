using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceIOScript : MonoBehaviour
{


    public GameObject selectionIndicator;

    public bool isSelected;


    // UNITY HOOKS

    void Start()
    {
        this.selectionIndicator.SetActive(false);
        this.isSelected = false;
        GalaxySceneManager.instance.factoryStructureIODelesectAllEvent.AddListener(Deselect);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void Select()
    {
        this.selectionIndicator.SetActive(true);
        this.isSelected = true;
    }

    public void Deselect()
    {
        this.selectionIndicator.SetActive(false);
        this.isSelected = false;
    }

    // IMPLEMENTATION METHODS


}
