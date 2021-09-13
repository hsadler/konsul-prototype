using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTaskQueue : MonoBehaviour
{


    private List<GameObject> workers = new List<GameObject>();
    private IDictionary<int, GameObject> workerIdToAvailableWorker = new Dictionary<int, GameObject>();

    private LinkedList<WorkerTask> tasks = new LinkedList<WorkerTask>();
    private LinkedList<WorkerTask> tasksInProgress = new LinkedList<WorkerTask>();


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {
        this.AssignTasksToWorkers();
    }

    // INTERFACE METHODS

    // worker management
    public void AddNewWorker(GameObject worker)
    {
        this.workers.Add(worker);
        this.SetWorkerAsAvailable(worker);
    }
    public void RemoveWorker(GameObject worker)
    {
        int workerId = worker.GetInstanceID();
        if (this.workerIdToAvailableWorker.ContainsKey(workerId))
        {
            this.workerIdToAvailableWorker.Remove(worker.GetInstanceID());
        }
        for (int i = 0; i < this.workers.Count; i++)
        {
            GameObject w = this.workers[i];
            if (w.GetInstanceID() == workerId)
            {
                this.workers.RemoveAt(i);
            }
        }
    }
    public void SetWorkerAsBusy(GameObject worker)
    {
        int workerId = worker.GetInstanceID();
        if (this.workerIdToAvailableWorker.ContainsKey(workerId))
        {
            this.workerIdToAvailableWorker.Remove(worker.GetInstanceID());
        }
        else
        {
            Debug.LogWarning("unable to set worker as busy since already unavailable by id: " + workerId.ToString());
        }
    }
    public void SetWorkerAsAvailable(GameObject worker)
    {
        int workerId = worker.GetInstanceID();
        if (this.WorkerIsAvailable(worker))
        {
            Debug.LogWarning("unable to set already available worker by id: " + workerId.ToString());
        }
        else
        {
            this.workerIdToAvailableWorker.Add(worker.GetInstanceID(), worker);
        }
    }

    public bool WorkerIsAvailable(GameObject worker)
    {
        return this.workerIdToAvailableWorker.ContainsKey(worker.GetInstanceID());
    }

    // worker task management
    public void AddWorkerTask(WorkerTask task)
    {
        // Debug.Log("adding worker task to queue with id: " + task.taskId.ToString());
        this.tasks.AddLast(task);
    }
    public void AddPriorityWorkerTask(WorkerTask task)
    {
        this.tasks.AddFirst(task);
    }
    public void RequeueWorkerTask(WorkerTask task)
    {
        // Debug.Log("requeueing worker task by id: " + task.taskId.ToString());
        this.tasks.Remove(task);
        this.tasksInProgress.Remove(task);
        this.AddWorkerTask(task);
    }
    public void CancelWorkerTask(WorkerTask task)
    {
        this.tasks.Remove(task);
        this.tasksInProgress.Remove(task);
        task.isCancelled = true;
    }
    public void TaskComplete(WorkerTask task)
    {
        this.tasksInProgress.Remove(task);
    }
    public WorkerTask FindTaskByFactoryStructure(GameObject factoryStructure)
    {
        // check task queue
        foreach (WorkerTask task in this.tasks)
        {
            if (task.structure == factoryStructure)
            {
                return task;
            }
        }
        // check in-progress
        foreach (WorkerTask task in this.tasksInProgress)
        {
            if (task.structure == factoryStructure)
            {
                return task;
            }
        }
        return null;
    }
    public void ConvertFetchAndPlaceTaskIfPossible(WorkerTask task)
    {
        // Debug.Log("attempting to convert structure placement task to constituent placement task by task id: " + task.taskId.ToString());
        // should be a fetch and place task
        if (task.taskType != ConstWorker.TASK_TYPE_FETCH_AND_PLACE)
        {
            // Debug.LogWarning("ConvertFetchAndPlaceTaskIfPossible cannot operate on non-fetchAndPlace task type. Requeueing task");
            this.RequeueWorkerTask(task);
            return;
        }
        // check if any storages contain any of the constituent parts
        FactoryEntityTemplate structureTemplate = GalaxySceneManager.instance.feData.GetFETemplate(task.structureFeType);
        List<GameObject> storages = GalaxySceneManager.instance.playerFactory.GetFactoryEntityListByType(ConstFEType.STORAGE);
        foreach (GameObject storage in storages)
        {
            var storageInventory = storage.GetComponent<FactoryEntityInventory>();
            foreach (KeyValuePair<int, int> entry in structureTemplate.assembledFrom)
            {
                if (storageInventory.IsAvailable(entry.Key))
                {
                    this.CreateContituentPartsTasksForStructure(task.structure, structureTemplate.assembledFrom);
                    this.CancelWorkerTask(task);
                    return;
                }
            }

        }
        // if execution reaches this point, that means there's no constituent parts, so simply requeue original task
        this.RequeueWorkerTask(task);
    }

    // IMPLEMENTATION METHODS

    private void AssignTasksToWorkers()
    {
        if (this.tasks.Count > 0 && this.workerIdToAvailableWorker.Values.Count > 0)
        {
            // try to match a worker for each task
            var tasksToSetInProgress = new List<WorkerTask>();
            foreach (WorkerTask task in this.tasks)
            {
                // NOTE: needed task.structure null check here for a bugfix
                if (task.structure != null && this.workerIdToAvailableWorker.Values.Count > 0)
                {
                    // match a worker by shortest distance to task
                    float shortestDistance = Mathf.Infinity;
                    GameObject matchedWorker = null;
                    foreach (GameObject worker in this.workerIdToAvailableWorker.Values)
                    {
                        float distance = Vector3.Distance(worker.transform.position, task.structure.transform.position);
                        if (distance <= ConstWorker.MAX_WORKER_TO_WORKER_TASK_DISTANCE && distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            matchedWorker = worker;
                        }
                    }
                    // assign task to worker and remove from the queue
                    if (matchedWorker != null)
                    {
                        // Debug.Log("matching worker id: " + matchedWorker.GetInstanceID().ToString() + " to task id: " + task.taskId.ToString());
                        matchedWorker.GetComponent<WorkerScript>().DoTask(task);
                        tasksToSetInProgress.Add(task);
                    }
                }
                // no more available workers
                else
                {
                    break;
                }
            }
            foreach (WorkerTask task in tasksToSetInProgress)
            {
                // Debug.Log("setting worker task as in-progress with id: " + task.taskId.ToString());
                this.tasks.Remove(task);
                this.tasksInProgress.AddLast(task);
            }
        }
    }

    private void CreateContituentPartsTasksForStructure(GameObject structure, IDictionary<int, int> constituents)
    {
        foreach (KeyValuePair<int, int> entry in constituents)
        {
            int feType = entry.Key;
            int feAmount = entry.Value;
            for (int i = 0; i < feAmount; i++)
            {
                var task = new WorkerTask(ConstWorker.TASK_TYPE_FETCH_AND_ADD_CONSTITUENT_PART, structure, feType);
                this.AddWorkerTask(task);
                // Debug.Log("constituent task created for taskId: " + task.taskId.ToString() + " and constituent part: " +
                //     GalaxySceneManager.instance.feData.GetFETemplate(feType).displayName);
            }
        }
    }


}
