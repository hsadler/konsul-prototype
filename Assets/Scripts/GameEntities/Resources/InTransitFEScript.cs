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
        this.sr.sprite = GalaxySceneManager.instance.playerFactory.GetFactoryEntitySpriteByType(this.FactoryEntityType);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return "resource type: " +
            "\n  " + GalaxySceneManager.instance.sharedData.factoryEntityTypeToDisplayString[this.FactoryEntityType];
    }

    // IMPLEMENTATION METHODS


}
