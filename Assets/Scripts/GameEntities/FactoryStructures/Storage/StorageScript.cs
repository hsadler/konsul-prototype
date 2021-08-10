using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour, IFactoryStructure, IFactoryStorage
{


    private IDictionary<int, int> resourceTypeToCount = new Dictionary<int, int>();


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // consume resource
        if (other.gameObject.CompareTag("Resource"))
        {
            int resourceType = other.gameObject.GetComponent<RawResourceScript>().resourceType;
            this.StoreResource(resourceType);
            Object.Destroy(other.gameObject);
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryStructureInfo()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string formattedString = "resources in storage: ";
        foreach (KeyValuePair<int, int> item in this.resourceTypeToCount)
        {
            formattedString += ("\n  " + gsm.sharedData.rawResourceTypeToDisplayName[item.Key] + ": " + item.Value.ToString());
        }
        return formattedString;
    }

    public void AdminPopulateStorage()
    {
        foreach (int resourceType in GalaxySceneManager.instance.sharedData.allResourceTypes)
        {
            this.StoreResource(resourceType, 1000);
        }
    }

    // IMPLEMENTATION METHODS

    private void StoreResource(int resourceType, int amount = 1)
    {
        if (this.resourceTypeToCount.ContainsKey(resourceType))
        {
            this.resourceTypeToCount[resourceType] += amount;
        }
        else
        {
            this.resourceTypeToCount.Add(resourceType, amount);
        }
    }


}
