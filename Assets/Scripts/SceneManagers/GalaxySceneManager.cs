using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySceneManager : MonoBehaviour
{


    // prefabs
    public GameObject gridLinePrefab;
    public GameObject planetarySystemPrefab;

    // manager components
    public Functions functions;
    public PlayerFactory playerFactory;

    // UI
    public bool uiVisible = true;
    private Rect guiSceneTelemetryRect = new Rect(10, 10, 210, 110);

    // camera
    private float cameraSize;

    // scene metrics
    public int planetarySystemCount = 0;
    public int starCount = 0;
    public int planetCount = 0;


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
        this.functions = new Functions();
    }

    void Start()
    {
        this.cameraSize = Camera.main.orthographicSize;
        this.GenerateGrid();
        this.GeneratePlanetarySystems();
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

    private void OnGUI()
    {
        // right click held
        if (Input.GetMouseButton(1))
        {
            this.HandleCameraMovement();
        }
        this.DisplaySceneTelemetry();
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    // galaxy grid
    private void GenerateGrid()
    {
        const int GALAXY_BUFFER = 200;
        int galaxyLowerBound = -(Constants.GALAXY_SIZE / 2) - GALAXY_BUFFER;
        int galaxyUpperBound = (Constants.GALAXY_SIZE / 2) + GALAXY_BUFFER;
        for (int i = 0; i < galaxyUpperBound; i++)
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

    // procedural generation
    private void GeneratePlanetarySystems()
    {
        for (int i = 0; i < Constants.PLANETARY_SYSTEMS_COUNT; i++)
        {
            GameObject planetarySystem = Instantiate(this.planetarySystemPrefab, Vector3.zero, Quaternion.identity);
            this.planetarySystemCount += 1;
            planetarySystem.GetComponent<PlanetarySystemScript>().ProcGen();
        }
    }

    // player input
    private void HandleLeftClick()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 placementPosition = this.functions.GetIntRoundedVector3(new Vector3(mousePosition.x, mousePosition.y, 0));
        this.playerFactory.PlaceFactoryStructure(Constants.STRUCTURE_HARVESTER, placementPosition);
    }

    // camera
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
    private void HandleCameraMovement()
    {
        // scale camera move amount with size of camera view
        float vert = Input.GetAxis("Mouse Y") * Time.deltaTime * Camera.main.orthographicSize * Constants.CAMERA_MOVE_SPEED;
        float horiz = Input.GetAxis("Mouse X") * Time.deltaTime * Camera.main.orthographicSize * Constants.CAMERA_MOVE_SPEED;
        Camera.main.transform.Translate(new Vector3(-horiz, -vert, 0));
    }

    // UI
    private void DisplaySceneTelemetry()
    {
        if (this.uiVisible)
        {
            // show scene telemetry
            GUI.contentColor = Color.green;
            int fps = (int)(1.0f / Time.smoothDeltaTime);
            string displayText =
                "FPS: " + fps.ToString() +
                "\nPlanetary Systems: " + this.planetarySystemCount.ToString() +
                "\nStars: " + this.starCount.ToString() +
                "\nPlanets: " + this.planetCount.ToString();
            GUI.Label(
                this.guiSceneTelemetryRect,
                displayText
            );
        }
    }


}
