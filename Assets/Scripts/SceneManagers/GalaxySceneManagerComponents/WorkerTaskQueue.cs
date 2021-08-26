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
        if (this.workerIdToAvailableWorker.ContainsKey(workerId))
        {
            Debug.LogWarning("unable to set already available worker by id: " + workerId.ToString());
        }
        else
        {
            this.workerIdToAvailableWorker.Add(worker.GetInstanceID(), worker);
        }
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

    // IMPLEMENTATION METHODS

    private void AssignTasksToWorkers()
    {
        if (this.tasks.Count > 0 && this.workerIdToAvailableWorker.Values.Count > 0)
        {
            // try to match a worker for each task
            var tasksToSetInProgress = new List<WorkerTask>();
            foreach (WorkerTask task in this.tasks)
            {
                if (this.workerIdToAvailableWorker.Values.Count > 0)
                {
                    // match a worker by shortest distance to task
                    float shortestDistance = Mathf.Infinity;
                    GameObject matchedWorker = null;
                    foreach (GameObject worker in this.workerIdToAvailableWorker.Values)
                    {
                        // TODO: BUG: task.structure GO doesn't is null sometimes
                        float distance = Vector3.Distance(worker.transform.position, task.structure.transform.position);
                        if (distance < shortestDistance)
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


}
