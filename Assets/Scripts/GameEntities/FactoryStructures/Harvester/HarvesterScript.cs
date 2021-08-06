using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour
{


    public GameObject rawResourcePrefab;

    private FactoryStructureIOBehavior io;

    private int ioLimit = 3;
    private int harvestedResource = Constants.RESOURCE_TYPE_NONE;


    // UNITY HOOKS

    void Start()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        this.io.ioLimit = this.ioLimit;
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
                Vector3 sendDirection = this.io.GetNextSendDirection();
                GameObject rawResource = Instantiate(this.rawResourcePrefab, this.transform.position, Quaternion.identity);
                var rrScript = rawResource.GetComponentInChildren<RawResourceScript>();
                rrScript.resourceType = this.harvestedResource;
                rrScript.SetLaunchForceAndDirection(1f, sendDirection);
                this.harvestedResource = Constants.RESOURCE_TYPE_NONE;
            }
        }
    }


}
