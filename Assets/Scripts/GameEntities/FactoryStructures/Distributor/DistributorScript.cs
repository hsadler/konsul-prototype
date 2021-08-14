using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryDistributor
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_DISTRIBUTOR;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public float distributionPerSecond = 1f;
    public float launchImpulse = 3f;

    private FactoryStructureIOBehavior io;
    private FactoryEntityReceiver receiver;


    // UNITY HOOKS

    void Awake()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
    }

    void Start()
    {
        InvokeRepeating("DistributeItems", 0f, this.distributionPerSecond);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        GalaxySceneManager gsm = GalaxySceneManager.instance;
        string formattedString = "items in buffer: ";
        if (this.receiver.feBuffer.Count < 1)
        {
            return formattedString;
        }
        IDictionary<int, int> feTypeToCount = new Dictionary<int, int>();
        foreach (int feType in this.receiver.feBuffer)
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
        if (this.IsStructureActive && this.receiver.feBuffer.Count > 0 && this.io.ResourceIOsExist())
        {
            int feType = this.receiver.feBuffer.Dequeue();
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
            var feLaunchable = go.GetComponent<FactoryEntityLaunchable>();
            feLaunchable.SetLaunchForceAndDirection(this.launchImpulse, launchDirection);
            feLaunchable.Launch();
        }
    }


}
