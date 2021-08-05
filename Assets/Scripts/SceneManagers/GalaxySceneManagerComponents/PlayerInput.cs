using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    private float cameraSize;

    public int inputMode;
    public IDictionary<int, string> inputModeToDisplayString = new Dictionary<int, string>()
    {
        { Constants.PLAYER_INPUT_MODE_INIT, "init" },
        { Constants.PLAYER_INPUT_MODE_PLACEMENT, "placement" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT, "select" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_IO, "io" },
    };

    public int currentPlacementStructureType;
    private IDictionary<UnityEngine.KeyCode, int> keyCodeToFactoryStructureType = new Dictionary<UnityEngine.KeyCode, int>()
    {
        { KeyCode.Alpha1, Constants.STRUCTURE_TYPE_HARVESTER },
        { KeyCode.Alpha2, Constants.STRUCTURE_TYPE_STORAGE },
        { KeyCode.Alpha3, Constants.STRUCTURE_TYPE_SPLITTER },
        { KeyCode.Alpha4, Constants.STRUCTURE_TYPE_MERGER },
    };

    public GameObject currentSelected;


    // UNITY HOOKS

    void Awake()
    {
        this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
        this.InitCurrentPlacementStrutureType();
    }

    void Start()
    {
        this.cameraSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        // TODO: maybe optimize these calls to be fewer since they can be treated as a tree
        this.HandlePlacementMode();
        this.HandlePlacement();
        this.HandleStructureSelection();
        this.HandleStructureIOMode();
        this.HandleCameraMovement();
        this.HandleCameraZoom();
        this.HandleModeRevert();
        // quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // IMPLEMENTATION METHODS

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
                this.DeselectStructures();
            }
        }
    }

    private void HandlePlacement()
    {
        // left click and placement mode
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_PLACEMENT && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
            GalaxySceneManager.instance.playerFactory.PlaceFactoryStructure(this.currentPlacementStructureType, placementPosition);
        }
    }

    private void HandleStructureSelection()
    {
        // left click and select mode
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_INIT && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject.CompareTag("FactoryStructure"))
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
                GalaxySceneManager.instance.factoryStructureSelectedEvent.Invoke(hit.gameObject);
                this.currentSelected = hit.gameObject;
            }
        }
    }

    private void HandleStructureIOMode()
    {
        // selection mode
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT && Input.GetKeyDown(KeyCode.Space))
        {
            this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_IO;
            // TODO NEXT: implement io logic
        }
    }

    private void HandleModeRevert()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_IO)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
                this.DeselectStructures();
            }
            else if (this.inputMode == Constants.PLAYER_INPUT_MODE_PLACEMENT)
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_INIT;
                this.InitCurrentPlacementStrutureType();
            }

        }
    }

    // factory building controls helpers

    private void DeselectStructures()
    {
        this.currentSelected = null;
        GalaxySceneManager.instance.factoryStructureDelesectAllEvent.Invoke();
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


}
