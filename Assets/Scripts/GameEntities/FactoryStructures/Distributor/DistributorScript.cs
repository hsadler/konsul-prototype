using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorScript : MonoBehaviour, IFactoryStructure
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
        // resource reception
        if (other.gameObject.CompareTag("Resource"))
        {
            // add to the buffer
            var rrScript = other.gameObject.GetComponent<RawResourceScript>();
            if (rrScript.launcherGameObjectId != this.gameObject.GetInstanceID())
            {
                int resourceType = rrScript.resourceType;
                this.resourceBuffer.Enqueue(resourceType);
                Object.Destroy(other.gameObject);
            }
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryStructureInfo()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string formattedString = "resources in buffer: ";
        if (this.resourceBuffer.Count < 1)
        {
            return formattedString + "none";
        }
        IDictionary<int, int> resourceTypeToCount = new Dictionary<int, int>();
        foreach (int resourceType in this.resourceBuffer)
        {
            if (resourceTypeToCount.ContainsKey(resourceType))
            {
                resourceTypeToCount[resourceType] += 1;
            }
            else
            {
                resourceTypeToCount.Add(resourceType, 1);
            }
        }
        foreach (KeyValuePair<int, int> item in resourceTypeToCount)
        {
            formattedString += ("\n  " + gsm.sharedData.rawResourceTypeToDisplayName[item.Key] + ": " + item.Value.ToString());
        }
        return formattedString;
    }

    // IMPLEMENTATION METHODS

    private void DistributeResource()
    {
        if (this.resourceBuffer.Count > 0 && this.io.ResourceIOsExist())
        {
            Vector3 launchDirection = this.io.GetNextSendDirection();
            // TODO: this is assuming a raw resource, refactor in future
            GameObject rawResource = Instantiate(
                this.rawResourcePrefab,
                this.transform.position,
                Quaternion.identity
            );
            var rrScript = rawResource.GetComponent<RawResourceScript>();
            rrScript.launcherGameObjectId = this.gameObject.GetInstanceID();
            rrScript.resourceType = this.resourceBuffer.Dequeue();
            rrScript.SetLaunchForceAndDirection(this.rawResourceLaunchImpulse, launchDirection);
        }
    }


}
