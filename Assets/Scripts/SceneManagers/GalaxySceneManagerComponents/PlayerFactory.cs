using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

    public GameObject harvesterPrefab;
    public GameObject distributorPrefab;
    public GameObject storagePrefab;
    public GameObject mirrorPrefab;
    public GameObject photovotaicPrefab;
    public GameObject accumulatorPrefab;


    public IDictionary<int, string> structureTypeToDisplayString = new Dictionary<int, string>()
    {
        { Constants.STRUCTURE_TYPE_HARVESTER, "harvester" },
        { Constants.STRUCTURE_TYPE_DISTRIBUTOR, "distributor" },
        { Constants.STRUCTURE_TYPE_STORAGE, "storage" },
        { Constants.STRUCTURE_TYPE_MIRROR, "mirror" },
        { Constants.STRUCTURE_TYPE_PHOTOVOLTAIC, "photovoltaic" },
        { Constants.STRUCTURE_TYPE_ACCUMULATOR, "accumulator" },
    };

    private IDictionary<int, GameObject> structureTypeToPrefab;


    // UNITY HOOKS

    private void Awake()
    {
        this.structureTypeToPrefab = new Dictionary<int, GameObject>()
        {
            { Constants.STRUCTURE_TYPE_HARVESTER, this.harvesterPrefab },
            { Constants.STRUCTURE_TYPE_DISTRIBUTOR, this.distributorPrefab },
            { Constants.STRUCTURE_TYPE_STORAGE, this.storagePrefab },
            { Constants.STRUCTURE_TYPE_MIRROR, this.mirrorPrefab },
            { Constants.STRUCTURE_TYPE_PHOTOVOLTAIC, this.photovotaicPrefab },
            { Constants.STRUCTURE_TYPE_ACCUMULATOR, this.accumulatorPrefab },
        };
    }

    // INTERFACE METHODS

    public void PlaceFactoryStructure(int type, Vector3 placementPosition)
    {
        if (this.structureTypeToPrefab.ContainsKey(type))
        {
            // place factory structure prefab at mouse location
            GameObject factoryStructurePrefab = this.structureTypeToPrefab[type];
            Instantiate(factoryStructurePrefab, placementPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Factory Structure type not found: " + type.ToString());
        }
    }


}
