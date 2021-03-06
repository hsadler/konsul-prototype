using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineryScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryFurnace
{


    public int FactoryEntityType { get; set; } = ConstFEType.REFINERY;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    public float launchImpulse = 0f;
    public float processTime = 0f;

    public int productFEType;
    private IDictionary<int, int> inputFETypeToCount;
    private int status;
    private const int STATUS_IDLE = 1;
    private const int STATUS_PROCESSING = 2;
    private const int STATUS_DISTRIBUTING = 3;

    private FactoryStructureIOBehavior io;
    private FactoryEntityReceiver receiver;
    private FactoryEntityInventory inventory;
    private FactoryEntityLauncher launcher;

    // UNITY HOOKS

    void Awake()
    {
        this.status = STATUS_IDLE;
        this.io = this.gameObject.GetComponent<FactoryStructureIOBehavior>();
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
        this.inventory = this.gameObject.GetComponent<FactoryEntityInventory>();
        this.launcher = this.gameObject.GetComponent<FactoryEntityLauncher>();
        this.SetProductFEType(this.productFEType);
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
        if (this.inventory.CapacityFull())
        {
            this.receiver.SetCanReceive(false);
        }
        else
        {
            this.receiver.SetCanReceive(true);
            this.LoadFromReceiverBuffer();
        }
        if (this.status == STATUS_IDLE)
        {
            this.CheckAndRefineNextResource();
        }
        else if (this.status == STATUS_DISTRIBUTING)
        {
            this.DistributeProduct();
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return "product: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.productFEType) +
            "\n\n" +
            this.inventory.GetStatus();
    }

    // IMPLEMENTATION METHODS

    private void LoadFromReceiverBuffer()
    {
        // NOTE: this implementation may cause lost resources if buffer 
        // contains more items than remaining storage capacity
        List<int> buffer = this.receiver.GetBuffer();
        foreach (int feType in buffer)
        {
            this.inventory.Store(feType);
        }
    }

    private void CheckAndRefineNextResource()
    {
        if (this.productFEType != ConstFEType.NONE && this.inputFETypeToCount != null)
        {
            if (this.inventory.IsMultipleAvailable(this.inputFETypeToCount))
            {
                this.inventory.RetrieveMultiple(inputFETypeToCount);
                this.status = STATUS_PROCESSING;
                Invoke("DistributeProduct", this.processTime);
            }
        }
    }

    private void DistributeProduct()
    {
        if (this.io.ResourceIOsExist())
        {
            // launch furnaced resource
            Vector3 launchDirection = this.io.GetNextSendDirection();
            this.launcher.Launch(this.productFEType, launchDirection, this.launchImpulse);
            this.status = STATUS_IDLE;
        }
        else
        {
            // set status in order to keep trying to distribute until successfull
            this.status = STATUS_DISTRIBUTING;
        }
    }

    private void SetProductFEType(int productFEType)
    {
        this.productFEType = productFEType;
        FactoryEntityTemplate feTemplate = GalaxySceneManager.instance.feData.GetFETemplate(this.productFEType);
        this.inputFETypeToCount = feTemplate.refinedFrom;
    }


}
