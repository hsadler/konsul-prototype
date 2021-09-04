using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityInventory : MonoBehaviour
{


    public int capacity = 1;

    private int contentsCount = 0;
    private IDictionary<int, int> entityTypeToCount = new Dictionary<int, int>();


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public bool Store(int feType)
    {
        if (this.contentsCount < this.capacity)
        {
            this.StoreFactoryEntity(feType);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Contains(int feType)
    {
        return this.entityTypeToCount.ContainsKey(feType) && this.entityTypeToCount[feType] > 0;
    }

    public int Retrieve(int feType)
    {
        if (this.Contains(feType))
        {
            return this.RetrieveFactoryEntity(feType);
        }
        else
        {
            return ConstFEType.NONE;
        }
    }

    public bool CapacityFull()
    {
        return this.contentsCount >= this.capacity;
    }

    public string GetStatus()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string status = "inventory (" + this.contentsCount.ToString() + "/" + this.capacity.ToString() + "): ";
        foreach (KeyValuePair<int, int> item in this.entityTypeToCount)
        {
            // status += ("\n  " + gsm.sharedData.factoryEntityTypeToDisplayString[item.Key] + ": " + item.Value.ToString());
            status += ("\n  " + gsm.feData.GetFETemplate(item.Key).displayName + ": " + item.Value.ToString());
        }
        return status;
    }

    public void AdminPopulate()
    {
        foreach (int eType in GalaxySceneManager.instance.feData.GetAllFETypes())
        {
            this.StoreFactoryEntity(eType, 1000);
        }
    }

    // IMPLEMENTATION METHODS

    private void StoreFactoryEntity(int feType, int amount = 1)
    {
        if (this.entityTypeToCount.ContainsKey(feType))
        {
            this.entityTypeToCount[feType] += amount;
        }
        else
        {
            this.entityTypeToCount.Add(feType, amount);
        }
        this.contentsCount += amount;
    }

    private int RetrieveFactoryEntity(int feType)
    {
        this.entityTypeToCount[feType] -= 1;
        this.contentsCount -= 1;
        return feType;
    }


}
