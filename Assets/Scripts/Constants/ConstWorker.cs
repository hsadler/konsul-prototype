using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstWorker
{


    // WORKERS/TASKS

    public const int TASK_TYPE_FETCH_AND_PLACE = 1;
    public const int TASK_TYPE_REMOVE_AND_STORE = 2;
    public const int TASK_TYPE_FETCH_AND_ADD_CONSTITUENT_PART = 3;

    public const int MODE_INIT = 0;
    public const int MODE_FETCH_STRUCTURE = 1;
    public const int MODE_PLACE_STRUCTURE = 2;
    public const int MODE_REMOVE_STRUCTURE = 3;
    public const int MODE_STORE_STRUCTURE = 4;
    public const int MODE_FETCH_CONSTITUENT_PART = 5;
    public const int MODE_PLACE_CONSTITUENT_PART = 6;

    public const float MAX_WORKER_TO_WORKER_TASK_DISTANCE = 100f;
    public const float MAX_WORKER_TO_STORAGE_DISTANCE = 100f;


}
