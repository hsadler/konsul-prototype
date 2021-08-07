using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorScript : MonoBehaviour
{


    public GameObject rawResourcePrefab;
    public float distributionPerSecond = 1f;
    public float rawResourceLaunchImpulse = 3f;


    private FactoryStructureIOBehavior io;
    private Queue<int> resourceBuffer = new Queue<int>();


    // UNITY HOOKS

    void Start()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        InvokeRepeating("DistributeResource", 0f, this.distributionPerSecond);
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Resource"))
        {
            // add to the buffer
            int resourceType = other.gameObject.GetComponent<RawResourceScript>().resourceType;
            this.resourceBuffer.Enqueue(resourceType);
            Object.Destroy(other.gameObject);
        }
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void DistributeResource()
    {
        if (this.resourceBuffer.Count > 0 && this.io.ResourceIOsExist())
        {
            Vector3 launchDirection = this.io.GetNextSendDirection();
            // TODO: this is assuming a raw resource, refactor in future
            GameObject rawResource = Instantiate(
                this.rawResourcePrefab,
                this.transform.position + launchDirection,
                Quaternion.identity
            );
            var rrScript = rawResource.GetComponent<RawResourceScript>();
            rrScript.resourceType = this.resourceBuffer.Dequeue();
            Debug.Log("sending resource from distributor: " + rrScript.resourceType);
            rrScript.SetLaunchForceAndDirection(this.rawResourceLaunchImpulse, launchDirection);
        }
    }


}
