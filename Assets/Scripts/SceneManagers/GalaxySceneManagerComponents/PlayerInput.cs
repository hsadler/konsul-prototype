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
        { ConstPlayerInput.MODE_INIT, "init" },
        { ConstPlayerInput.MODE_PLACEMENT, "structure placement" },
        { ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT, "entity select" },
        { ConstPlayerInput.MODE_STRUCTURE_IO, "transit create" },
        { ConstPlayerInput.MODE_STRUCTURE_IO_SELECT, "transit select" },
        { ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT, "entity multiselect" },
        { ConstPlayerInput.MODE_MULTI_STRUCTURE_IO, "batch transit create" },
    };

    public int currentPlacementStructureType;
    private IDictionary<UnityEngine.KeyCode, int> keyCodeToFactoryStructureType = new Dictionary<UnityEngine.KeyCode, int>()
    {
        { KeyCode.Alpha1, ConstFEType.HARVESTER },
        { KeyCode.Alpha2, ConstFEType.DISTRIBUTOR },
        { KeyCode.Alpha3, ConstFEType.STORAGE },
        { KeyCode.Alpha4, ConstFEType.RESOURCE_PROCESSOR },
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
        this.inputMode = ConstPlayerInput.MODE_INIT;
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
        if (Input.GetKeyDown(ConstPlayerInput.ADMIN_MODE_TOGGLE_KEY))
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
                this.inputMode = ConstPlayerInput.MODE_PLACEMENT;
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
        if (this.inputMode == ConstPlayerInput.MODE_PLACEMENT)
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
        if (Input.GetMouseButtonUp(0) && this.inputMode == ConstPlayerInput.MODE_PLACEMENT)
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
                    var task = new WorkerTask(ConstWorker.TASK_TYPE_FETCH_AND_PLACE, go);
                    GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                }
            }
        }
    }

    private void HandleEntitySelection()
    {
        if (
            this.inputMode == ConstPlayerInput.MODE_INIT ||
            this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT ||
            this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT ||
            this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO_SELECT
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
                    else if (!Input.GetKey(ConstPlayerInput.ADDITIVE_SELECTION_KEY))
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
            (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT || this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO_SELECT) &&
            Input.GetKeyDown(ConstPlayerInput.CYCLE_IO_SELECT_KEY) &&
            this.currentEntitySelected != null
        )
        {
            this.inputMode = ConstPlayerInput.MODE_STRUCTURE_IO_SELECT;
            this.currentStructureIOSelected = this.currentEntitySelected.GetComponent<FactoryStructureIOBehavior>().RotateSelection();
        }
    }

    private void HandleRemoval()
    {
        //  removal key press
        if (Input.GetKeyDown(ConstPlayerInput.REMOVAL_KEY))
        {
            // collect structures to remove
            List<GameObject> fStructuresToRemove = new List<GameObject>();
            if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT)
            {
                fStructuresToRemove.Add(this.currentEntitySelected);
            }
            else if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT)
            {
                fStructuresToRemove = this.currentEntitiesSelected;
            }
            foreach (GameObject fStructure in fStructuresToRemove)
            {
                if (fStructure != null)
                {
                    var feRemovable = fStructure.GetComponent<FactoryEntityRemovable>();
                    if (feRemovable != null)
                    {
                        // remove immediately if admin
                        if (this.isAdminMode)
                        {
                            feRemovable.Remove(cancelAssociatedTasks: true);
                            this.currentEntitySelected = null;
                            this.inputMode = ConstPlayerInput.MODE_INIT;
                        }
                        else
                        {
                            var fs = fStructure.GetComponent<IFactoryStructure>();
                            var fsb = fStructure.GetComponent<FactoryStructureBehavior>();
                            // turn on removal indicator
                            feRemovable.SetMarkForRemoval(true);
                            // remove constituent parts already added to inactive structure if needed
                            if (fs != null && !fs.IsStructureActive)
                            {
                                // cancel all tasks associated with structure
                                foreach (WorkerTask task in GalaxySceneManager.instance.workerTaskQueue.FindTasksByFactoryStructure(fStructure))
                                {
                                    GalaxySceneManager.instance.workerTaskQueue.CancelWorkerTask(task);
                                }
                                int[] partsAdded = fsb.GetConstituentPartsAdded();
                                // create constituent part removal tasks for all constituent parts added 
                                if (partsAdded.Length > 0)
                                {
                                    foreach (int feType in partsAdded)
                                    {
                                        WorkerTask task = new WorkerTask(
                                            ConstWorker.TASK_TYPE_REMOVE_CONSTITUENT_PART_AND_STORE,
                                            fStructure,
                                            constituentPartFeType: feType
                                        );
                                        GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                                    }
                                }
                                // remove immediately if no structure parts have been added yet
                                else
                                {
                                    feRemovable.Remove(cancelAssociatedTasks: true);
                                }
                            }
                            // create single worker task to remove active structure
                            else
                            {
                                var task = new WorkerTask(ConstWorker.TASK_TYPE_REMOVE_AND_STORE, fStructure);
                                GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                            }
                        }
                    }
                }
            }
            // structure-io-select mode
            if (this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO_SELECT)
            {
                this.currentEntitySelected.GetComponent<FactoryStructureIOBehavior>().RemoveCurrentSelectedResourceIO();
                this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT;
            }
        }
    }

    private void HandelCancelRemoval()
    {
        // cancel-removal keypress
        if (Input.GetKeyDown(ConstPlayerInput.CANCEL_REMOVAL_KEY))
        {
            // collect structures to cancel removal
            List<GameObject> fStructuresToCancel = new List<GameObject>();
            if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT)
            {
                fStructuresToCancel.Add(this.currentEntitySelected);
            }
            else if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT)
            {
                fStructuresToCancel = this.currentEntitiesSelected;
            }
            foreach (GameObject fStructure in fStructuresToCancel)
            {
                if (fStructure != null)
                {
                    // remove removal indicator
                    var fRemovable = fStructure.GetComponent<FactoryEntityRemovable>();
                    if (fRemovable != null)
                    {
                        fRemovable.SetMarkForRemoval(false);
                    }
                    // cancel all tasks associated with structure
                    foreach (WorkerTask task in GalaxySceneManager.instance.workerTaskQueue.FindTasksByFactoryStructure(fStructure))
                    {
                        GalaxySceneManager.instance.workerTaskQueue.CancelWorkerTask(task);
                    }
                    // create build tasks for non-active structure
                    var fs = fStructure.GetComponent<IFactoryStructure>();
                    if (fs != null && !fs.IsStructureActive)
                    {
                        var fsb = fStructure.GetComponent<FactoryStructureBehavior>();
                        // create constituent-part-addition tasks for each remaining part needed
                        if (fsb.GetConstituentPartsAdded().Length > 0)
                        {
                            foreach (int feType in fsb.GetRemainingContituentPartsNeeded())
                            {
                                var task = new WorkerTask(
                                    ConstWorker.TASK_TYPE_FETCH_AND_ADD_CONSTITUENT_PART,
                                    fStructure,
                                    constituentPartFeType: feType
                                );
                                GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                            }
                        }
                        // no parts added yet, create single fetch-and-place-task
                        else
                        {
                            var task = new WorkerTask(ConstWorker.TASK_TYPE_FETCH_AND_PLACE, fStructure);
                            GalaxySceneManager.instance.workerTaskQueue.AddWorkerTask(task);
                        }
                    }
                }
            }
        }
    }

    private void HandleStructureIOMode()
    {
        // io key press
        if (Input.GetKeyDown(ConstPlayerInput.STRUCTURE_IO_MODE_KEY))
        {
            // entity-select mode or io-select mode
            if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT || this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO_SELECT)
            {
                this.DeselectAllStructuresIO();
                this.inputMode = ConstPlayerInput.MODE_STRUCTURE_IO;
            }
            // entity-multiselect mode
            else if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT)
            {
                this.inputMode = ConstPlayerInput.MODE_MULTI_STRUCTURE_IO;
            }
        }
    }

    private void HandleStructureIOCreation()
    {
        // structure-io mode or multi-structure-io mode and left click
        if (Input.GetMouseButtonUp(0))
        {
            bool iosCreated = false;
            // collect structures on which to create IOs
            List<GameObject> fStructuresToCreateIOs = new List<GameObject>();
            if (this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO)
            {
                fStructuresToCreateIOs.Add(this.currentEntitySelected);
            }
            else if (this.inputMode == ConstPlayerInput.MODE_MULTI_STRUCTURE_IO)
            {
                fStructuresToCreateIOs = this.currentEntitiesSelected;
            }
            // create IOs
            GameObject clickedFactoryEntity = GetHoveredFactoryEntity();
            if (clickedFactoryEntity != null)
            {
                foreach (GameObject fStructure in fStructuresToCreateIOs)
                {
                    if (fStructure != null && fStructure.GetComponent<FactoryStructureIOBehavior>() != null)
                    {
                        GalaxySceneManager.instance.factoryStructureIOPlacementEvent.Invoke(fStructure, clickedFactoryEntity);
                        iosCreated = true;
                    }
                }
            }
            // revert to correct mode if any IOs were created 
            if (iosCreated)
            {
                if (this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO)
                {
                    this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT;
                }
                else if (this.inputMode == ConstPlayerInput.MODE_MULTI_STRUCTURE_IO)
                {
                    this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT;
                }
            }
        }
    }

    private void HandleModeRevert()
    {
        // revert key press
        if (Input.GetKeyDown(ConstPlayerInput.REVERT_MODE_KEY))
        {
            if (this.inputMode == ConstPlayerInput.MODE_PLACEMENT)
            {
                this.inputMode = ConstPlayerInput.MODE_INIT;
                this.InitCurrentPlacementStructureType();
                this.InitCursorFactoryStructureGO();
            }
            else if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT || this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT)
            {
                this.inputMode = ConstPlayerInput.MODE_INIT;
                this.DeselectAllFactoryEntities();
            }
            else if (this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO)
            {
                this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT;
            }
            else if (this.inputMode == ConstPlayerInput.MODE_MULTI_STRUCTURE_IO)
            {
                this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT;
            }
            else if (this.inputMode == ConstPlayerInput.MODE_STRUCTURE_IO_SELECT)
            {
                this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT;
                this.DeselectAllStructuresIO();
            }
        }
    }

    private void HandleAdminPopulateSelectedStorage()
    {
        if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT)
        {
            if (this.currentEntitySelected != null)
            {
                var inventory = this.currentEntitySelected.GetComponent<FactoryEntityInventory>();
                if (inventory != null)
                {
                    if (Input.GetKeyDown(ConstPlayerInput.ADMIN_POPULATE_STORAGE_ALL_KEY))
                    {
                        inventory.AdminPopulate(amount: 1000);
                    }
                    else if (Input.GetKeyDown(ConstPlayerInput.ADMIN_POPULATE_STORAGE_RESOURCES_KEY))
                    {
                        inventory.AdminPopulate(amount: 1000, filterFeGroup: ConstFEGroup.RESOURCE);
                    }
                }
            }
        }
    }

    private void HandleAdminSpawnWorker()
    {
        if (Input.GetKeyDown(ConstPlayerInput.ADMIN_CREATE_WORKER_KEY))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
            GalaxySceneManager.instance.playerFactory.CreateFactoryEntity(ConstFEType.WORKER, placementPosition);
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
                if (hit.gameObject != this.cursorFactoryStructureGO)
                {
                    return hit.gameObject;
                }
            }
        }
        return null;
    }

    private void SelectEntity(GameObject factoryEntity)
    {
        // method can handle single selection as well as additive multi-selection 
        this.DeselectAllStructuresIO();
        // additive seleciton
        if (Input.GetKey(ConstPlayerInput.ADDITIVE_SELECTION_KEY))
        {
            // additive entity selection following a single selection
            if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT)
            {
                List<GameObject> multiSelected = new List<GameObject>() {
                    this.currentEntitySelected,
                    factoryEntity
                };
                this.MultiSelectEntities(multiSelected);
                return;
            }
            // additive entity selection following a multiselection
            else if (this.inputMode == ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT)
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
        this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_SELECT;
        this.DeselectAllFactoryEntities();
        this.currentEntitySelected = factoryEntity;
        var selectable = factoryEntity.GetComponent<FactoryEntitySelectable>();
        if (selectable != null)
        {
            selectable.Select();
        }
    }

    private void MultiSelectEntities(List<GameObject> factoryEntities)
    {
        this.inputMode = ConstPlayerInput.MODE_FACTORY_ENTITY_MULTISELECT;
        this.currentEntitySelected = null;
        this.DeselectAllStructuresIO();
        if (!Input.GetKey(ConstPlayerInput.ADDITIVE_SELECTION_KEY))
        {
            this.DeselectAllFactoryEntities();
        }
        foreach (GameObject fEntity in factoryEntities)
        {
            this.currentEntitiesSelected.Add(fEntity);
            var selectable = fEntity.GetComponent<FactoryEntitySelectable>();
            if (selectable != null)
            {
                selectable.Select();
            }
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
            float vert = Input.GetAxis("Mouse Y") * Time.deltaTime * Camera.main.orthographicSize * GameSettings.CAMERA_MOVE_SPEED;
            float horiz = Input.GetAxis("Mouse X") * Time.deltaTime * Camera.main.orthographicSize * GameSettings.CAMERA_MOVE_SPEED;
            Camera.main.transform.Translate(new Vector3(-horiz, -vert, 0));
        }
    }

    private void HandleCameraZoom()
    {
        float zoomMultiplier = GameSettings.CAMERA_ZOOM_AMOUNT_NORMAL;
        if (Input.GetKey(ConstPlayerInput.SMALL_ZOOM_KEY))
        {
            zoomMultiplier = GameSettings.CAMERA_ZOOM_AMOUNT_SMALL;
        }
        else if (Input.GetKey(ConstPlayerInput.LARGE_ZOOM_KEY))
        {
            zoomMultiplier = GameSettings.CAMERA_ZOOM_AMOUNT_LARGE;
        }
        float currCameraSize = Camera.main.orthographicSize;
        if (Input.mouseScrollDelta.y != 0)
        {
            this.cameraSize = currCameraSize - (Input.mouseScrollDelta.y * zoomMultiplier);
            // clamp
            if (this.cameraSize < GameSettings.CAMERA_SIZE_MIN)
            {
                this.cameraSize = GameSettings.CAMERA_SIZE_MIN;
            }
            else if (this.cameraSize > GameSettings.CAMERA_SIZE_MAX)
            {
                this.cameraSize = GameSettings.CAMERA_SIZE_MAX;
            }
        }
        Camera.main.orthographicSize = Mathf.Lerp(currCameraSize, this.cameraSize, Time.deltaTime * GameSettings.CAMERA_ZOOM_SPEED);
    }

    private void HandleGameQuit()
    {
        if (Input.GetKeyDown(ConstPlayerInput.QUIT_GAME_KEY))
        {
            Application.Quit();
        }
    }


}
