using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTask
{


    public Guid taskId;
    public int taskType;
    public Vector3 position;
    public int structureType;


    public WorkerTask(int taskType, Vector3 position, int structureType = Constants.ENTITY_TYPE_NONE)
    {
        this.taskId = Guid.NewGuid();
        this.taskType = taskType;
        this.position = position;
        this.structureType = structureType;
    }


}
