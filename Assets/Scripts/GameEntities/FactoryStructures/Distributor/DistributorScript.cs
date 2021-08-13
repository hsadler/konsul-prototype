using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryDistributor
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_DISTRIBUTOR;
    public int LauncherGameObjectId { get; set; }

    public bool IsStructureActive { get; set; } = false;

    public float distributionPerSecond = 1f;
    public float launchImpulse = 3f;

    private FactoryStructureIOBehavior io;
    private Queue<int> feBuffer = new Queue<int>();


    // UNITY HOOKS

    void Start()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        InvokeRepeating("DistributeItems", 0f, this.distributionPerSecond);
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: consider moving can-consume behavior to it's own script
        // factory-entity consumption
        if (this.IsStructureActive && other.gameObject.CompareTag("FactoryEntity"))
        {
            // don't consume inactive structures
            var fs = other.gameObject.GetComponent<IFactoryStructure>();
            if (fs != null && !fs.IsStructureActive)
            {
                return;
            }
            // add to the buffer
            var fe = other.gameObject.GetComponent<IFactoryEntity>();
            if (fe.LauncherGameObjectId != this.gameObject.GetInstanceID())
            {
                this.feBuffer.Enqueue(fe.FactoryEntityType);
                Object.Destroy(other.gameObject);
            }
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string formattedString = "items in buffer: ";
        if (this.feBuffer.Count < 1)
        {
            return formattedString;
        }
        IDictionary<int, int> feTypeToCount = new Dictionary<int, int>();
        foreach (int feType in this.feBuffer)
        {
            if (feTypeToCount.ContainsKey(feType))
            {
                feTypeToCount[feType] += 1;
            }
            else
            {
                feTypeToCount.Add(feType, 1);
            }
        }
        foreach (KeyValuePair<int, int> item in feTypeToCount)
        {
            formattedString += ("\n  " + gsm.sharedData.factoryEntityTypeToDisplayString[item.Key] + ": " + item.Value.ToString());
        }
        return formattedString;
    }

    // IMPLEMENTATION METHODS

    private void DistributeItems()
    {
        if (this.IsStructureActive && this.feBuffer.Count > 0 && this.io.ResourceIOsExist())
        {
            int feType = this.feBuffer.Dequeue();
            Vector3 launchDirection = this.io.GetNextSendDirection();
            GameObject fePrefab = GalaxySceneManager.instance.playerFactory.GetFactoryEntityPrefabByType(feType);
            GameObject go = Instantiate(
                fePrefab,
                this.transform.position,
                Quaternion.identity
            );
            var fe = go.GetComponent<IFactoryEntity>();
            fe.LauncherGameObjectId = this.gameObject.GetInstanceID();
            fe.FactoryEntityType = feType;
            go.GetComponent<FactoryEntityLaunchable>().SetLaunchForceAndDirection(this.launchImpulse, launchDirection);
        }
    }


}
