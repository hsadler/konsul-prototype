using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{


    public GameObject inTransitFEPrefab;

    private IDictionary<int, LinkedList<GameObject>> entityTypeToEntities = new Dictionary<int, LinkedList<GameObject>>();


    // UNITY HOOKS

    private void Awake()
    {

    }

    // INTERFACE METHODS

    // factory-entity creation API

    public GameObject CreateCursorFactoryStructure(int feType)
    {
        FactoryEntityTemplate feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(feType);
        if (feTemplate != null)
        {
            var go = Instantiate(feTemplate.prefab, Vector3.zero, Quaternion.identity);
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            fsb.GivePrePlacementAppearance();
            return go;
        }
        return null;
    }

    public GameObject CreateInProgressInProgressFactoryStructure(int feType, Vector3 placementPosition)
    {
        FactoryEntityTemplate feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(feType);
        if (feTemplate != null)
        {
            var go = Instantiate(feTemplate.prefab, placementPosition, Quaternion.identity);
            var fsb = go.GetComponent<FactoryStructureBehavior>();
            fsb.GiveGhostAppearance();
            return go;
        }
        else
        {
            Debug.LogWarning("Factory Entity type not found for in-progress placement: " + feType.ToString());
            return null;
        }
    }

    public GameObject CreateFactoryEntity(int feType, Vector3 placementPosition)
    {
        FactoryEntityTemplate feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(feType);
        if (feTemplate != null)
        {
            var go = Instantiate(feTemplate.prefab, placementPosition, Quaternion.identity);
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
            Debug.LogWarning("Factory Entity type not found for placement: " + feType.ToString());
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

    // resource processing API
    public int GetProcessedResourceFromResource(int resourceType)
    {
        FactoryEntityTemplate feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(resourceType);
        return GalaxySceneManager.instance.functions.GetRandomTypeFromProbabilities(feTemplate.processedTo);
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
