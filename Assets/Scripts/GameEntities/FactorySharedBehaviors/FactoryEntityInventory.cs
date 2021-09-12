using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityInventory : MonoBehaviour
{


    public int capacity = 1;

    private int contentsCount = 0;
    private IDictionary<int, int> storageEntityTypeToCount = new Dictionary<int, int>();
    private IDictionary<int, int> reservedEntityTypeToCount = new Dictionary<int, int>();


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
            this.StoreFactoryEntity(feType, this.storageEntityTypeToCount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAvailable(int feType)
    {
        return this.storageEntityTypeToCount.ContainsKey(feType) && this.storageEntityTypeToCount[feType] > 0;
    }

    public bool IsReserveAvailable(int feType)
    {
        return this.reservedEntityTypeToCount.ContainsKey(feType) && this.reservedEntityTypeToCount[feType] > 0;
    }

    public bool Reserve(int feType)
    {
        if (this.IsAvailable(feType))
        {
            this.StoreFactoryEntity(this.Retrieve(feType), this.reservedEntityTypeToCount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ReleaseReservation(int feType)
    {
        if (this.IsReserveAvailable(feType))
        {
            this.StoreFactoryEntity(this.RetrieveReserved(feType), this.storageEntityTypeToCount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int Retrieve(int feType)
    {
        if (this.IsAvailable(feType))
        {
            return this.RetrieveFactoryEntity(feType, this.storageEntityTypeToCount);
        }
        else
        {
            return ConstFEType.NONE;
        }
    }

    public int RetrieveReserved(int feType)
    {
        if (this.IsReserveAvailable(feType))
        {
            return this.RetrieveFactoryEntity(feType, this.reservedEntityTypeToCount);
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
        foreach (KeyValuePair<int, int> item in this.storageEntityTypeToCount)
        {
            status += ("\n  " + gsm.feData.GetDisplayNameFromFEType(item.Key) + ": " + item.Value.ToString());
        }
        return status;
    }

    public void AdminPopulate(int amount = 1000, int filterFeGroup = 0)
    {
        foreach (int eType in GalaxySceneManager.instance.feData.GetAllFETypes())
        {

            if (filterFeGroup > 0)
            {
                if (GalaxySceneManager.instance.feData.GetFETemplate(eType).group == filterFeGroup)
                {
                    this.StoreFactoryEntity(eType, this.storageEntityTypeToCount, amount);
                }
            }
            else
            {
                this.StoreFactoryEntity(eType, this.storageEntityTypeToCount, amount);
            }
        }
    }

    // IMPLEMENTATION METHODS

    private void StoreFactoryEntity(int feType, IDictionary<int, int> storage, int amount = 1)
    {
        if (storage.ContainsKey(feType))
        {
            storage[feType] += amount;
        }
        else
        {
            storage.Add(feType, amount);
        }
        this.contentsCount += amount;
    }

    private int RetrieveFactoryEntity(int feType, IDictionary<int, int> storage)
    {
        storage[feType] -= 1;
        this.contentsCount -= 1;
        return feType;
    }


}
