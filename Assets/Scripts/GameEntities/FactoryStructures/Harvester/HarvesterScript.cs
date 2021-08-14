using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryHarvester
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_HARVESTER;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public GameObject rawResourcePrefab;
    public float launchImpulse = 1f;

    private FactoryStructureIOBehavior io;
    private int harvestedResource = Constants.ENTITY_TYPE_NONE;
    private int lastHarvestedResource = Constants.ENTITY_TYPE_NONE;


    // UNITY HOOKS

    void Start()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
    }

    void Update()
    {
        if (this.IsStructureActive)
        {
            this.CheckAndSendResource();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.IsStructureActive)
        {
            if (this.harvestedResource == Constants.ENTITY_TYPE_NONE && other.gameObject.CompareTag("Planet"))
            {
                PlanetScript pScript = other.gameObject.GetComponentInParent<PlanetScript>();
                this.harvestedResource = pScript.ExtractResource();
                this.lastHarvestedResource = this.harvestedResource;
            }
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return
            "last harvested resource: " +
            "\n  " + GalaxySceneManager.instance.sharedData.factoryEntityTypeToDisplayString[this.lastHarvestedResource];
    }

    // IMPLEMENTATION METHODS

    private void CheckAndSendResource()
    {
        if (this.harvestedResource != Constants.ENTITY_TYPE_NONE)
        {
            if (this.io.ResourceIOsExist())
            {
                Vector3 launchDirection = this.io.GetNextSendDirection();
                GameObject go = Instantiate(
                    this.rawResourcePrefab,
                    this.transform.position,
                    Quaternion.identity
                );
                go.GetComponent<RawResourceScript>().FactoryEntityType = this.harvestedResource;
                var feLaunchable = go.GetComponent<FactoryEntityLaunchable>();
                feLaunchable.SetLaunchForceAndDirection(this.launchImpulse, launchDirection);
                feLaunchable.Launch();
                this.harvestedResource = Constants.ENTITY_TYPE_NONE;
            }
        }
    }


}
