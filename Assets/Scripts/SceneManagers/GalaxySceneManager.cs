using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySceneManager : MonoBehaviour
{


    public GameObject gridLinePrefab;
    public GameObject starSystemPrefab;

    private int galaxySize = 500;
    private int starsQuantity = 100;


    // the static reference to the singleton instance
    public static GalaxySceneManager instance;


    // UNITY HOOKS

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        this.GenerateGrid();
        this.GenerateStarSystems();
    }

    void Update() { }

    private void OnGUI()
    {
        this.HandleCameraZoom();
        this.HandleCameraMovement();
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void GenerateGrid()
    {
        int galaxyLowerBound = -(this.galaxySize / 2);
        int galaxyUpperBound = (this.galaxySize / 2);
        for (int i = 0; i < this.galaxySize / 2; i++)
        {
            this.CreateYGridLine(galaxyLowerBound, galaxyUpperBound, i);
            this.CreateXGridLine(galaxyLowerBound, galaxyUpperBound, i);
            if (i > 0)
            {
                this.CreateYGridLine(galaxyLowerBound, galaxyUpperBound, -i);
                this.CreateXGridLine(galaxyLowerBound, galaxyUpperBound, -i);
            }
        }
    }

    // grid line creation helpers
    private void CreateYGridLine(int lowerBound, int upperBound, int xAxisPos)
    {
        GameObject yGridLine = Instantiate(this.gridLinePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer yLr = yGridLine.GetComponent<LineRenderer>();
        var yPoints = new Vector3[2];
        yPoints[0] = new Vector3(xAxisPos, lowerBound, 0);
        yPoints[1] = new Vector3(xAxisPos, upperBound, 0);
        yLr.SetPositions(yPoints);
    }
    private void CreateXGridLine(int lowerBound, int upperBound, int yAxisPos)
    {
        GameObject xGridLine = Instantiate(this.gridLinePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer xLr = xGridLine.GetComponent<LineRenderer>();
        var xPoints = new Vector3[2];
        xPoints[0] = new Vector3(lowerBound, yAxisPos, 0);
        xPoints[1] = new Vector3(upperBound, yAxisPos, 0);
        xLr.SetPositions(xPoints);
    }

    private void GenerateStarSystems()
    {
        for (int i = 0; i < this.starsQuantity; i++)
        {
            int randX = Random.Range(-(this.galaxySize / 2), this.galaxySize / 2);
            int randY = Random.Range(-(this.galaxySize / 2), this.galaxySize / 2);
            Instantiate(this.starSystemPrefab, new Vector3(randX, randY, 0), Quaternion.identity);
        }
    }

    private void HandleCameraZoom()
    {
        float zoomMultiplier = 4f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            zoomMultiplier = 10f;
        }
        const float CAMERA_SIZE_MIN = 10f;
        const float CAMERA_SIZE_MAX = 1000f;
        float cameraSize = Camera.main.orthographicSize;
        cameraSize -= Input.mouseScrollDelta.y * zoomMultiplier;
        if (cameraSize < CAMERA_SIZE_MIN)
        {
            cameraSize = CAMERA_SIZE_MIN;
        }
        else if (cameraSize > CAMERA_SIZE_MAX)
        {
            cameraSize = CAMERA_SIZE_MAX;
        }
        Camera.main.orthographicSize = cameraSize;
    }

    private void HandleCameraMovement()
    {
        const float CAMERA_MOVE_SPEED = 2f;
        if (Input.GetMouseButton(0))
        {
            float vert = Input.GetAxis("Mouse Y") * Time.deltaTime * Camera.main.orthographicSize * CAMERA_MOVE_SPEED;
            float horiz = Input.GetAxis("Mouse X") * Time.deltaTime * Camera.main.orthographicSize * CAMERA_MOVE_SPEED;
            Camera.main.transform.Translate(new Vector3(-horiz, -vert, 0));
        }
    }


}
