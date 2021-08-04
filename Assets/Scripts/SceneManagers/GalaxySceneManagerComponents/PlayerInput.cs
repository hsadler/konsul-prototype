using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    private float cameraSize;

    public int inputMode;
    public IDictionary<int, string> inputModeToDisplayString = new Dictionary<int, string>()
    {
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT, "select" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_EDIT, "edit" },
        { Constants.PLAYER_INPUT_MODE_STRUCTURE_PLACE, "place" },
    };

    public int currentlySelectedFactoryStructureType = Constants.STRUCTURE_TYPE_HARVESTER;
    private IDictionary<UnityEngine.KeyCode, int> keyToFactoryStructureType = new Dictionary<UnityEngine.KeyCode, int>()
    {
        { KeyCode.Alpha1, Constants.STRUCTURE_TYPE_HARVESTER },
        { KeyCode.Alpha2, Constants.STRUCTURE_TYPE_STORAGE },
        { KeyCode.Alpha3, Constants.STRUCTURE_TYPE_SPLITTER },
        { KeyCode.Alpha4, Constants.STRUCTURE_TYPE_MERGER },
    };


    // UNITY HOOKS

    void Awake()
    {
        this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
    }

    void Start()
    {
        this.cameraSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        this.HandleFactoryStructurePlacementSelection();
        this.HandleFactoryStructurePlacement();
        this.HandleFactoryStructureSelect();
        this.HandleFactoryStructureEdit();
        this.HandleCameraMovement();
        this.HandleCameraZoom();
        // mode change
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT;
        }
        // quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // IMPLEMENTATION METHODS

    private void HandleFactoryStructurePlacementSelection()
    {
        // any of the number keys containing a factory structure are pressed
        foreach (var numkey in this.keyToFactoryStructureType.Keys)
        {
            if (Input.GetKeyDown(numkey))
            {
                this.inputMode = Constants.PLAYER_INPUT_MODE_STRUCTURE_PLACE;
                this.currentlySelectedFactoryStructureType = this.keyToFactoryStructureType[numkey];
            }
        }
    }

    private void HandleFactoryStructurePlacement()
    {
        // left click and placement mode
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_PLACE && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
            GalaxySceneManager.instance.playerFactory.PlaceFactoryStructure(this.currentlySelectedFactoryStructureType, placementPosition);
        }
    }

    private void HandleFactoryStructureSelect()
    {
        // left click and select mode
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_SELECT && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject.CompareTag("FactoryStructure"))
            {
                GalaxySceneManager.instance.factoryStructureSelectedEvent.Invoke(hit.gameObject);
            }
        }
    }

    private void HandleFactoryStructureEdit()
    {
        // left click and edit mode
        if (this.inputMode == Constants.PLAYER_INPUT_MODE_STRUCTURE_EDIT && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
            GalaxySceneManager.instance.playerFactory.PlaceFactoryStructure(this.currentlySelectedFactoryStructureType, placementPosition);
        }
    }

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
