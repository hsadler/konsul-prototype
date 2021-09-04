using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{


    // game object references
    public GameObject planetBody;
    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject waypoint3;
    public GameObject waypoint4;
    public GameObject orbitLine;

    // size
    public float sizeRadius;

    // orbit
    public int orbitRadius;
    public int orbitSpeed;
    private Vector3[] waypointPositions;
    private int currentWaypointIndex;

    private List<int> resources = new List<int>();
    private IDictionary<int, int> resourceTypeToCount = new Dictionary<int, int>() {
        { ConstFEType.WATER, 0 },
        { ConstFEType.GAS, 0 },
        { ConstFEType.STONE, 0 },
        { ConstFEType.METAL, 0 },
        { ConstFEType.ORGANICS, 0 }
    };

    // planet color
    public Color planetColor;
    private float planetColorBrightnessMultiplier = 0.9f;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {
        this.MovePlanetAlongOrbit();
    }

    // INTERFACE METHODS

    public int ProcGen(string orbitDirection, List<int> occupiedOrbits)
    {
        this.GenPlanetSize();
        this.GenPlanetColor();
        this.GenPlanetResourceComposition();
        this.GenPlanetOrbit(orbitDirection, occupiedOrbits);
        return this.orbitRadius;
    }

    public int ExtractResource()
    {
        return this.resources[Random.Range(0, this.resources.Count)];
    }

    // IMPLEMENTATION METHODS

    private void GenPlanetSize()
    {
        this.sizeRadius = Random.Range(GameSettings.PLANET_MIN_SIZE_RADIUS, GameSettings.PLANET_MAX_SIZE_RADIUS);
        this.planetBody.transform.localScale = new Vector3(this.sizeRadius * 2, this.sizeRadius * 2, 0);
    }

    private void GenPlanetColor()
    {
        this.planetColor = new Color32(
            this.GetRandomPlanetRGB(),
            this.GetRandomPlanetRGB(),
            this.GetRandomPlanetRGB(),
            255
        );
        this.planetBody.GetComponent<SpriteRenderer>().color = planetColor * this.planetColorBrightnessMultiplier;
    }

    private byte GetRandomPlanetRGB()
    {
        return (byte)Random.Range(50f, 150f);
    }

    private void GenPlanetOrbit(string orbitDirection, List<int> occupiedOrbits)
    {
        // generate an unoccupied random orbit
        int randOrbit;
        do
        {
            randOrbit = Random.Range(GameSettings.PLANET_MIN_ORBIT_RADIUS, GameSettings.PLANET_MAX_ORBIT_RADIUS + 1);
        } while (occupiedOrbits.Contains(randOrbit));
        this.orbitRadius = randOrbit;
        // set waypoint positions
        this.waypoint1.transform.localPosition = new Vector3(this.orbitRadius, this.orbitRadius, 0);
        if (orbitDirection == "right")
        {
            this.waypoint2.transform.localPosition = new Vector3(this.orbitRadius, -this.orbitRadius, 0);
            this.waypoint3.transform.localPosition = new Vector3(-this.orbitRadius, -this.orbitRadius, 0);
            this.waypoint4.transform.localPosition = new Vector3(-this.orbitRadius, this.orbitRadius, 0);
        }
        else
        {
            this.waypoint2.transform.localPosition = new Vector3(-this.orbitRadius, this.orbitRadius, 0);
            this.waypoint3.transform.localPosition = new Vector3(-this.orbitRadius, -this.orbitRadius, 0);
            this.waypoint4.transform.localPosition = new Vector3(this.orbitRadius, -this.orbitRadius, 0);
        }
        // set waypoint positions array
        this.waypointPositions = new Vector3[4] {
            this.waypoint1.transform.localPosition,
            this.waypoint2.transform.localPosition,
            this.waypoint3.transform.localPosition,
            this.waypoint4.transform.localPosition
        };
        // set current waypoint
        this.currentWaypointIndex = 1;
        // set orbit speed
        this.orbitSpeed = Random.Range(GameSettings.PLANET_MIN_ORBIT_SPEED, GameSettings.PLANET_MAX_ORBIT_SPEED + 1);
        // set orbit line
        var orbitLineRenderer = this.orbitLine.GetComponent<LineRenderer>();
        var linePositions = new Vector3[5];
        linePositions[0] = this.waypoint1.transform.localPosition;
        linePositions[1] = this.waypoint2.transform.localPosition;
        linePositions[2] = this.waypoint3.transform.localPosition;
        linePositions[3] = this.waypoint4.transform.localPosition;
        linePositions[4] = this.waypoint1.transform.localPosition;
        orbitLineRenderer.SetPositions(linePositions);
        // set initial planet position on orbit path
        // TODO: change this to start at a random position along the path
        this.planetBody.transform.localPosition = this.waypoint1.transform.localPosition;
    }

    private void GenPlanetResourceComposition()
    {
        var rawResourceTypes = new List<int>
        {
            ConstFEType.WATER,
            ConstFEType.GAS,
            ConstFEType.STONE,
            ConstFEType.METAL,
            ConstFEType.ORGANICS,
        };
        // populate resources list
        foreach (int resource in rawResourceTypes)
        {
            int resourceSampleAmount = Random.Range(0, 11);
            for (int i = 0; i < resourceSampleAmount; i++)
            {
                this.resources.Add(resource);
                this.resourceTypeToCount[resource] += 1;
            }
        }
    }

    private void MovePlanetAlongOrbit()
    {
        Vector3 currentWaypointPosition = this.waypointPositions[this.currentWaypointIndex];
        if (Vector3.Distance(this.planetBody.transform.localPosition, currentWaypointPosition) < 0.001f)
        {
            this.currentWaypointIndex = this.currentWaypointIndex < 3 ? this.currentWaypointIndex + 1 : 0;
            currentWaypointPosition = this.waypointPositions[this.currentWaypointIndex];
        }
        this.planetBody.transform.localPosition = Vector3.MoveTowards(
            this.planetBody.transform.localPosition,
            currentWaypointPosition,
            this.orbitSpeed * Time.deltaTime
        );
    }


}
