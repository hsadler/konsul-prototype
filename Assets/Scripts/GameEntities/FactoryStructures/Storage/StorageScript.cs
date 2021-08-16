using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour, IFactoryEntity, IFactoryStructure, IFactoryStorage
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public bool IsStructureActive { get; set; } = false;

    private FactoryEntityReceiver receiver;
    private FactoryEntityInventory inventory;


    // UNITY HOOKS

    void Awake()
    {
        this.receiver = this.gameObject.GetComponent<FactoryEntityReceiver>();
        this.inventory = this.gameObject.GetComponent<FactoryEntityInventory>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (this.inventory.CapacityFull())
        {
            this.receiver.SetCanReceive(false);
        }
        else
        {
            this.receiver.SetCanReceive(true);
            this.LoadFromReceiverBuffer();
        }
    }

    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return this.inventory.GetStatus();
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


}
