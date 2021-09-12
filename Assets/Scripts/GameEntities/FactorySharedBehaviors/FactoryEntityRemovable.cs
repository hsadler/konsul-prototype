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
        GalaxySceneManager.instance.factoryStructureRemovalEvent.AddListener(this.HandlePlayerRemove);
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        // TODO: enable this code when ready
        // attempt to remove associated task from worker-task-queue if structure is not active
        // var wtq = GalaxySceneManager.instance.workerTaskQueue;
        // if (!fs.IsStructureActive)
        // {
        //     WorkerTask task = wtq.FindTaskByFactoryStructure(this.gameObject);
        //     if (task != null)
        //     {
        //         wtq.CancelWorkerTask(task);
        //     }
        // }
    }

    // INTERFACE METHODS

    public void SetMarkForRemoval(bool status)
    {
        this.toRemoveIndicator.SetActive(status);
    }

    // IMPLEMENTATION METHODS

    private void HandlePlayerRemove(GameObject removedGO)
    {
        if (removedGO == this.gameObject)
        {
            // attempt to remove associated task from worker-task-queue if structure is not active
            if (!fs.IsStructureActive)
            {
                WorkerTask task = GalaxySceneManager.instance.workerTaskQueue.FindTaskByFactoryStructure(this.gameObject);
                if (task != null)
                {
                    GalaxySceneManager.instance.workerTaskQueue.CancelWorkerTask(task);
                }
            }
            Object.Destroy(this.gameObject);
        }
    }


}
