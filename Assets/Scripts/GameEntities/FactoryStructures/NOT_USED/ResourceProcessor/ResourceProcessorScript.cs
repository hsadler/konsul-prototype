using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProcessorScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryResourceProcessor
{


    public int FactoryEntityType { get; set; } = ConstFEType.RESOURCE_PROCESSOR;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public float launchImpulse = 0f;
    public float processTime = 0f;

    private int processedFEType;
    private int status;
    private const int STATUS_IDLE = 1;
    private const int STATUS_PROCESSING = 2;
    private const int STATUS_DISTRIBUTING = 3;

    private FactoryStructureIOBehavior io;
    private FactoryEntityReceiver receiver;
    private FactoryEntityBufferQueue bufferQueue;
    private FactoryEntityLauncher launcher;

    // UNITY HOOKS

    void Awake()
    {
        this.status = STATUS_IDLE;
        this.processedFEType = ConstFEType.NONE;
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
        this.bufferQueue = this.gameObject.GetComponent<FactoryEntityBufferQueue>();
        this.launcher = this.gameObject.GetComponent<FactoryEntityLauncher>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!this.IsStructureActive)
        {
            return;
        }
        if (this.bufferQueue.CapacityFull())
        {
            this.receiver.SetCanReceive(false);
        }
        else
        {
            this.receiver.SetCanReceive(true);
            this.LoadFromBufferQueue();
        }
        if (this.status == STATUS_IDLE)
        {
            this.CheckAndProcessNextResource();
        }
        else if (this.status == STATUS_DISTRIBUTING)
        {
            this.DistributeProcessed();
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return this.bufferQueue.GetStatus();
    }

    // IMPLEMENTATION METHODS

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
        if (!this.bufferQueue.IsEmpty())
        {
            this.status = STATUS_PROCESSING;
            this.processedFEType = GalaxySceneManager.instance.playerFactory.GetProcessedResourceFromResource(this.bufferQueue.GetNext());
            Invoke("DistributeProcessed", this.processTime);
        }
    }

    private void DistributeProcessed()
    {
        if (this.io.ResourceIOsExist())
        {
            // launch processed resource
            Vector3 launchDirection = this.io.GetNextSendDirection();
            this.launcher.Launch(this.processedFEType, launchDirection, this.launchImpulse);
            this.status = STATUS_IDLE;
            this.processedFEType = ConstFEType.NONE;
        }
        else
        {
            // set status in order to keep trying to distribute until successfull
            this.status = STATUS_DISTRIBUTING;
        }
    }


}
