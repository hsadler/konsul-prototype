using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour
{


    public GameObject rawResourcePrefab;
    public float rawResourceLaunchImpulse = 3f;

    private FactoryStructureIOBehavior io;
    private int harvestedResource = Constants.RESOURCE_TYPE_NONE;


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
        if (this.harvestedResource == Constants.RESOURCE_TYPE_NONE && other.gameObject.CompareTag("Planet"))
        {
            PlanetScript pScript = other.gameObject.GetComponentInParent<PlanetScript>();
            this.harvestedResource = pScript.ExtractResource();
        }
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void CheckAndSendResource()
    {
        if (this.harvestedResource != Constants.RESOURCE_TYPE_NONE)
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
                this.harvestedResource = Constants.RESOURCE_TYPE_NONE;
            }
        }
    }


}
