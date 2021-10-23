using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductSelectItemScript : MonoBehaviour
{


    public int feType;
    public SpriteRenderer sr;

    private FactoryEntityTemplate feTemplate;


    // UNITY HOOKS

    void Start()
    {
        this.feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(this.feType);
        this.sr.sprite = feTemplate.sprite;
    }

    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown for selected factory entity product: " + this.feTemplate.displayName);
        GalaxySceneManager.instance.playerInput.SelectOutputProductForCurrentSelectedFactoryStructure(this.feType);
    }


}
