using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryStorage
{

    // TODO: assumes consumption of raw resources, needs future refactor



    public int FactoryEntityType { get; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE;
    public bool IsStructureActive { get; set; } = false;

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

    public bool QueryIsResourceInStorage(int factoryEntityType)
    {
        return this.resourceTypeToCount.ContainsKey(factoryEntityType) && this.resourceTypeToCount[factoryEntityType] > 0;
    }

    public int RetrieveResource(int factoryEntityType)
    {
        // TODO: implement stub
        return 1;
    }

    public string GetStringFormattedFactoryEntityInfo()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string formattedString = "resources in storage: ";
        foreach (KeyValuePair<int, int> item in this.resourceTypeToCount)
        {
            formattedString += ("\n  " + gsm.sharedData.factoryEntityTypeToDisplayString[item.Key] + ": " + item.Value.ToString());
        }
        return formattedString;
    }

    public void AdminPopulateStorage()
    {
        foreach (int resourceType in GalaxySceneManager.instance.sharedData.allFactoryEntityTypes)
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
