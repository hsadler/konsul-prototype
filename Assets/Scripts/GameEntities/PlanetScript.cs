using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{


    public GameObject planetBody;
    public GameObject waypoint1;
    public GameObject waypoint2;
    public GameObject waypoint3;
    public GameObject waypoint4;
    public GameObject orbitLine;

    public float sizeRadius;

    public int orbitRadius;
    public int orbitSpeed;
    public string orbitDirection;

    public float pctWater;
    public float pctGases;
    public float pctRocks;
    public float pctMetals;
    public float pctOrganics;

    private float planetColorBrightnessMultiplier = 0.9f;


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public int ProcGen(List<int> occupiedOrbits)
    {
        this.GenPlanetSize();
        this.GenPlanetColor();
        this.GenPlanetComposition();
        this.GenPlanetOrbit(occupiedOrbits);
        return this.orbitRadius;
    }

    // IMPLEMENTATION METHODS

    private void GenPlanetSize()
    {
        this.sizeRadius = Random.Range(Constants.PLANET_MIN_SIZE_RADIUS, Constants.PLANET_MAX_SIZE_RADIUS);
        this.planetBody.transform.localScale = new Vector3(this.sizeRadius, this.sizeRadius, 0);
    }

    private void GenPlanetColor()
    {
        Color planetColor = new Color32(
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

    private void GenPlanetOrbit(List<int> occupiedOrbits)
    {
        // generate an unoccupied random orbit
        int randOrbit;
        do
        {
            randOrbit = Random.Range(Constants.PLANET_MIN_ORBIT_RADIUS, Constants.PLANET_MAX_ORBIT_RADIUS);
        } while (occupiedOrbits.Contains(randOrbit));
        this.orbitRadius = randOrbit;
        // generate random orbit direction
        int randDirectionValue = Random.Range(0, 2);
        this.orbitDirection = randDirectionValue == 0 ? "right" : "left";
        // set waypoint locations
        this.waypoint1.transform.localPosition = new Vector3(this.orbitRadius, this.orbitRadius, 0);
        if (this.orbitDirection == "right")
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
        // TODO: maybe change this to start at a random position along the path
        this.planetBody.transform.localPosition = this.waypoint1.transform.localPosition;
    }

    private void GenPlanetComposition()
    {
        // STUB
    }


}
