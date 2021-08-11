using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerScript : MonoBehaviour
{


    private WorkerTask currentTask;


    // UNITY HOOKS

    void Start()
    {
    }

    void Update()
    {
    }


    // INTERFACE METHODS

    public void DoTask(WorkerTask task)
    {
        this.currentTask = task;
        if (task.taskType == Constants.WORKER_TASK_TYPE_BUILD)
        {
            this.FetchAndBuildFactoryStructure(task.structureType, task.position);
        }
        else if (task.taskType == Constants.WORKER_TASK_TYPE_REMOVE)
        {
            this.RemoveAndStoreFactoryStructure(task.position);
        }
    }

    // IMPLEMENTATION METHODS

    private void FetchAndBuildFactoryStructure(int structureType, Vector3 position)
    {
        // TODO: implement STUB
        Debug.Log("Worker is carrying out order to fetch structure type: " + structureType.ToString() +
            " and build at position: " + position.ToString());
    }

    private void RemoveAndStoreFactoryStructure(Vector3 position)
    {
        // TODO: implement STUB
        Debug.Log("Worker is carrying out order to remove structure at position: " + position.ToString());
    }


}
