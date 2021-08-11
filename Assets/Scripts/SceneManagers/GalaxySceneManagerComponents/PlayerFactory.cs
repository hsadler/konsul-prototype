using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{


    // structure prefabs
    public GameObject harvesterPrefab;
    public GameObject distributorPrefab;
    public GameObject storagePrefab;
    public GameObject mirrorPrefab;
    public GameObject photovotaicPrefab;
    public GameObject accumulatorPrefab;
    private IDictionary<int, GameObject> entityTypeToPrefab;

    // unit prefabs
    public GameObject workerPrefab;
    public GameObject probePrefab;
    public GameObject systemExpansionShipPrefab;


    // UNITY HOOKS

    private void Awake()
    {
        this.entityTypeToPrefab = new Dictionary<int, GameObject>()
        {
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_HARVESTER, this.harvesterPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_DISTRIBUTOR, this.distributorPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE, this.storagePrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_MIRROR, this.mirrorPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_PHOTOVOLTAIC, this.photovotaicPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_ACCUMULATOR, this.accumulatorPrefab },
        };
    }

    // INTERFACE METHODS

    public void PlaceFactoryStructure(int factoryEntityType, Vector3 placementPosition)
    {
        if (this.entityTypeToPrefab.ContainsKey(factoryEntityType))
        {
            // place factory structure prefab at mouse location
            GameObject factoryStructurePrefab = this.entityTypeToPrefab[factoryEntityType];
            GameObject fs = Instantiate(factoryStructurePrefab, placementPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Factory Structure type not found: " + factoryEntityType.ToString());
        }
    }

    public void PlaceFactoryUnit(int factoryEntityType, Vector3 placementPosition)
    {
        // TODO: implement STUB
    }



}
