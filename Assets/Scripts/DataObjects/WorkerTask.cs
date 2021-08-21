using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTask
{


    public Guid taskId;
    public bool isCancelled;
    public int taskType;
    public GameObject structure;
    public int structureFeType;


    public WorkerTask(int taskType, GameObject structure)
    {
        this.taskId = Guid.NewGuid();
        this.isCancelled = false;
        this.taskType = taskType;
        this.structure = structure;
        this.structureFeType = structure.GetComponent<IFactoryEntity>().FactoryEntityType;
    }


}
