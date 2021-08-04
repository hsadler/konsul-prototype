using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

    public GameObject harvesterPrefab;
    public GameObject storagePrefab;
    public GameObject splitterPrefab;
    public GameObject mergerPrefab;
    public GameObject mirrorPrefab;
    public GameObject photovotaicPrefab;
    public GameObject accumulatorPrefab;


    private IDictionary<int, GameObject> structureTypeToPrefab;


    // UNITY HOOKS

    private void Awake()
    {
        this.structureTypeToPrefab = new Dictionary<int, GameObject>()
        {
            { Constants.STRUCTURE_HARVESTER, this.harvesterPrefab },
            { Constants.STRUCTURE_STORAGE, this.storagePrefab },
            { Constants.STRUCTURE_SPLITTER, this.splitterPrefab },
            { Constants.STRUCTURE_MERGER, this.mergerPrefab },
            { Constants.STRUCTURE_MIRROR, this.mirrorPrefab },
            { Constants.STRUCTURE_PHOTOVOLTAIC, this.photovotaicPrefab },
            { Constants.STRUCTURE_ACCUMULATOR, this.accumulatorPrefab },
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
