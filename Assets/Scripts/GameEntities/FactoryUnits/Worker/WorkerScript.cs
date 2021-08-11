using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerScript : MonoBehaviour, IFactoryEntity
{


    public int FactoryEntityType { get; } = Constants.FACTORY_UNIT_ENTITY_TYPE_WORKER;

    private WorkerTask currentTask;


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.workerTaskQueue.AddNewWorker(this.gameObject);
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        GalaxySceneManager.instance.workerTaskQueue.RemoveWorker(this.gameObject);
    }


    // INTERFACE METHODS

    public string GetStringFormattedFactoryEntityInfo()
    {
        return "I'm the worker info string";
    }

    public void DoTask(WorkerTask task)
    {
        GalaxySceneManager.instance.workerTaskQueue.SetWorkerAsBusy(this.gameObject);
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
