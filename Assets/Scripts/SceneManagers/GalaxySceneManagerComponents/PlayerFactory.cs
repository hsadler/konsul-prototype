using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

    // resource prefabs
    public GameObject rawResourcePrefab;

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

    private IDictionary<int, LinkedList<GameObject>> entityTypeToEntities = new Dictionary<int, LinkedList<GameObject>>();


    // UNITY HOOKS

    private void Awake()
    {
        this.entityTypeToPrefab = new Dictionary<int, GameObject>()
        {
            // resources
            { Constants.RESOURCE_ENTITY_TYPE_WATER, this.rawResourcePrefab },
            { Constants.RESOURCE_ENTITY_TYPE_GAS, this.rawResourcePrefab },
            { Constants.RESOURCE_ENTITY_TYPE_STONE, this.rawResourcePrefab },
            { Constants.RESOURCE_ENTITY_TYPE_METAL, this.rawResourcePrefab },
            { Constants.RESOURCE_ENTITY_TYPE_ORGANICS, this.rawResourcePrefab },
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

    // factory-entity creation API
    public GameObject CreateCursorFactoryStructure(int factoryEntityType)
    {
        GameObject fsPrefab = this.GetFactoryEntityPrefabByType(factoryEntityType);
        if (fsPrefab != null)
        {
            var go = Instantiate(fsPrefab, Vector3.zero, Quaternion.identity);
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            fsb.GivePrePlacementAppearance();
            return go;
        }
        return null;
    }

    public GameObject CreateInProgressInProgressFactoryStructure(int factoryEntityType, Vector3 placementPosition)
    {
        GameObject factoryStructureInProgressPrefab = this.GetFactoryEntityPrefabByType(factoryEntityType);
        if (factoryStructureInProgressPrefab != null)
        {
            var go = Instantiate(factoryStructureInProgressPrefab, placementPosition, Quaternion.identity);
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            fsb.GiveGhostAppearance();
            return go;
        }
        else
        {
            Debug.LogWarning("Factory Entity type not found for in-progress placement: " + factoryEntityType.ToString());
            return null;
        }
    }

    public GameObject CreateFactoryEntity(int factoryEntityType, Vector3 placementPosition)
    {
        GameObject factoryEntityPrefab = this.GetFactoryEntityPrefabByType(factoryEntityType);
        if (factoryEntityPrefab != null)
        {
            var go = Instantiate(factoryEntityPrefab, placementPosition, Quaternion.identity);
            // immediately activate structures
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            if (fsb != null)
            {
                fsb.ActivateStructure();
            }
            return go;
        }
        else
        {
            Debug.LogWarning("Factory Entity type not found for placement: " + factoryEntityType.ToString());
            return null;
        }
    }

    // registry API
    public void AddFactoryEntityToRegistry(GameObject feGO)
    {
        // Debug.Log("adding FE to registry: " + feGO.name);
        var fe = feGO.GetComponent<IFactoryEntity>();
        if (this.entityTypeToEntities.ContainsKey(fe.FactoryEntityType))
        {
            this.entityTypeToEntities[fe.FactoryEntityType].AddFirst(feGO);
        }
        else
        {
            var linkedList = new LinkedList<GameObject>();
            linkedList.AddFirst(feGO);
            this.entityTypeToEntities.Add(fe.FactoryEntityType, linkedList);
        }
        // Debug.Log(this.entityTypeToEntities.ToString());
    }

    public void RemoveFactoryEntityFromRegistry(GameObject feGO)
    {
        // Debug.Log("attempting to remove FE from registry: " + feGO.name);
        var fe = feGO.GetComponent<IFactoryEntity>();
        LinkedList<GameObject> feLinkedList = this.GetFactoryEntityLinkedListByType(fe.FactoryEntityType);
        GameObject toRemove = null;
        foreach (GameObject currFeGO in feLinkedList)
        {
            if (currFeGO == feGO)
            {
                toRemove = currFeGO;
            }
        }
        if (toRemove != null)
        {
            feLinkedList.Remove(toRemove);
        }
        // Debug.Log(this.entityTypeToEntities.ToString());
    }

    public List<GameObject> GetFactoryEntityListByType(int factoryEntityType)
    {
        return this.GetFactoryEntityLinkedListByType(factoryEntityType).ToList<GameObject>();
    }

    // prefab API
    public GameObject GetFactoryEntityPrefabByType(int factoryEntityType)
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

    // IMPLEMENTATION METHODS

    public LinkedList<GameObject> GetFactoryEntityLinkedListByType(int factoryEntityType)
    {
        if (this.entityTypeToEntities.ContainsKey(factoryEntityType))
        {
            return this.entityTypeToEntities[factoryEntityType];
        }
        else
        {
            return new LinkedList<GameObject>();
        }
    }


}
