using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxScript : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.playerInput.selectionBox = this.gameObject;
        this.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS


}
