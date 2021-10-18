using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblerScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryAssembler
{

    // TODO: IMPLEMENT FROM DISTRIBUTOR EXAMPLE

    public int FactoryEntityType { get; set; } = ConstFEType.ASSEMBLER;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public float distributionPerSecond = 1f;
    public float launchImpulse = 0f;

    private FactoryStructureIOBehavior io;
    private FactoryEntityReceiver receiver;
    private FactoryEntityBufferQueue bufferQueue;
    private FactoryEntityLauncher launcher;

    // UNITY HOOKS

    void Awake()
    {
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
        this.bufferQueue = this.gameObject.GetComponent<FactoryEntityBufferQueue>();
        this.launcher = this.gameObject.GetComponent<FactoryEntityLauncher>();
    }

    void Start()
    {
        InvokeRepeating("DistributeItems", 0f, this.distributionPerSecond);
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
            this.launcher.Launch(feType, launchDirection, this.launchImpulse);
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
