using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstWorker
{


    // WORKERS/TASKS

    public const int TASK_TYPE_BUILD = 1;
    public const int TASK_TYPE_REMOVE = 2;

    public const int MODE_INIT = 0;
    public const int MODE_FETCH = 1;
    public const int MODE_BUILD = 2;
    public const int MODE_REMOVE = 3;
    public const int MODE_STORE = 4;

    public const float MAX_WORKER_TO_WORKER_TASK_DISTANCE = 100f;
    public const float MAX_WORKER_TO_STORAGE_DISTANCE = 100f;


}
