using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProcessorScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryDistributor
{


    public int FactoryEntityType { get; set; } = ConstFEType.RESOURCE_PROCESSOR;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public float launchImpulse = 0f;
    public float processTime = 0f;

    private int processedFEType = ConstFEType.NONE;
    private const int STATUS_IDLE = 1;
    private const int STATUS_PROCESSING = 2;

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
        this.CheckAndProcessNextResource();
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return this.bufferQueue.GetStatus();
    }

    // IMPLEMENTATION METHODS

    // TODO: delete when ready
    private void DistributeItems()
    {
        if (this.IsStructureActive && !this.bufferQueue.IsEmpty() && this.io.ResourceIOsExist())
        {
            int feType = this.bufferQueue.GetNext();
            Vector3 launchDirection = this.io.GetNextSendDirection();
            GameObject prefab = GalaxySceneManager.instance.playerFactory.inTransitFEPrefab;
            GameObject go = Instantiate(
                prefab,
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

    private void CheckAndProcessNextResource()
    {
        // TODO: implement stub

        // logic:
        // if resource is avail
        // set status as "processing"
        // process item
        // set timout to call DistributeProcessed after processing time is complete
    }

    private void DistributeProcessed()
    {
        // TODO: implement stub

        // logic:
        // distribute the processed resource
        // set status to "idle"
    }


}
