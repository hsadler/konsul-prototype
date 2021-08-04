using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

    public GameObject harvesterPrefab;

    private IDictionary<int, GameObject> structureTypeToPrefab;


    // UNITY HOOKS

    private void Awake()
    {
        this.structureTypeToPrefab = new Dictionary<int, GameObject>()
        {
            { Constants.STRUCTURE_HARVESTER, this.harvesterPrefab }
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
