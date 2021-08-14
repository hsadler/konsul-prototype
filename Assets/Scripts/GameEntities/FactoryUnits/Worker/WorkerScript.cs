using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerScript : MonoBehaviour, IFactoryEntity, IFactoryUnit, IFactoryWorker
{


    public int FactoryEntityType { get; set; } = Constants.FACTORY_UNIT_ENTITY_TYPE_WORKER;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public float moveSpeed = 1f;
    public float interactionDistance = 1f;

    private IDictionary<int, int> inventory = new Dictionary<int, int>();
    private WorkerTask task;
    private int workerMode = Constants.WORKER_MODE_INIT;

    // fetch and build
    private GameObject selectedFetchStorage;

    // remove and store
    private GameObject selectedDeliveryStorage;


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.workerTaskQueue.AddNewWorker(this.gameObject);
    }

    void Update()
    {
        if (this.task == null)
        {
            return;
        }
        if (this.task.taskType == Constants.WORKER_TASK_TYPE_BUILD)
        {
            this.FetchAndBuildFactoryStructure();
        }
        else if (this.task.taskType == Constants.WORKER_TASK_TYPE_REMOVE)
        {
            this.RemoveAndStoreFactoryStructure();
        }
    }

    void OnDestroy()
    {
        GalaxySceneManager.instance.workerTaskQueue.RemoveWorker(this.gameObject);
    }


    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return "worker mode: doing stuff";
    }

    public void DoTask(WorkerTask task)
    {
        GalaxySceneManager.instance.workerTaskQueue.SetWorkerAsBusy(this.gameObject);
        this.task = task;
    }

    // IMPLEMENTATION METHODS

    private void InitWorker()
    {
        this.workerMode = Constants.WORKER_MODE_INIT;
        this.task = null;
        this.selectedFetchStorage = null;
        this.selectedDeliveryStorage = null;
    }

    // fetch and build

    private void FetchAndBuildFactoryStructure()
    {
        if (this.workerMode == Constants.WORKER_MODE_INIT)
        {
            this.workerMode = Constants.WORKER_MODE_FETCH;
        }
        else if (this.workerMode == Constants.WORKER_MODE_FETCH)
        {
            this.FetchFromStorage();
        }
        else if (this.workerMode == Constants.WORKER_MODE_BUILD)
        {
            this.BuildFactoryStructure();
        }
    }

    private void FetchFromStorage()
    {
        if (this.selectedFetchStorage == null)
        {
            this.SelectStorageForFetch();
        }
        // close enough to storage for retrieval of item
        else if (Vector3.Distance(this.transform.position, this.selectedFetchStorage.transform.position) < this.interactionDistance)
        {
            // retrieve and bump worker mode
            int retrieved = this.selectedFetchStorage.GetComponent<StorageScript>().Retrieve(this.task.structureFeType, this.gameObject);
            this.AddFactoryEntityToInventory(retrieved);
            this.workerMode = Constants.WORKER_MODE_BUILD;
        }
        // move closer to storage
        else
        {
            // face storage
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, this.selectedFetchStorage.transform.position - this.transform.position);
            // move
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                this.selectedFetchStorage.transform.position,
                this.moveSpeed * Time.deltaTime
            );
        }
    }

    private void BuildFactoryStructure()
    {
        // close enough to location to build
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {
            // remove structure from inventory
            this.RemoveFactoryEntityFromInventory(this.task.structureFeType);
            // activate the in-progress game object
            this.task.structure.GetComponent<FactoryStructureBehavior>().ActivateStructure();
            // set worker as available
            GalaxySceneManager.instance.workerTaskQueue.SetWorkerAsAvailable(this.gameObject);
            // init worker
            this.InitWorker();
        }
        // move closer to build location
        else
        {
            // face location
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, this.task.structure.transform.position - this.transform.position);
            // move
            this.transform.position = Vector3.MoveTowards(
                this.transform.position,
                this.task.structure.transform.position,
                this.moveSpeed * Time.deltaTime
            );
        }
    }

    private void SelectStorageForFetch()
    {
        List<GameObject> storages = GalaxySceneManager.instance.playerFactory.GetFactoryEntityListByType(Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE);
        float shortestDistance = Mathf.Infinity;
        GameObject closestStorage = null;
        foreach (GameObject storage in storages)
        {
            // storage is the closest and contains the needed item
            float d = Vector3.Distance(this.transform.position, storage.transform.position);
            if (d < shortestDistance)
            {
                if (storage.GetComponent<StorageScript>().Contains(this.task.structureFeType))
                {
                    shortestDistance = d;
                    closestStorage = storage;
                }
            }
        }
        if (closestStorage != null)
        {
            this.selectedFetchStorage = closestStorage;
        }
        else
        {
            // TODO: maybe declare task as unable to complete and free worker
        }
    }

    // remove and store

    private void RemoveAndStoreFactoryStructure()
    {
        // TODO: implement STUB
        // Debug.Log("Worker is carrying out order to remove structure at position: " + this.currentTask.position.ToString());
    }

    private void RemoveFactoryStructure()
    {

    }

    private void DeliverAndStoreFactoryStructure()
    {

    }

    private void SelectStorageForDelivery()
    {

    }

    // inventory

    private void AddFactoryEntityToInventory(int feType)
    {
        if (this.inventory.ContainsKey(feType))
        {
            this.inventory[feType] += 1;
        }
        else
        {
            this.inventory.Add(feType, 1);
        }
    }

    private int RemoveFactoryEntityFromInventory(int feType)
    {
        if (this.InventoryContainsFactoryEntity(feType))
        {
            this.inventory[feType] -= 1;
            return feType;
        }
        else
        {
            return Constants.ENTITY_TYPE_NONE;
        }
    }

    private bool InventoryContainsFactoryEntity(int feType)
    {
        return this.inventory.ContainsKey(feType) && this.inventory[feType] > 0;
    }


}
