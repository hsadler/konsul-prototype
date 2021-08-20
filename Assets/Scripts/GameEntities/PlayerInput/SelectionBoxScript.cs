using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxScript : MonoBehaviour
{


    private BoxCollider2D col;


    // UNITY HOOKS

    void Awake()
    {
        this.col = this.gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        GalaxySceneManager.instance.playerInput.selectionBox = this.gameObject;
        this.gameObject.SetActive(false);
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS


}
