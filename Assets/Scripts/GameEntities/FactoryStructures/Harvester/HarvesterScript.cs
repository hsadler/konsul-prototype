using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour, IFactoryStructure, IFactoryHarvester
{


    public GameObject rawResourcePrefab;
    public float rawResourceLaunchImpulse = 3f;

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
        this.CheckAndSendResource();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.harvestedResource == Constants.ENTITY_TYPE_NONE && other.gameObject.CompareTag("Planet"))
        {
            PlanetScript pScript = other.gameObject.GetComponentInParent<PlanetScript>();
            this.harvestedResource = pScript.ExtractResource();
            this.lastHarvestedResource = this.harvestedResource;
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryStructureInfo()
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
                // TODO: this is assuming a raw resource, refactor in future
                GameObject rawResource = Instantiate(
                    this.rawResourcePrefab,
                    this.transform.position,
                    Quaternion.identity
                );
                var rrScript = rawResource.GetComponent<RawResourceScript>();
                rrScript.resourceType = this.harvestedResource;
                rrScript.SetLaunchForceAndDirection(this.rawResourceLaunchImpulse, launchDirection);
                this.harvestedResource = Constants.ENTITY_TYPE_NONE;
            }
        }
    }


}
