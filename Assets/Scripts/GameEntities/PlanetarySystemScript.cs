using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetarySystemScript : MonoBehaviour
{


    public GameObject starPrefab;
    public GameObject planetPrefab;

    public List<GameObject> stars = new List<GameObject>();
    public List<GameObject> planets = new List<GameObject>();

    public string orbitDirection;

    private List<int> occupiedOrbits = new List<int>();


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
        this.GenLocation();
        this.GenOrbitDirection();
        this.GenStars();
        this.GenPlanets();
    }

    // IMPLEMENTATION METHODS

    private void GenLocation()
    {
        int galaxyLowerBound = -(Constants.GALAXY_SIZE / 2);
        int galaxyUpperBound = (Constants.GALAXY_SIZE / 2);
        int randX = Random.Range(galaxyLowerBound, galaxyUpperBound + 1);
        int randY = Random.Range(galaxyLowerBound, galaxyUpperBound + 1);
        this.transform.position = new Vector3(randX, randY, 0);
    }

    private void GenOrbitDirection()
    {
        int randDirectionValue = Random.Range(0, 2);
        this.orbitDirection = randDirectionValue == 0 ? "right" : "left";
    }

    private void GenStars()
    {
        GameObject star = Instantiate(starPrefab, this.transform.position, Quaternion.identity, this.transform);
        star.GetComponent<StarScript>().ProcGen();
        this.stars.Add(star);
        GalaxySceneManager.instance.starCount += 1;
    }

    private void GenPlanets()
    {
        int numPlanets = Random.Range(Constants.PLANETARY_SYSTEMS_MIN_PLANETS, Constants.PLANETARY_SYSTEMS_MAX_PLANETS + 1);
        for (int i = 0; i < numPlanets; i++)
        {
            GameObject planet = Instantiate(planetPrefab, this.transform.position, Quaternion.identity, this.transform);
            var planetScript = planet.GetComponentInChildren<PlanetScript>();
            int orbit = planetScript.ProcGen(this.orbitDirection, this.occupiedOrbits);
            this.occupiedOrbits.Add(orbit);
            this.planets.Add(planet);
            GalaxySceneManager.instance.planetCount += 1;
        }
    }


}
