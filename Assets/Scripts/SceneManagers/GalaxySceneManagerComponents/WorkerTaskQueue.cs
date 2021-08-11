using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTaskQueue : MonoBehaviour
{

    // TODO: implement


    private List<GameObject> workers = new List<GameObject>();
    private Queue<WorkerTask> taskQueue = new Queue<WorkerTask>();


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void AddWorkerTask(WorkerTask task)
    {
        // STUB
        Debug.Log("adding worker task of task-id: " + task.taskId);
    }

    // IMPLEMENTATION METHODS


}
