using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryStorage
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    private FactoryEntityReceiver receiver;

    private IDictionary<int, int> entityTypeToCount = new Dictionary<int, int>();


    // UNITY HOOKS

    void Awake()
    {
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
    }

    void Start()
    {

    }

    void Update()
    {
        this.LoadFromReceiverBuffer();
    }

    // INTERFACE METHODS

    public bool Store(int feType)
    {
        this.StoreFactoryEntity(feType);
        return true;
    }

    // TODO: maybe combine the query and reserve steps
    public bool Contains(int feType)
    {
        return this.entityTypeToCount.ContainsKey(feType) && this.entityTypeToCount[feType] > 0;
    }

    public bool ReserveForRetrieval(int feType, GameObject worker)
    {
        // TODO: implement stub
        return true;
    }

    public int Retrieve(int feType, GameObject worker)
    {
        // TODO: implement stub
        return feType;
    }

    public string GetStringFormattedFactoryEntityInfo()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string formattedString = "items in storage: ";
        foreach (KeyValuePair<int, int> item in this.entityTypeToCount)
        {
            formattedString += ("\n  " + gsm.sharedData.factoryEntityTypeToDisplayString[item.Key] + ": " + item.Value.ToString());
        }
        return formattedString;
    }

    public void AdminPopulateStorage()
    {
        foreach (int eType in GalaxySceneManager.instance.sharedData.allFactoryEntityTypes)
        {
            this.StoreFactoryEntity(eType, 1000);
        }
    }

    // IMPLEMENTATION METHODS

    private void LoadFromReceiverBuffer()
    {
        while (this.receiver.feBuffer.Count > 0)
        {
            this.StoreFactoryEntity(this.receiver.feBuffer.Dequeue());
        }
    }

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
    }


}
