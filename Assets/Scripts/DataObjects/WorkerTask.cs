using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTask
{


    public int structureType;
    public Vector3 placementPosition;


    public WorkerTask(int structureType, Vector3 placementPosition)
    {
        this.structureType = structureType;
        this.placementPosition = placementPosition;
    }


}
