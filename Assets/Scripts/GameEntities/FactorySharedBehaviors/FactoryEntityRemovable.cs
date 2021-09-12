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

    public void Remove()
    {
        // attempt to remove associated task from worker-task-queue if structure is not active
        var wtq = GalaxySceneManager.instance.workerTaskQueue;
        if (!fs.IsStructureActive)
        {
            WorkerTask task = wtq.FindTaskByFactoryStructure(this.gameObject);
            if (task != null)
            {
                wtq.CancelWorkerTask(task);
            }
        }
        Object.Destroy(this.gameObject);
    }

    // IMPLEMENTATION METHODS


}
