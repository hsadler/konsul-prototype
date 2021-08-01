using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{


    public float sizeRadius;

    public int orbitRadius;
    public int orbitSpeed;

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

    public void ProcGen()
    {
        this.GenPlanetSize();
        this.GenPlanetColor();
        this.GenPlanetOrbit();
        this.GenPlanetComposition();
    }

    // IMPLEMENTATION METHODS

    private void GenPlanetSize()
    {
        this.sizeRadius = Random.Range(Constants.PLANET_MIN_SIZE_RADIUS, Constants.PLANET_MAX_SIZE_RADIUS);
        this.transform.localScale = new Vector3(this.sizeRadius, this.sizeRadius, 0);
    }

    private void GenPlanetColor()
    {
        Color planetColor = new Color32(
            this.GetRandomPlanetRGB(),
            this.GetRandomPlanetRGB(),
            this.GetRandomPlanetRGB(),
            255
        );
        this.gameObject.GetComponent<SpriteRenderer>().color = planetColor * this.planetColorBrightnessMultiplier;
    }

    private byte GetRandomPlanetRGB()
    {
        return (byte)Random.Range(50f, 150f);
    }

    private void GenPlanetOrbit()
    {
        // TODO: implement (this is a mock)
        float randX = Random.Range(-10f, 10f);
        float randY = Random.Range(-10f, 10f);
        this.transform.localPosition = new Vector3(randX, randY, 0);
    }

    private void GenPlanetComposition()
    {
        // STUB
    }


}
