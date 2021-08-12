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
            // structures
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_HARVESTER, this.harvesterPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_DISTRIBUTOR, this.distributorPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE, this.storagePrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_MIRROR, this.mirrorPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_PHOTOVOLTAIC, this.photovotaicPrefab },
            { Constants.FACTORY_STRUCTURE_ENTITY_TYPE_ACCUMULATOR, this.accumulatorPrefab },
            // units
            { Constants.FACTORY_UNIT_ENTITY_TYPE_WORKER, this.workerPrefab },
        };
    }

    // INTERFACE METHODS

    public GameObject CreateCursorFactoryStructure(int factoryEntityType)
    {
        GameObject fsPrefab = this.GetFactoryEntityPrefabByType(factoryEntityType);
        if (fsPrefab != null)
        {
            var go = Instantiate(fsPrefab, Vector3.zero, Quaternion.identity);
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            fsb.SetIsStructureActive(false);
            fsb.GivePrePlacementAppearance();
            return go;
        }
        return null;
    }

    public GameObject PlaceInProgressInProgressFactoryStructure(int factoryEntityType, Vector3 placementPosition)
    {
        GameObject factoryStructureInProgressPrefab = this.GetFactoryEntityPrefabByType(factoryEntityType);
        if (factoryStructureInProgressPrefab != null)
        {
            var go = Instantiate(factoryStructureInProgressPrefab, placementPosition, Quaternion.identity);
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            fsb.SetIsStructureActive(false);
            fsb.GiveGhostAppearance();
            return go;
        }
        else
        {
            Debug.LogWarning("Factory Entity type not found for in-progress placement: " + factoryEntityType.ToString());
            return null;
        }
    }

    public void PlaceFactoryEntity(int factoryEntityType, Vector3 placementPosition)
    {
        GameObject factoryEntityPrefab = this.GetFactoryEntityPrefabByType(factoryEntityType);
        if (factoryEntityPrefab != null)
        {
            Instantiate(factoryEntityPrefab, placementPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Factory Entity type not found for placement: " + factoryEntityType.ToString());
        }
    }

    // IMPLEMENTATION METHODS

    private GameObject GetFactoryEntityPrefabByType(int factoryEntityType)
    {
        if (this.entityTypeToPrefab.ContainsKey(factoryEntityType))
        {
            return this.entityTypeToPrefab[factoryEntityType];
        }
        else
        {
            Debug.LogWarning("unable to fetch factory entity prefab by type: " + factoryEntityType.ToString());
            return null;
        }
    }


}
