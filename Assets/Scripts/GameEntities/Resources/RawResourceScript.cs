using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawResourceScript : MonoBehaviour, IFactoryEntity, IFactoryResource
{


    public SpriteRenderer sr;

    public int FactoryEntityType { get; set; } = Constants.ENTITY_TYPE_NONE;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;


    // UNITY HOOKS

    void Start()
    {
        this.sr.color = GalaxySceneManager.instance.sharedData.rawResourceTypeToColor[this.FactoryEntityType];
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return "raw resource type: " +
            "\n  " + GalaxySceneManager.instance.sharedData.factoryEntityTypeToDisplayString[this.FactoryEntityType];
    }

    // IMPLEMENTATION METHODS


}
