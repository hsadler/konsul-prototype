using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

    // structures
    public GameObject harvesterPrefab;
    public GameObject distributorPrefab;
    public GameObject storagePrefab;
    public GameObject mirrorPrefab;
    public GameObject photovotaicPrefab;
    public GameObject accumulatorPrefab;

    // units
    public GameObject workerPrefab;

    private Queue<WorkerTask> workerTaskQueue = new Queue<WorkerTask>();


    public IDictionary<int, string> structureTypeToDisplayString = new Dictionary<int, string>()
    {
        { Constants.FACTORY_STRUCTURE_TYPE_HARVESTER, "harvester" },
        { Constants.FACTORY_STRUCTURE_TYPE_DISTRIBUTOR, "distributor" },
        { Constants.FACTORY_STRUCTURE_TYPE_STORAGE, "storage" },
        { Constants.FACTORY_STRUCTURE_TYPE_MIRROR, "mirror" },
        { Constants.FACTORY_STRUCTURE_TYPE_PHOTOVOLTAIC, "photovoltaic" },
        { Constants.FACTORY_STRUCTURE_TYPE_ACCUMULATOR, "accumulator" },
    };

    private IDictionary<int, GameObject> structureTypeToPrefab;


    // UNITY HOOKS

    private void Awake()
    {
        this.structureTypeToPrefab = new Dictionary<int, GameObject>()
        {
            { Constants.FACTORY_STRUCTURE_TYPE_HARVESTER, this.harvesterPrefab },
            { Constants.FACTORY_STRUCTURE_TYPE_DISTRIBUTOR, this.distributorPrefab },
            { Constants.FACTORY_STRUCTURE_TYPE_STORAGE, this.storagePrefab },
            { Constants.FACTORY_STRUCTURE_TYPE_MIRROR, this.mirrorPrefab },
            { Constants.FACTORY_STRUCTURE_TYPE_PHOTOVOLTAIC, this.photovotaicPrefab },
            { Constants.FACTORY_STRUCTURE_TYPE_ACCUMULATOR, this.accumulatorPrefab },
        };
    }

    // INTERFACE METHODS

    public void PlaceFactoryStructure(int type, Vector3 placementPosition)
    {
        if (this.structureTypeToPrefab.ContainsKey(type))
        {
            // place factory structure prefab at mouse location
            GameObject factoryStructurePrefab = this.structureTypeToPrefab[type];
            GameObject fs = Instantiate(factoryStructurePrefab, placementPosition, Quaternion.identity);
            fs.GetComponentInChildren<FactoryStructureBehavior>().factoryStructureType = type;
        }
        else
        {
            Debug.LogWarning("Factory Structure type not found: " + type.ToString());
        }
    }


}
