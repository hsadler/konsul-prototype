using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    private float cameraSize;

    public int inputMode;
    public bool isAdminMode;
    public IDictionary<int, string> inputModeToDisplayString = new Dictionary<int, string>()
    {
        { Constants.PLAYER_INPUT_MODE_INIT, "init" },
        { Constants.PLAYER_INPUT_MODE_PLACEMENT, "structure placement" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT, "structure select" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_IO, "transit create" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT, "transit select" },
    };

    public int currentPlacementStructureType;
    private IDictionary<UnityEngine.KeyCode, int> keyCodeToFactoryStructureType = new Dictionary<UnityEngine.KeyCode, int>()
    {
        { KeyCode.Alpha1, Constants.FACTORY_STRUCTURE_TYPE_HARVESTER },
        { KeyCode.Alpha2, Constants.FACTORY_STRUCTURE_TYPE_DISTRIBUTOR },
        { KeyCode.Alpha3, Constants.FACTORY_STRUCTURE_TYPE_STORAGE },
    };

    public GameObject currentStructureSelected;
    public GameObject currentStructureIOSelected;


    // UNITY HOOKS

    void Awake()
    {
        this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
        this.isAdminMode = false;
        this.InitCurrentPlacementStrutureType();
    }

    void Start()
    {
        this.cameraSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        // factory building
        if (Input.anyKeyDown)
        {
            this.HandleAdminModeToggle();
            this.HandlePlacementMode();
            this.HandleRemoval();
            this.HandleStructureIOMode();
            this.HandleCycleIOSelection();
            this.HandleModeRevert();
            this.HandleGameQuit();
            if (this.isAdminMode)
            {
                this.HandleAdminPopulateSelectedStorage();
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            this.HandleStructureSelection();
            this.HandlePlacementOrSelection();
            this.HandleStructureIO();
        }
        // camera
        this.HandleCameraMovement();
        this.HandleCameraZoom();
    }

    // IMPLEMENTATION METHODS

    // admin mode
    private void HandleAdminModeToggle()
    {
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_ADMIN_MODE_TOGGLE))
        {
            this.isAdminMode = !this.isAdminMode;
        }
    }

    // factory building controls

    private void HandlePlacementMode()
    {
        // any of the number keys containing a factory structure are pressed
        foreach (var numkey in this.keyCodeToFactoryStructureType.Keys)
        {
            if (Input.GetKeyDown(numkey))
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_PLACEMENT;
                this.currentPlacementStructureType = this.keyCodeToFactoryStructureType[numkey];
                this.DeselectAllStructures();
                this.DeselectAllStructuresIO();
            }
        }
    }

    private void HandlePlacementOrSelection()
    {
        // placement mode and left click
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_PLACEMENT)
        {
            GameObject clickedFactoryStructure = this.GetClickedFactoryStructure();
            if (clickedFactoryStructure != null)
            {
                // select instead of create
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
                GalaxySceneManager.instance.factoryStructureSelectedEvent.Invoke(clickedFactoryStructure);
                this.currentStructureSelected = clickedFactoryStructure;
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
                if (this.isAdminMode)
                {
                    GalaxySceneManager.instance.playerFactory.PlaceFactoryStructure(this.currentPlacementStructureType, placementPosition);
                }
                else
                {
                    var task = new WorkerTask(Constants.WORKER_TASK_TYPE_BUILD, placementPosition, this.currentPlacementStructureType);
                    GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                }
            }
        }
    }

    private void HandleStructureSelection()
    {
        // init mode or structure-select or structure-io-select mode and left click
        if (
            this.inputMode == Constants.PLAYER_INPUT_MODE_INIT ||
            this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT ||
            this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT
        )
        {
            this.DeselectAllStructuresIO();
            GameObject clickedFactoryStructure = this.GetClickedFactoryStructure();
            if (clickedFactoryStructure != null)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
                GalaxySceneManager.instance.factoryStructureSelectedEvent.Invoke(clickedFactoryStructure);
                this.currentStructureSelected = clickedFactoryStructure;
            }
        }
    }

    private void HandleCycleIOSelection()
    {
        // structure-select mode or structure-io-select and key press
        if (
            (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT || this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT) &&
            Input.GetKeyDown(Constants.PLAYER_INPUT_CYCLE_IO_SELECT)
        )
        {
            this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT;
            this.currentStructureIOSelected = this.currentStructureSelected.GetComponent<FactoryStructureIOBehavior>().RotateSelection();
        }
    }

    private void HandleRemoval()
    {
        //  removal key press
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_REMOVAL_KEY))
        {
            // structure-select mode
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT)
            {
                if (this.isAdminMode)
                {
                    GalaxySceneManager.instance.factoryStructureRemovalEvent.Invoke(this.currentStructureSelected);
                }
                else
                {
                    var task = new WorkerTask(Constants.WORKER_TASK_TYPE_REMOVE, this.currentStructureSelected.transform.position);
                    GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                }
            }
            // structure-io-select mode
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT)
            {
                this.currentStructureSelected.GetComponent<FactoryStructureIOBehavior>().RemoveCurrentSelectedResourceIO();
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
            }
        }
    }

    private void HandleStructureIOMode()
    {
        // structure-select mode and mode key press
        if (
            (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT || this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT) &&
            Input.GetKeyDown(Constants.PLAYER_INPUT_STRUCTURE_IO_MODE_KEY)
        )
        {
            this.DeselectAllStructuresIO();
            this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_IO;
        }
    }

    private void HandleStructureIO()
    {
        // structure-io mode and left click
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO)
        {
            GameObject clickedFactoryStructure = GetClickedFactoryStructure();
            if (clickedFactoryStructure != null)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
                GalaxySceneManager.instance.factoryStructureIOPlacementEvent.Invoke(this.currentStructureSelected, clickedFactoryStructure);
            }
        }
    }

    private void HandleModeRevert()
    {
        // revert key press
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_REVERT_MODE_KEY))
        {
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_PLACEMENT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
                this.InitCurrentPlacementStrutureType();
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
                this.DeselectAllStructures();
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
                this.DeselectAllStructuresIO();
            }
        }
    }

    private void HandleAdminPopulateSelectedStorage()
    {
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT && Input.GetKeyDown(Constants.PLAYER_INPUT_ADMIN_POPULATE_STORAGE))
        {
            var storageScript = this.currentStructureSelected.GetComponent<IFactoryStorage>();
            if (storageScript != null)
            {
                storageScript.AdminPopulateStorage();
            }
        }
    }

    // factory building controls helpers

    private GameObject GetClickedFactoryStructure()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        if (hit != null && hit.gameObject.CompareTag("FactoryStructure"))
        {
            return hit.gameObject;
        }
        else
        {
            return null;
        }
    }

    private void DeselectAllStructures()
    {
        this.currentStructureSelected = null;
        GalaxySceneManager.instance.factoryStructureDelesectAllEvent.Invoke();
    }

    private void DeselectAllStructuresIO()
    {
        this.currentStructureIOSelected = null;
        GalaxySceneManager.instance.factoryStructureIODelesectAllEvent.Invoke();
    }

    private void InitCurrentPlacementStrutureType()
    {
        // zero means nothing is selected for placement
        this.currentPlacementStructureType = 0;
    }

    // camera controls

    private void HandleCameraMovement()
    {
        // right click held
        if (Input.GetMouseButton(1))
        {
            // scale camera move amount with size of camera view
            float vert = Input.GetAxis("Mouse Y") * Time.deltaTime * Camera.main.orthographicSize * Constants.CAMERA_MOVE_SPEED;
            float horiz = Input.GetAxis("Mouse X") * Time.deltaTime * Camera.main.orthographicSize * Constants.CAMERA_MOVE_SPEED;
            Camera.main.transform.Translate(new Vector3(-horiz, -vert, 0));
        }
    }

    private void HandleCameraZoom()
    {
        float zoomMultiplier = 25f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            zoomMultiplier = 150f;
        }
        float currCameraSize = Camera.main.orthographicSize;
        if (Input.mouseScrollDelta.y != 0)
        {
            this.cameraSize = currCameraSize - (Input.mouseScrollDelta.y * zoomMultiplier);
            // clamp
            if (this.cameraSize < Constants.CAMERA_SIZE_MIN)
            {
                this.cameraSize = Constants.CAMERA_SIZE_MIN;
            }
            else if (this.cameraSize > Constants.CAMERA_SIZE_MAX)
            {
                this.cameraSize = Constants.CAMERA_SIZE_MAX;
            }
        }
        Camera.main.orthographicSize = Mathf.Lerp(currCameraSize, this.cameraSize, Time.deltaTime * Constants.CAMERA_ZOOM_SPEED);
    }

    private void HandleGameQuit()
    {
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_QUIT_GAME_KEY))
        {
            Application.Quit();
        }
    }


}
