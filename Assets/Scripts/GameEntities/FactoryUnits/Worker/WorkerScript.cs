using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerScript : MonoBehaviour, IFactoryEntity, IFactoryUnit, IFactoryWorker
{


    public int FactoryEntityType { get; set; } = ConstFEType.WORKER;
    public int LauncherGameObjectId { get; set; }
    public bool InTransit { get; set; } = false;

    public float moveSpeed = 1f;
    public float interactionDistance = 1f;

    private FactoryEntityInventory inventory;
    private WorkerTask task;
    private int workerMode = ConstWorker.MODE_INIT;

    private GameObject selectedStorage;
    private FactoryEntityInventory selectedStorageInventory;
    private bool hasStorageReservation = false;


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
        if (this.task.taskType == ConstWorker.TASK_TYPE_FETCH_AND_PLACE)
        {
            this.FetchAndPlaceFactoryStructure();
        }
        else if (this.task.taskType == ConstWorker.TASK_TYPE_FETCH_AND_ADD_CONSTITUENT_PART)
        {
            this.FetchAndAddConstituentPartToFactoryStructure();
        }
        else if (this.task.taskType == ConstWorker.TASK_TYPE_REMOVE_AND_STORE)
        {
            this.RemoveAndStoreFactoryStructure();
        }
        else if (this.task.taskType == ConstWorker.TASK_TYPE_REMOVE_CONSTITUENT_PART_AND_STORE)
        {
            this.RemoveAndStoreConstituentPart();
        }
    }

    void OnDestroy()
    {
        this.InitWorker();
        GalaxySceneManager.instance.workerTaskQueue.RemoveWorker(this.gameObject);
    }


    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return this.inventory.GetStatus();
    }

    public void DoTask(WorkerTask task)
    {
        GalaxySceneManager.instance.workerTaskQueue.SetWorkerAsBusy(this.gameObject);
        this.task = task;
    }

    // IMPLEMENTATION METHODS

    private void InitWorker()
    {
        // init mode
        this.workerMode = ConstWorker.MODE_INIT;
        // release storage reservations
        if (this.task != null)
        {
            if (this.selectedStorageInventory != null && this.hasStorageReservation)
            {
                if (this.task.taskType == ConstWorker.TASK_TYPE_FETCH_AND_PLACE)
                {
                    this.selectedStorageInventory.ReleaseReservation(this.task.structureFeType);
                }
                else if (this.task.taskType == ConstWorker.TASK_TYPE_FETCH_AND_ADD_CONSTITUENT_PART)
                {
                    this.selectedStorageInventory.ReleaseReservation(this.task.constituentPartFeType);
                }
            }
        }
        // init vars
        this.task = null;
        this.selectedStorage = null;
        this.selectedStorageInventory = null;
        // set worker as available if needed
        var wtq = GalaxySceneManager.instance.workerTaskQueue;
        if (!wtq.WorkerIsAvailable(this.gameObject))
        {
            wtq.SetWorkerAsAvailable(this.gameObject);
        }
    }

    // fetch and place structure

    private void FetchAndPlaceFactoryStructure()
    {
        if (this.task.structure == null)
        {
            GalaxySceneManager.instance.workerTaskQueue.CancelWorkerTask(this.task);
            this.InitWorker();
            return;
        }
        // in-progress structure has been removed, task can be cancelled via worker initialization
        if (this.workerMode == ConstWorker.MODE_INIT)
        {
            this.workerMode = ConstWorker.MODE_FETCH_STRUCTURE;
        }
        else if (this.workerMode == ConstWorker.MODE_FETCH_STRUCTURE)
        {
            this.FetchStructureFromStorage();
        }
        else if (this.workerMode == ConstWorker.MODE_PLACE_STRUCTURE)
        {
            this.PlaceFactoryStructure();
        }
    }

    private void FetchStructureFromStorage()
    {
        // select storage and reserve structure
        if (this.selectedStorage == null)
        {
            GameObject storage = this.GetClosestStorage(this.task.structureFeType);
            if (storage != null)
            {
                this.selectedStorage = storage;
                this.selectedStorageInventory = storage.GetComponent<FactoryEntityInventory>();
                this.selectedStorageInventory.Reserve(this.task.structureFeType);
                this.hasStorageReservation = true;
            }
            else
            {
                GalaxySceneManager.instance.workerTaskQueue.ConvertFetchAndPlaceTaskIfPossible(this.task);
                this.InitWorker();
                return;
            }
        }
        // move and place
        else
        {
            // close enough to storage for retrieval of item
            if (Vector3.Distance(this.transform.position, this.selectedStorage.transform.position) < this.interactionDistance)
            {
                // retrieve and bump worker mode
                int retrieved = this.selectedStorageInventory.RetrieveReserved(this.task.structureFeType);
                // Debug.Log("attempting to retrieving structure type: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType) + " from storage");
                // Debug.Log("type retrieved: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(retrieved));
                this.inventory.Store(retrieved);
                this.hasStorageReservation = false;
                this.workerMode = ConstWorker.MODE_PLACE_STRUCTURE;
            }
            // move closer to fetch storage
            else
            {
                this.HandleMoveTowardsPosition(this.selectedStorage.transform.position);
            }
        }
    }

    private void PlaceFactoryStructure()
    {
        // close enough to location to build
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {
            // Debug.Log("placing structure of type: " + this.task.structureFeType.ToString());
            // remove structure from inventory
            this.inventory.Retrieve(this.task.structureFeType);
            // activate the in-progress game object
            this.task.structure.GetComponent<FactoryStructureBehavior>().ActivateStructure();
            // declare task complete
            GalaxySceneManager.instance.workerTaskQueue.TaskComplete(this.task);
            // init worker
            this.InitWorker();
        }
        // move closer to build location
        else
        {
            this.HandleMoveTowardsPosition(this.task.structure.transform.position);
        }
    }

    // fetch and add constituent part

    private void FetchAndAddConstituentPartToFactoryStructure()
    {
        // Debug.Log("worker doing fetch-and-add-constituent-part task...");
        if (this.task.structure == null)
        {
            GalaxySceneManager.instance.workerTaskQueue.CancelWorkerTask(this.task);
            this.InitWorker();
            return;
        }
        // in-progress structure has been removed, task can be cancelled via worker initialization
        if (this.workerMode == ConstWorker.MODE_INIT)
        {
            this.workerMode = ConstWorker.MODE_FETCH_CONSTITUENT_PART;
        }
        else if (this.workerMode == ConstWorker.MODE_FETCH_CONSTITUENT_PART)
        {
            this.FetchConstituentPartFromStorage();
        }
        else if (this.workerMode == ConstWorker.MODE_PLACE_CONSTITUENT_PART)
        {
            this.PlaceConstituentPart();
        }
    }

    private void FetchConstituentPartFromStorage()
    {
        if (this.selectedStorage == null)
        {
            GameObject storage = this.GetClosestStorage(this.task.constituentPartFeType);
            if (storage != null)
            {
                this.selectedStorage = storage;
                this.selectedStorageInventory = storage.GetComponent<FactoryEntityInventory>();
                this.selectedStorageInventory.Reserve(this.task.constituentPartFeType);
                this.hasStorageReservation = true;
            }
            else
            {
                GalaxySceneManager.instance.workerTaskQueue.RequeueWorkerTask(this.task);
                this.InitWorker();
                return;
            }
        }
        else
        {
            // close enough to storage for retrieval of constituent part
            if (Vector3.Distance(this.transform.position, this.selectedStorage.transform.position) < this.interactionDistance)
            {
                // retrieve and bump worker mode
                int retrieved = this.selectedStorageInventory.RetrieveReserved(this.task.constituentPartFeType);
                // Debug.Log("attempting to retrieving constituent part type: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.constituentPartFeType) + " from storage");
                // Debug.Log("type retrieved: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(retrieved));
                this.inventory.Store(retrieved);
                this.hasStorageReservation = false;
                this.workerMode = ConstWorker.MODE_PLACE_CONSTITUENT_PART;
            }
            // move closer to fetch storage
            else
            {
                this.HandleMoveTowardsPosition(this.selectedStorage.transform.position);
            }
        }
    }

    private void PlaceConstituentPart()
    {
        // close enough to location to build
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {

            // Debug.Log("adding constituent part: " +
            //     GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.constituentPartFeType) +
            //     " to structure of type: " +
            //     GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType));

            // attempt to add the constituent part
            bool status = this.task.structure.GetComponent<FactoryStructureBehavior>().AddConstituentPart(this.task.constituentPartFeType);
            if (status)
            {
                // Debug.Log("status of constituent part addition: " + status.ToString());
                // remove structure from inventory
                this.inventory.Retrieve(this.task.constituentPartFeType);
                // declare task complete
                GalaxySceneManager.instance.workerTaskQueue.TaskComplete(this.task);
            }
            else
            {
                // requeue task if not successful
                GalaxySceneManager.instance.workerTaskQueue.RequeueWorkerTask(this.task);
            }
            // init worker
            this.InitWorker();
        }
        // move closer to build location
        else
        {
            this.HandleMoveTowardsPosition(this.task.structure.transform.position);
        }
    }

    // remove structure and store

    private void RemoveAndStoreFactoryStructure()
    {
        if (this.workerMode == ConstWorker.MODE_INIT)
        {
            this.workerMode = ConstWorker.MODE_REMOVE_STRUCTURE;
        }
        else if (this.workerMode == ConstWorker.MODE_REMOVE_STRUCTURE)
        {
            this.RemoveFactoryStructure();
        }
        else if (this.workerMode == ConstWorker.MODE_STORE_STRUCTURE)
        {
            this.DeliverAndStoreFactoryStructure();
        }
    }

    private void RemoveFactoryStructure()
    {
        // close enough to location to remove
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {
            // Debug.Log("removing structure of type: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType));
            // remove the structure
            var removable = this.task.structure.GetComponent<FactoryEntityRemovable>();
            if (removable != null)
            {
                removable.Remove();
            }
            else
            {
                Debug.LogWarning("unable to remove structure of type: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType));
            }
            // add structure to inventory
            this.inventory.Store(this.task.structureFeType);
            // bump mode
            this.workerMode = ConstWorker.MODE_STORE_STRUCTURE;
        }
        // move closer to build location
        else
        {
            this.HandleMoveTowardsPosition(this.task.structure.transform.position);
        }
    }

    private void DeliverAndStoreFactoryStructure()
    {
        if (this.selectedStorage == null)
        {
            GameObject storage = this.GetClosestStorage();
            if (storage != null)
            {
                this.selectedStorage = storage;
                this.selectedStorageInventory = storage.GetComponent<FactoryEntityInventory>();
            }
            else
            {
                GalaxySceneManager.instance.workerTaskQueue.RequeueWorkerTask(this.task);
                this.InitWorker();
            }
        }
        else
        {
            // close enough to deliver to storage
            if (Vector3.Distance(this.transform.position, this.selectedStorage.transform.position) < this.interactionDistance)
            {
                // remove structure from inventory and deposite to storage
                this.selectedStorageInventory.Store(this.inventory.Retrieve(this.task.structureFeType));
                // declare task complete
                GalaxySceneManager.instance.workerTaskQueue.TaskComplete(this.task);
                // init worker
                this.InitWorker();
            }
            // do move closer to delivery storage
            else
            {
                this.HandleMoveTowardsPosition(this.selectedStorage.transform.position);
            }
        }
    }

    // remove and store constituent part

    private void RemoveAndStoreConstituentPart()
    {
        if (this.workerMode == ConstWorker.MODE_INIT)
        {
            this.workerMode = ConstWorker.MODE_REMOVE_CONSTITUENT_PART;
        }
        else if (this.workerMode == ConstWorker.MODE_REMOVE_CONSTITUENT_PART)
        {
            this.RemoveConstituentPart();
        }
        else if (this.workerMode == ConstWorker.MODE_STORE_CONSTITUENT_PART)
        {
            this.DeliverAndStoreConstituentPart();
        }
    }

    private void RemoveConstituentPart()
    {
        // close enough to structure to interact
        if (Vector3.Distance(this.transform.position, this.task.structure.transform.position) < this.interactionDistance)
        {

            Debug.Log(
                "removing constituent part of type: " +
                GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.constituentPartFeType) +
                " from structure type: " +
                GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType)
            );

            // extract constituent part
            var fsb = this.task.structure.GetComponent<FactoryStructureBehavior>();
            if (fsb != null)
            {
                int removedConstituentPart = fsb.RemoveLastConstituentPart();
                if (removedConstituentPart != ConstFEType.NONE)
                {
                    this.inventory.Store(removedConstituentPart);
                    // if final contituent part removed by this worker, remove the structure
                    if (fsb.GetConstituentPartsAdded().Length < 1)
                    {
                        var removable = this.task.structure.GetComponent<FactoryEntityRemovable>();
                        if (removable != null)
                        {
                            removable.Remove();
                        }
                        else
                        {
                            Debug.LogWarning("unable to remove structure of type: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType));
                        }
                    }
                }
                else
                {
                    Debug.LogWarning(
                        "removed constituent part NONE type from structure type: " +
                        GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType)
                    );
                }
            }
            else
            {
                Debug.LogWarning(
                    "structure type has no factory structure behavior script: " +
                    GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(this.task.structureFeType)
                );
            }
            // bump mode
            this.workerMode = ConstWorker.MODE_STORE_CONSTITUENT_PART;
        }
        // move closer to build location
        else
        {
            this.HandleMoveTowardsPosition(this.task.structure.transform.position);
        }
    }

    private void DeliverAndStoreConstituentPart()
    {
        if (this.selectedStorage == null)
        {
            GameObject storage = this.GetClosestStorage();
            if (storage != null)
            {
                this.selectedStorage = storage;
                this.selectedStorageInventory = storage.GetComponent<FactoryEntityInventory>();
            }
            else
            {
                GalaxySceneManager.instance.workerTaskQueue.RequeueWorkerTask(this.task);
                this.InitWorker();
            }
        }
        else
        {
            // close enough to deliver to storage
            if (Vector3.Distance(this.transform.position, this.selectedStorage.transform.position) < this.interactionDistance)
            {
                // remove constituent part from inventory and deposite to storage
                this.selectedStorageInventory.Store(this.inventory.Retrieve(this.task.constituentPartFeType));
                // declare task complete
                GalaxySceneManager.instance.workerTaskQueue.TaskComplete(this.task);
                // init worker
                this.InitWorker();
            }
            // do move closer to delivery storage
            else
            {
                this.HandleMoveTowardsPosition(this.selectedStorage.transform.position);
            }
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

    private GameObject GetClosestStorage(int feTypeToFetch = ConstFEType.NONE)
    {
        // Debug.Log("selecting storage to fetch item: " + GalaxySceneManager.instance.feData.GetDisplayNameFromFEType(feTypeToFetch));
        List<GameObject> storages = GalaxySceneManager.instance.playerFactory.GetFactoryEntityListByType(ConstFEType.STORAGE);
        float shortestDistance = Mathf.Infinity;
        GameObject closestStorage = null;
        foreach (GameObject storage in storages)
        {
            // storage is the closest and contains the needed item (only if item is not NONE)
            float d = Vector3.Distance(this.transform.position, storage.transform.position);
            if (d <= ConstWorker.MAX_WORKER_TO_STORAGE_DISTANCE && d < shortestDistance)
            {
                if (feTypeToFetch == ConstFEType.NONE || storage.GetComponent<FactoryEntityInventory>().IsAvailable(feTypeToFetch))
                {
                    shortestDistance = d;
                    closestStorage = storage;
                }
            }
        }
        return closestStorage;
    }


}
