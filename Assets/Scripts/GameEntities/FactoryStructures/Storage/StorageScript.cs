using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryStorage
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE;
    public int LauncherGameObjectId { get; set; }

    public bool IsStructureActive { get; set; } = false;

    private IDictionary<int, int> entityTypeToCount = new Dictionary<int, int>();


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // consume factory-entity
        if (this.IsStructureActive && other.gameObject.CompareTag("FactoryEntity"))
        {
            // don't consume inactive structures
            var fs = other.gameObject.GetComponent<IFactoryStructure>();
            if (fs != null && !fs.IsStructureActive)
            {
                return;
            }
            var fe = other.gameObject.GetComponent<IFactoryEntity>();
            this.StoreFactoryEntity(fe.FactoryEntityType);
            Object.Destroy(other.gameObject);
        }
    }

    // INTERFACE METHODS

    // TODO: maybe combine the query and reserve steps
    public bool QueryIsEntityInStorage(int feType)
    {
        return this.entityTypeToCount.ContainsKey(feType) && this.entityTypeToCount[feType] > 0;
    }

    public bool ReserveEntityForRetrieval(int feType, GameObject worker)
    {
        // TODO: implement stub
        return true;
    }

    public int RetrieveEntity(int feType, GameObject worker)
    {
        // TODO: implement stub
        return 1;
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
