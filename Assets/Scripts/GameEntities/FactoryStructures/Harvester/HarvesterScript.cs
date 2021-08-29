using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryHarvester
{


    public int FactoryEntityType { get; set; } = ConstFEType.HARVESTER;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public float launchImpulse = 0f;

    private FactoryStructureIOBehavior io;
    private int harvestedResource = ConstFEType.NONE;
    private int lastHarvestedResource = ConstFEType.NONE;


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
            if (this.harvestedResource == ConstFEType.NONE && other.gameObject.CompareTag("Planet"))
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

    // TODO: maybe replace this with a buffer queue
    private void CheckAndSendResource()
    {
        if (this.harvestedResource != ConstFEType.NONE)
        {
            if (this.io.ResourceIOsExist())
            {
                Vector3 launchDirection = this.io.GetNextSendDirection();
                GameObject prefab = GalaxySceneManager.instance.playerFactory.inTransitFEPrefab;
                GameObject go = Instantiate(
                    prefab,
                    this.transform.position + launchDirection,
                    Quaternion.identity
                );
                go.GetComponent<InTransitFEScript>().FactoryEntityType = this.harvestedResource;
                var feLaunchable = go.GetComponent<FactoryEntityLaunchable>();
                feLaunchable.SetLaunchForceAndDirection(this.launchImpulse, launchDirection);
                feLaunchable.Launch();
                this.harvestedResource = ConstFEType.NONE;
            }
        }
    }


}
