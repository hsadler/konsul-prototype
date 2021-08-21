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
    private FactoryEntityBufferQueue bufferQueue;

    // UNITY HOOKS

    void Awake()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
        this.bufferQueue = this.gameObject.GetComponent<FactoryEntityBufferQueue>();
    }

    void Start()
    {
        InvokeRepeating("DistributeItems", 0f, this.distributionPerSecond);
    }

    void Update()
    {
        if (this.bufferQueue.CapacityFull())
        {
            this.receiver.SetCanReceive(false);
        }
        else
        {
            this.receiver.SetCanReceive(true);
            this.LoadFromBufferQueue();
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return this.bufferQueue.GetStatus();
    }

    // IMPLEMENTATION METHODS

    private void DistributeItems()
    {
        if (this.IsStructureActive && !this.bufferQueue.IsEmpty() && this.io.ResourceIOsExist())
        {
            int feType = this.bufferQueue.GetNext();
            Vector3 launchDirection = this.io.GetNextSendDirection();
            GameObject fePrefab = GalaxySceneManager.instance.playerFactory.GetFactoryEntityPrefabByType(feType);
            GameObject go = Instantiate(
                fePrefab,
                this.transform.position + launchDirection,
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

    private void LoadFromBufferQueue()
    {
        // NOTE: this implementation may cause lost resources if reciever buffer 
        // contains more items than remaining capacity
        List<int> buffer = this.receiver.GetBuffer();
        foreach (int feType in buffer)
        {
            this.bufferQueue.Add(feType);
        }
    }


}
