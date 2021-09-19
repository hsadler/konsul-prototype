using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityRemovable : MonoBehaviour
{


    public GameObject toRemoveIndicator;

    private IFactoryStructure fs;


    // UNITY HOOKS

    void Awake()
    {
        this.fs = this.GetComponent<IFactoryStructure>();
    }

    void Start()
    {
        this.toRemoveIndicator.SetActive(false);
    }

    void Update()
    {
    }

    // INTERFACE METHODS

    public void SetMarkForRemoval(bool status)
    {
        this.toRemoveIndicator.SetActive(status);
    }

    public void Remove(bool cancelAssociatedTasks = false)
    {
        // attempt to remove associated task from worker-task-queue
        if (cancelAssociatedTasks)
        {
            var wtq = GalaxySceneManager.instance.workerTaskQueue;
            foreach (WorkerTask task in wtq.FindTasksByFactoryStructure(this.gameObject))
            {
                wtq.CancelWorkerTask(task);
            }
        }
        Object.Destroy(this.gameObject);
    }

    // IMPLEMENTATION METHODS


}
