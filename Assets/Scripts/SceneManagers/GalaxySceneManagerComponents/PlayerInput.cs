using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    private float cameraSize;


    // UNITY HOOKS

    void Start()
    {
        this.cameraSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        // upon escape press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        // upon left click
        if (Input.GetMouseButtonDown(0))
        {
            this.HandleLeftClick();
        }
        this.HandleCameraZoom();
    }

    void OnGUI()
    {
        // right click held
        if (Input.GetMouseButton(1))
        {
            this.HandleCameraMovement();
        }
    }

    // IMPLEMENTATION METHODS
    private void HandleLeftClick()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 placementPosition = GalaxySceneManager.instance.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
        GalaxySceneManager.instance.playerFactory.PlaceFactoryStructure(Constants.STRUCTURE_HARVESTER, placementPosition);
    }

    private void HandleCameraMovement()
    {
        // scale camera move amount with size of camera view
        float vert = Input.GetAxis("Mouse Y") * Time.deltaTime * Camera.main.orthographicSize * Constants.CAMERA_MOVE_SPEED;
        float horiz = Input.GetAxis("Mouse X") * Time.deltaTime * Camera.main.orthographicSize * Constants.CAMERA_MOVE_SPEED;
        Camera.main.transform.Translate(new Vector3(-horiz, -vert, 0));
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
