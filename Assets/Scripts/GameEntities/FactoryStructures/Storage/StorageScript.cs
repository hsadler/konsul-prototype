using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour, IFactoryStructure
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
            if (this.resourceTypeToCount.ContainsKey(resourceType))
            {
                this.resourceTypeToCount[resourceType] += 1;
            }
            else
            {
                this.resourceTypeToCount.Add(resourceType, 1);
            }
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

    // IMPLEMENTATION METHODS


}
