using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{


    public GameObject inTransitFEPrefab;

    // resource prefabs
    public GameObject rawResourcePrefab;

    [Space(20)]

    // structure prefabs
    public GameObject harvesterPrefab;
    public GameObject distributorPrefab;
    public GameObject storagePrefab;
    public GameObject resourceProcessorPrefab;
    public GameObject mirrorPrefab;
    public GameObject photovoltaicPrefab;
    public GameObject accumulatorPrefab;
    // unit prefabs
    public GameObject workerPrefab;
    public GameObject probePrefab;
    public GameObject systemExpansionShipPrefab;
    private IDictionary<int, GameObject> entityTypeToPrefab;

    [Space(20)]

    // factory entity sprites
    public Sprite waterSprite;
    public Sprite gasSprite;
    public Sprite stoneSprite;
    public Sprite metalSprite;
    public Sprite organicsSprite;
    [Space(10)]
    public Sprite nitrogenSprite;
    public Sprite heliumSprite;
    public Sprite co2Sprite;
    public Sprite hydrogenSprite;
    public Sprite oxygenSprite;
    public Sprite silicatesSprite;
    public Sprite quartzSprite;
    public Sprite ironSprite;
    public Sprite copperSprite;
    public Sprite aluminumSprite;
    public Sprite leadSprite;
    public Sprite rareMetalsSprite;
    public Sprite biomassSprite;
    public Sprite cellCultureSprite;
    [Space(10)]
    public Sprite harvesterSprite;
    public Sprite distributorSprite;
    public Sprite storageSprite;
    public Sprite resourceProcessorSprite;
    public Sprite mirrorSprite;
    public Sprite photovoltaicSprite;
    public Sprite accumulatorSprite;
    public IDictionary<int, Sprite> entityTypeToSprite;

    private IDictionary<int, LinkedList<GameObject>> entityTypeToEntities = new Dictionary<int, LinkedList<GameObject>>();

    // raw resource to intermediate resource probabilities
    public IDictionary<int, IDictionary<int, float>> resourceToProcessedResources = new Dictionary<int, IDictionary<int, float>>()
    {
        { ConstFEType.WATER, new Dictionary<int, float>() {
            { ConstFEType.HYDROGEN, 66f },
            { ConstFEType.OXYGEN, 33f },
        }},
        { ConstFEType.GAS, new Dictionary<int, float>() {
            { ConstFEType.NITROGEN, 40f },
            { ConstFEType.HELIUM, 5f },
            { ConstFEType.CO2, 5f },
            { ConstFEType.HYDROGEN, 20f },
            { ConstFEType.OXYGEN, 30f },
        }},
        { ConstFEType.STONE, new Dictionary<int, float>() {
            { ConstFEType.SILICATES, 90f },
            { ConstFEType.QUARTZ, 10f },
        }},
        { ConstFEType.METAL, new Dictionary<int, float>() {
            { ConstFEType.IRON, 60f },
            { ConstFEType.COPPER, 20f },
            { ConstFEType.ALUMINUM, 10f },
            { ConstFEType.LEAD, 8f },
            { ConstFEType.RARE_METALS, 2f },
        }},
        { ConstFEType.ORGANICS, new Dictionary<int, float>() {
            { ConstFEType.BIOMASS, 98f },
            { ConstFEType.CELL_CULTURE, 2f },
        }},
    };


    // UNITY HOOKS

    private void Awake()
    {
        this.entityTypeToPrefab = new Dictionary<int, GameObject>()
        {
            // resources
            { ConstFEType.WATER, this.rawResourcePrefab },
            { ConstFEType.GAS, this.rawResourcePrefab },
            { ConstFEType.STONE, this.rawResourcePrefab },
            { ConstFEType.METAL, this.rawResourcePrefab },
            { ConstFEType.ORGANICS, this.rawResourcePrefab },
            // structures
            { ConstFEType.HARVESTER, this.harvesterPrefab },
            { ConstFEType.DISTRIBUTOR, this.distributorPrefab },
            { ConstFEType.STORAGE, this.storagePrefab },
            { ConstFEType.RESOURCE_PROCESSOR, this.resourceProcessorPrefab },
            { ConstFEType.MIRROR, this.mirrorPrefab },
            { ConstFEType.PHOTOVOLTAIC, this.photovoltaicPrefab },
            { ConstFEType.ACCUMULATOR, this.accumulatorPrefab },
            // units
            { ConstFEType.WORKER, this.workerPrefab },
        };
        this.entityTypeToSprite = new Dictionary<int, Sprite>()
        {
            // raw resources
            { ConstFEType.WATER, this.waterSprite },
            { ConstFEType.GAS, this.gasSprite },
            { ConstFEType.STONE, this.stoneSprite },
            { ConstFEType.METAL, this.metalSprite },
            { ConstFEType.ORGANICS, this.organicsSprite },
            // intermediate resources
            { ConstFEType.NITROGEN, this.nitrogenSprite },
            { ConstFEType.HELIUM, this.heliumSprite },
            { ConstFEType.CO2, this.co2Sprite },
            { ConstFEType.HYDROGEN, this.hydrogenSprite },
            { ConstFEType.OXYGEN, this.oxygenSprite },
            { ConstFEType.SILICATES, this.silicatesSprite },
            { ConstFEType.QUARTZ, this.quartzSprite },
            { ConstFEType.IRON, this.ironSprite },
            { ConstFEType.COPPER, this.copperSprite },
            { ConstFEType.ALUMINUM, this.aluminumSprite },
            { ConstFEType.LEAD, this.leadSprite },
            { ConstFEType.RARE_METALS, this.rareMetalsSprite },
            { ConstFEType.BIOMASS, this.biomassSprite },
            { ConstFEType.CELL_CULTURE, this.cellCultureSprite },
            // structures
            { ConstFEType.HARVESTER, this.harvesterSprite },
            { ConstFEType.DISTRIBUTOR, this.distributorSprite },
            { ConstFEType.STORAGE, this.storageSprite },
            { ConstFEType.RESOURCE_PROCESSOR, this.resourceProcessorSprite },
            { ConstFEType.MIRROR, this.mirrorSprite },
            { ConstFEType.PHOTOVOLTAIC, this.photovoltaicSprite },
            { ConstFEType.ACCUMULATOR, this.accumulatorSprite },
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

    // prefab and sprite APIs
    public GameObject GetFactoryEntityPrefabByType(int feType)
    {
        if (this.entityTypeToPrefab.ContainsKey(feType))
        {
            return this.entityTypeToPrefab[feType];
        }
        else
        {
            Debug.LogWarning("unable to fetch factory entity prefab by type: " + feType.ToString());
            return null;
        }
    }
    public Sprite GetFactoryEntitySpriteByType(int feType)
    {
        if (this.entityTypeToSprite.ContainsKey(feType))
        {
            return this.entityTypeToSprite[feType];
        }
        else
        {
            Debug.LogWarning("unable to fetch factory entity sprite by type: " + feType.ToString());
            return null;
        }
    }

    // resource processing API
    public int GetProcessedResourceFromResource(int resourceType)
    {
        if (this.resourceToProcessedResources.ContainsKey(resourceType))
        {
            return GalaxySceneManager.instance.functions.GetRandomTypeFromProbabilities(this.resourceToProcessedResources[resourceType]);
        }
        else
        {
            return ConstFEType.NONE;
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
