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
    public SharedData sharedData;

    // MonoBehaviour manager components
    public PlayerInput playerInput;
    public PlayerFactory playerFactory;
    public WorkerTaskQueue workerTaskQueue;

    // UI
    public bool uiVisible = true;
    private Rect guiSceneTelemetryRect = new Rect(10, 10, 800, 2000);

    // scene metrics
    public int planetarySystemCount = 0;
    public int starCount = 0;
    public int planetCount = 0;
    public int factoryEntityCount = 0;
    public int factoryStructureCount = 0;
    public int factoryEntityItemsInTransit = 0;

    // unity events
    public FactoryEntitySelectedEvent factoryEntitySelectedEvent = new FactoryEntitySelectedEvent();
    public FactoryEntityDelesectAllEvent factoryEntityDelesectAllEvent = new FactoryEntityDelesectAllEvent();
    public FactoryStructureIODelesectAllEvent factoryStructureIODelesectAllEvent = new FactoryStructureIODelesectAllEvent();
    public FactoryStructureRemovalEvent factoryStructureRemovalEvent = new FactoryStructureRemovalEvent();
    public FactoryStructureIOPlacementEvent factoryStructureIOPlacementEvent = new FactoryStructureIOPlacementEvent();


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
        this.sharedData = new SharedData();
    }

    void Start()
    {
        this.GenerateGrid();
        this.GeneratePlanetarySystems();
    }

    void Update()
    {

    }

    private void OnGUI()
    {
        this.DisplaySceneInfo();
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

    // UI
    private void DisplaySceneInfo()
    {
        if (this.uiVisible)
        {
            // prepare data for display
            string playerInputMode = this.playerInput.inputModeToDisplayString[this.playerInput.inputMode];
            string selectedForPlacement = "none";
            if (this.playerInput.currentPlacementStructureType != 0)
            {
                selectedForPlacement = this.sharedData.factoryEntityTypeToDisplayString[this.playerInput.currentPlacementStructureType];
            }
            // show scene telemetry
            GUI.contentColor = Color.green;
            int fps = (int)(1.0f / Time.smoothDeltaTime);
            string displayText =
                "FPS: " + fps.ToString() +
                "\nPlanetary Systems: " + this.planetarySystemCount.ToString() +
                "\nStars: " + this.starCount.ToString() +
                "\nPlanets: " + this.planetCount.ToString() +
                "\nFactory Structures: " + this.factoryStructureCount.ToString() +
                "\nItems in Transit: " + this.factoryEntityItemsInTransit.ToString() +
                "\n" +
                "\nIs Admin Mode: " + this.playerInput.isAdminMode.ToString() +
                "\nPlayer Input Mode: " + playerInputMode +
                "\nSelected for Placement: " + selectedForPlacement +
                "\n" +
                "\n" + this.GetSelectedEntityInfo();
            GUI.Label(
                this.guiSceneTelemetryRect,
                displayText
            );
        }
    }

    private string GetSelectedEntityInfo()
    {
        if (this.playerInput.currentEntitySelected != null)
        {
            GameObject feGO = this.playerInput.currentEntitySelected;
            var feScript = feGO.GetComponent<IFactoryEntity>();
            string selectedInfo =
                "Selected Entity: " + this.sharedData.factoryEntityTypeToDisplayString[feScript.FactoryEntityType];
            selectedInfo +=
                "\n--------------------\n" +
                feScript.GetStringFormattedFactoryEntityInfo();
            return selectedInfo;
        }
        else
        {
            return "Selected Entity: none";
        }
    }


}
