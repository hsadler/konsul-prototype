using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTransitFEScript : MonoBehaviour, IFactoryEntity, IInTransitFE
{


    public SpriteRenderer sr;

    public int FactoryEntityType { get; set; } = ConstFEType.NONE;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;


    // UNITY HOOKS

    void Start()
    {
        FactoryEntityTemplate feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(this.FactoryEntityType);
        this.sr.sprite = feTemplate.sprite;
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return "resource type: " +
            "\n  " + GalaxySceneManager.instance.feData.GetFETemplate(this.FactoryEntityType).displayName;
    }

    // IMPLEMENTATION METHODS


}
