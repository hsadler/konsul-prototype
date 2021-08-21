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

    private FactoryEntityInventory inventory;
    // private IDictionary<int, int> inventory = new Dictionary<int, int>();
    private WorkerTask task;
    private int workerMode = Constants.WORKER_MODE_INIT;

    // fetch and build
    private GameObject selectedFetchStorage;

    // remove and store
    private GameObject selectedDeliveryStorage;


    // UNITY HOOKS

    void Awake()
    {
        GalaxySceneManager.instance.workerTaskQueue.AddNewWorker(this.gameObject);
        this.inventory = this.GetComponent<FactoryEntityInventory>();
    }

    void Start()
    {
    }

    void Update()
    {
        if (this.task == null)
        {
            return;
        }
        if (this.task.isCancelled)
        {
            this.InitWorker();
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
        return this.inventory.GetStatus();
    }

    public void DoTask(WorkerTask task)
    {
        // Debug.Log("worker assigned task with id: " + task.taskId.ToString());
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
        GalaxySceneManager.instance.workerTaskQueue.SetWorkerAsAvailable(this.gameObject);
    }

    // fetch and build

    private void FetchAndBuildFactoryStructure()
    {
        // in-progress structure has been removed, task can be cancelled via worker initialization
        if (this.task.structure == null)
        {
            this.InitWorker();
            return;
        }
        if (this.workerMode == Constants.WORKER_MODE_INIT)
        {
            // Debug.Log("setting worker mode to fetch");
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
            // Debug.Log("retrieving type: " + this.task.structureFeType.ToString() + " from storage");
            // TODO: account for retrieved to be NONE feType
            // retrieve and bump worker mode
            int retrieved = this.selectedFetchStorage.GetComponent<FactoryEntityInventory>().Retrieve(this.task.structureFeType);
            this.inventory.Store(retrieved);
            this.workerMode = Constants.WORKER_MODE_BUILD;
        }
        // move closer to fetch storage
        else
        {
            this.HandleMoveTowardsPosition(this.selectedFetchStorage.transform.position);
        }
    }

    private void BuildFactoryStructure()
    {
        // close enough to location to build
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {
            // Debug.Log("placing structure of type: " + this.task.structureFeType.ToString());
            // remove structure from inventory
            this.inventory.Retrieve(this.task.structureFeType);
            // activate the in-progress game object
            this.task.structure.GetComponent<FactoryStructureBehavior>().ActivateStructure();
            // init worker
            this.InitWorker();
        }
        // move closer to build location
        else
        {
            this.HandleMoveTowardsPosition(this.task.structure.transform.position);
        }
    }

    private void SelectStorageForFetch()
    {
        // Debug.Log("selecting storage for fetch");
        List<GameObject> storages = GalaxySceneManager.instance.playerFactory.GetFactoryEntityListByType(Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE);
        float shortestDistance = Mathf.Infinity;
        GameObject closestStorage = null;
        foreach (GameObject storage in storages)
        {
            // storage is the closest and contains the needed item
            float d = Vector3.Distance(this.transform.position, storage.transform.position);
            if (d < shortestDistance)
            {
                if (storage.GetComponent<FactoryEntityInventory>().Contains(this.task.structureFeType))
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
        if (this.workerMode == Constants.WORKER_MODE_INIT)
        {
            // Debug.Log("setting worker mode to remove");
            this.workerMode = Constants.WORKER_MODE_REMOVE;
        }
        else if (this.workerMode == Constants.WORKER_MODE_REMOVE)
        {
            this.RemoveFactoryStructure();
        }
        else if (this.workerMode == Constants.WORKER_MODE_STORE)
        {
            this.DeliverAndStoreFactoryStructure();
        }
    }

    private void RemoveFactoryStructure()
    {
        // close enough to location to remove
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {
            // Debug.Log("removing structure of type: " + this.task.structureFeType.ToString());
            // remove the structure
            Object.Destroy(this.task.structure);
            // add structure to inventory
            this.inventory.Store(this.task.structureFeType);
            // bump mode
            this.workerMode = Constants.WORKER_MODE_STORE;
        }
        // move closer to build location
        else
        {
            this.HandleMoveTowardsPosition(this.task.structure.transform.position);
        }
    }

    private void DeliverAndStoreFactoryStructure()
    {
        if (this.selectedDeliveryStorage == null)
        {
            this.SelectStorageForDelivery();
        }
        // close enough to deliver to storage
        else if (Vector3.Distance(this.transform.position, this.selectedDeliveryStorage.transform.position) < this.interactionDistance)
        {
            // remove structure from inventory and deposite to storage
            this.selectedDeliveryStorage.GetComponent<FactoryEntityInventory>().Store(
                this.inventory.Retrieve(this.task.structureFeType)
            );
            // init worker
            this.InitWorker();
        }
        // do move
        else
        {
            this.HandleMoveTowardsPosition(this.selectedDeliveryStorage.transform.position);
        }
    }

    private void SelectStorageForDelivery()
    {
        // Debug.Log("selecting storage for delivery");
        List<GameObject> storages = GalaxySceneManager.instance.playerFactory.GetFactoryEntityListByType(Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE);
        float shortestDistance = Mathf.Infinity;
        GameObject closestStorage = null;
        foreach (GameObject storage in storages)
        {
            float d = Vector3.Distance(this.transform.position, storage.transform.position);
            if (d < shortestDistance)
            {
                shortestDistance = d;
                closestStorage = storage;
            }
        }
        if (closestStorage != null)
        {
            this.selectedDeliveryStorage = closestStorage;
        }
        else
        {
            // TODO: maybe declare task as unable to complete and free worker
        }
    }

    // helpers

    private void HandleMoveTowardsPosition(Vector3 targetPosition)
    {
        // face position
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPosition - this.transform.position);
        // move
        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            targetPosition,
            this.moveSpeed * Time.deltaTime
        );
    }


}
