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
        { Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT, "entity select" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_IO, "transit create" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT, "transit select" },
        { Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT, "entity multiselect" },
        { Constants.PLAYER_INPUT_MODE_MULTI_STRUCTURE_IO, "batch transit create" },
    };

    public int currentPlacementStructureType;
    private IDictionary<UnityEngine.KeyCode, int> keyCodeToFactoryStructureType = new Dictionary<UnityEngine.KeyCode, int>()
    {
        { KeyCode.Alpha1, Constants.FACTORY_STRUCTURE_ENTITY_TYPE_HARVESTER },
        { KeyCode.Alpha2, Constants.FACTORY_STRUCTURE_ENTITY_TYPE_DISTRIBUTOR },
        { KeyCode.Alpha3, Constants.FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE },
    };

    public GameObject currentEntitySelected;
    public GameObject currentStructureIOSelected;
    public GameObject cursorFactoryStructureGO;

    // multiselect
    public GameObject selectionBox;
    private Vector3 initialMultiselectMousePosition;
    private List<GameObject> currentEntitiesSelected = new List<GameObject>();


    // UNITY HOOKS

    void Awake()
    {
        this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
        this.isAdminMode = false;
        this.InitCurrentPlacementStructureType();
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
            this.HandleEntityPlacementMode();
            this.HandleRemoval();
            this.HandelCancelRemoval();
            this.HandleStructureIOMode();
            this.HandleCycleIOSelection();
            this.HandleModeRevert();
            this.HandleGameQuit();
            if (this.isAdminMode)
            {
                this.HandleAdminPopulateSelectedStorage();
                this.HandleAdminSpawnWorker();
            }
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            this.HandleEntitySelection();
        }
        if (Input.GetMouseButtonUp(0))
        {
            this.HandleEntityPlacementOrSelection();
            this.HandleStructureIOCreation();
        }
        this.HandleEntityPlacementCursor();
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

    private void HandleEntityPlacementMode()
    {
        // any of the number keys containing a factory structure are pressed
        foreach (var numkey in this.keyCodeToFactoryStructureType.Keys)
        {
            if (Input.GetKeyDown(numkey))
            {
                this.DeselectAllFactoryEntities();
                this.DeselectAllStructuresIO();
                this.InitCursorFactoryStructureGO();
                this.inputMode = Constants.PLAYER_INPUT_MODE_PLACEMENT;
                this.currentPlacementStructureType = this.keyCodeToFactoryStructureType[numkey];
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 selectPlacementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
                this.cursorFactoryStructureGO = GalaxySceneManager.instance.playerFactory.CreateCursorFactoryStructure(currentPlacementStructureType);
                this.cursorFactoryStructureGO.transform.position = selectPlacementPosition;
            }
        }
    }

    private void HandleEntityPlacementCursor()
    {
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_PLACEMENT)
        {
            GameObject hoveredFactoryEntity = this.GetHoveredFactoryEntity();
            // not hovering, have a cursor object
            if (hoveredFactoryEntity == null && this.cursorFactoryStructureGO != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 selectPlacementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
                this.cursorFactoryStructureGO.SetActive(true);
                this.cursorFactoryStructureGO.transform.position = selectPlacementPosition;
            }
            else if (this.cursorFactoryStructureGO != null)
            {
                this.cursorFactoryStructureGO.SetActive(false);
            }
        }
    }

    private void HandleEntityPlacementOrSelection()
    {
        // placement mode and left click
        if (Input.GetMouseButtonUp(0) && this.inputMode == Constants.PLAYER_INPUT_MODE_PLACEMENT)
        {
            GameObject clickedFactoryEntity = this.GetHoveredFactoryEntity();
            if (clickedFactoryEntity != null)
            {
                // select instead of create
                this.SelectEntity(clickedFactoryEntity);
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
                if (this.isAdminMode)
                {
                    GalaxySceneManager.instance.playerFactory.CreateFactoryEntity(this.currentPlacementStructureType, placementPosition);
                }
                else
                {
                    // place in-progress structure
                    var go = GalaxySceneManager.instance.playerFactory.CreateInProgressInProgressFactoryStructure(this.currentPlacementStructureType, placementPosition);
                    // queue task for worker to build
                    var task = new WorkerTask(Constants.WORKER_TASK_TYPE_BUILD, go);
                    GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                }
            }
        }
    }

    private void HandleEntitySelection()
    {
        if (
            this.inputMode == Constants.PLAYER_INPUT_MODE_INIT ||
            this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT ||
            this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT ||
            this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT
        )
        {
            // initial mouse button press: activate and initialize the selection-box
            if (Input.GetMouseButtonDown(0))
            {
                this.selectionBox.SetActive(true);
                this.selectionBox.transform.localScale = Vector3.zero;
                this.initialMultiselectMousePosition = Input.mousePosition;
            }
            // mouse button up: selection handling
            else if (Input.GetMouseButtonUp(0))
            {
                this.selectionBox.SetActive(false);
                // selection box handling
                if (Input.mousePosition != this.initialMultiselectMousePosition)
                {
                    // detect what factory entities are within selection box
                    Vector3 mPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 mPos2 = Camera.main.ScreenToWorldPoint(this.initialMultiselectMousePosition);
                    Collider2D[] hits = Physics2D.OverlapAreaAll(mPos1, mPos2);
                    List<GameObject> boxSelectedFactoryEntities = new List<GameObject>();
                    foreach (Collider2D col in hits)
                    {
                        if (col.gameObject.CompareTag("FactoryEntity"))
                        {
                            boxSelectedFactoryEntities.Add(col.gameObject);
                        }
                    }
                    // multi-selection setting
                    if (boxSelectedFactoryEntities.Count > 1)
                    {
                        this.MultiSelectEntities(boxSelectedFactoryEntities);
                    }
                    // single-selection setting
                    else if (boxSelectedFactoryEntities.Count == 1)
                    {
                        this.SelectEntity(boxSelectedFactoryEntities[0]);
                    }
                    // nothing selected and not additive selection, so clear selections
                    else if (!Input.GetKey(Constants.PLAYER_INPUT_ADDITIVE_SELECTION_KEY))
                    {
                        this.DeselectAllFactoryEntities();
                    }
                }
                // single-point click selection handling
                else
                {
                    GameObject clickedFactoryEntity = this.GetHoveredFactoryEntity();
                    if (clickedFactoryEntity != null)
                    {
                        this.SelectEntity(clickedFactoryEntity);
                    }
                    else
                    {
                        this.DeselectAllFactoryEntities();
                    }
                }
            }
            // mouse button held down: update the position and shape of the selection-box
            else if (Input.GetMouseButton(0))
            {
                Vector3 mPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mPos2 = Camera.main.ScreenToWorldPoint(this.initialMultiselectMousePosition);
                float width = Mathf.Abs(mPos1.x - mPos2.x);
                float height = Mathf.Abs(mPos1.y - mPos2.y);
                Vector3 midpoint = (mPos1 - mPos2) / 2;
                this.selectionBox.transform.localScale = new Vector3(width, height, 0);
                Vector3 boxPos = mPos1 - midpoint;
                this.selectionBox.transform.position = new Vector3(boxPos.x, boxPos.y, 0);
            }
        }
    }

    private void HandleCycleIOSelection()
    {
        // entity-select mode or structure-io-select and key press
        if (
            (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT || this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT) &&
            Input.GetKeyDown(Constants.PLAYER_INPUT_CYCLE_IO_SELECT)
        )
        {
            this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT;
            // TODO: BUG: something about object reference not set
            this.currentStructureIOSelected = this.currentEntitySelected.GetComponent<FactoryStructureIOBehavior>().RotateSelection();
        }
    }

    private void HandleRemoval()
    {
        //  removal key press
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_REMOVAL_KEY))
        {
            // entity-select mode
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT)
            {
                var feRemovable = this.currentEntitySelected.GetComponent<FactoryEntityRemovable>();
                if (feRemovable != null)
                {
                    if (this.isAdminMode)
                    {
                        GalaxySceneManager.instance.factoryStructureRemovalEvent.Invoke(this.currentEntitySelected);
                        this.currentEntitySelected = null;
                        this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
                    }
                    else
                    {
                        IFactoryStructure fs = this.currentEntitySelected.GetComponent<IFactoryStructure>();
                        // remove immediately if not an active structure
                        if (fs != null && !fs.IsStructureActive)
                        {
                            GalaxySceneManager.instance.factoryStructureRemovalEvent.Invoke(this.currentEntitySelected);
                        }
                        // mark structure as to-remove and create worker task
                        else
                        {
                            feRemovable.SetMarkForRemoval(true);
                            var task = new WorkerTask(Constants.WORKER_TASK_TYPE_REMOVE, this.currentEntitySelected);
                            GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                        }
                    }
                }
            }
            // structure-io-select mode
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT)
            {
                this.currentEntitySelected.GetComponent<FactoryStructureIOBehavior>().RemoveCurrentSelectedResourceIO();
                this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT;
            }
        }
    }

    private void HandelCancelRemoval()
    {
        // cancel-removal keypress
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_CANCEL_REMOVAL_KEY))
        {
            List<GameObject> fStructuresToCancel = new List<GameObject>();
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT)
            {
                fStructuresToCancel.Add(this.currentEntitySelected);
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT)
            {
                fStructuresToCancel = this.currentEntitiesSelected;
            }
            foreach (GameObject fStructure in fStructuresToCancel)
            {
                WorkerTask task = GalaxySceneManager.instance.workerTaskQueue.FindTaskByFactoryStructure(fStructure);
                if (task != null)
                {
                    FactoryEntityRemovable fRemovable = fStructure.GetComponent<FactoryEntityRemovable>();
                    fRemovable.SetMarkForRemoval(false);
                    GalaxySceneManager.instance.workerTaskQueue.CancelWorkerTask(task);
                }
            }
        }
    }

    private void HandleStructureIOMode()
    {
        // entity-select mode and mode key press
        if (
            (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT || this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT) &&
            Input.GetKeyDown(Constants.PLAYER_INPUT_STRUCTURE_IO_MODE_KEY)
        )
        {
            this.DeselectAllStructuresIO();
            this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_IO;
        }
    }

    private void HandleStructureIOCreation()
    {
        // structure-io mode and left click
        if (Input.GetMouseButtonUp(0) && this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO)
        {
            GameObject clickedFactoryEntity = GetHoveredFactoryEntity();
            if (clickedFactoryEntity != null)
            {
                if (this.currentEntitySelected.GetComponent<FactoryStructureIOBehavior>() != null)
                {
                    GalaxySceneManager.instance.factoryStructureIOPlacementEvent.Invoke(this.currentEntitySelected, clickedFactoryEntity);
                    this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT;
                }
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
                this.InitCurrentPlacementStructureType();
                this.InitCursorFactoryStructureGO();
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT || this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
                this.DeselectAllFactoryEntities();
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT;
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_MULTI_STRUCTURE_IO)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT;
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT;
                this.DeselectAllStructuresIO();
            }
        }
    }

    private void HandleAdminPopulateSelectedStorage()
    {
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT && Input.GetKeyDown(Constants.PLAYER_INPUT_ADMIN_POPULATE_STORAGE))
        {
            var inventory = this.currentEntitySelected.GetComponent<FactoryEntityInventory>();
            if (inventory != null)
            {
                inventory.AdminPopulate();
            }
        }
    }

    private void HandleAdminSpawnWorker()
    {
        if (Input.GetKeyDown(Constants.PLAYER_INPUT_ADMIN_CREATE_WORKER))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
            GalaxySceneManager.instance.playerFactory.CreateFactoryEntity(Constants.FACTORY_UNIT_ENTITY_TYPE_WORKER, placementPosition);
        }
    }

    // factory building controls helpers

    private GameObject GetHoveredFactoryEntity()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
        foreach (Collider2D hit in hits)
        {
            if (hit != null && hit.gameObject.CompareTag("FactoryEntity"))
            {
                // don't return hover of cursor factory structure
                if (hit.gameObject == this.cursorFactoryStructureGO)
                {
                    return null;
                }
                return hit.gameObject;
            }
        }
        return null;
    }

    private void SelectEntity(GameObject factoryEntity)
    {
        // method can handle single selection as well as additive multi-selection 
        this.DeselectAllStructuresIO();
        // additive seleciton
        if (Input.GetKey(Constants.PLAYER_INPUT_ADDITIVE_SELECTION_KEY))
        {
            // additive entity selection following a single selection
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT)
            {
                List<GameObject> multiSelected = new List<GameObject>() {
                    this.currentEntitySelected,
                    factoryEntity
                };
                this.MultiSelectEntities(multiSelected);
                return;
            }
            // additive entity selection following a multiselection
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT)
            {
                this.MultiSelectEntities(new List<GameObject>() { factoryEntity });
                return;
            }
            // additive entity selection following nothing selected
            else
            {
                // fall through
            }
        }
        // single entity selection
        this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT;
        this.DeselectAllFactoryEntities();
        this.currentEntitySelected = factoryEntity;
        GalaxySceneManager.instance.factoryEntitySelectedEvent.Invoke(factoryEntity);
    }

    private void MultiSelectEntities(List<GameObject> factoryEntities)
    {
        this.inputMode = Constants.PLAYER_INPUT_MODE_FACTORY_ENTITY_MULTISELECT;
        this.currentEntitySelected = null;
        this.DeselectAllStructuresIO();
        if (!Input.GetKey(Constants.PLAYER_INPUT_ADDITIVE_SELECTION_KEY))
        {
            this.DeselectAllFactoryEntities();
        }
        foreach (GameObject fEntity in factoryEntities)
        {
            this.currentEntitiesSelected.Add(fEntity);
            GalaxySceneManager.instance.factoryEntitySelectedEvent.Invoke(fEntity);
        }
    }

    private void DeselectAllFactoryEntities()
    {
        this.currentEntitySelected = null;
        this.currentEntitiesSelected = new List<GameObject>();
        GalaxySceneManager.instance.factoryEntityDelesectAllEvent.Invoke();
    }

    private void DeselectAllStructuresIO()
    {
        this.currentStructureIOSelected = null;
        GalaxySceneManager.instance.factoryStructureIODelesectAllEvent.Invoke();
    }

    private void InitCurrentPlacementStructureType()
    {
        // zero means nothing is selected for placement
        this.currentPlacementStructureType = 0;
    }

    private void InitCursorFactoryStructureGO()
    {
        if (this.cursorFactoryStructureGO != null)
        {
            Object.Destroy(this.cursorFactoryStructureGO);
            this.cursorFactoryStructureGO = null;
        }
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
        float zoomMultiplier = 15f;
        if (Input.GetKey(Constants.PLAYER_INPUT_FAST_ZOOM_KEY))
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
