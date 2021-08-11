using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTaskQueue : MonoBehaviour
{


    private List<GameObject> workers = new List<GameObject>();
    private IDictionary<int, GameObject> workerIdToAvailableWorker = new Dictionary<int, GameObject>();

    private LinkedList<WorkerTask> tasks = new LinkedList<WorkerTask>();


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
        this.tasks.AddLast(task);
    }
    public void AddPriorityWorkerTask(WorkerTask task)
    {
        this.tasks.AddFirst(task);
    }
    public void CancelWorkerTask(WorkerTask task)
    {
        this.tasks.Remove(task);
    }

    // IMPLEMENTATION METHODS

    private void AssignTasksToWorkers()
    {
        if (this.tasks.Count > 0 && this.workerIdToAvailableWorker.Values.Count > 0)
        {
            // try to match a worker for each task
            foreach (WorkerTask task in this.tasks)
            {
                if (this.workerIdToAvailableWorker.Values.Count > 0)
                {
                    // match a worker by shortest distance to task
                    float shortestDistance = Mathf.Infinity;
                    GameObject matchedWorker = null;
                    foreach (GameObject worker in this.workerIdToAvailableWorker.Values)
                    {
                        float distance = Vector3.Distance(worker.transform.position, task.position);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            matchedWorker = worker;
                        }
                    }
                    // assign task to worker and remove from the queue
                    if (matchedWorker != null)
                    {
                        matchedWorker.GetComponent<WorkerScript>().DoTask(task);
                        this.tasks.Remove(task);
                    }
                }
                // no more available workers
                else
                {
                    return;
                }
            }
        }
    }


}
