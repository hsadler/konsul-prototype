using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetarySystemScript : MonoBehaviour
{


    public GameObject starPrefab;
    public GameObject planetPrefab;

    public List<GameObject> stars = new List<GameObject>();
    public List<GameObject> planets = new List<GameObject>();


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
        this.GenStars();
        this.GenPlanets();
    }

    // IMPLEMENTATION METHODS

    private void GenLocation()
    {
        int galaxyLowerBound = -(Constants.GALAXY_SIZE / 2);
        int galaxyUpperBound = (Constants.GALAXY_SIZE / 2);
        int randX = Random.Range(galaxyLowerBound, galaxyUpperBound);
        int randY = Random.Range(galaxyLowerBound, galaxyUpperBound);
        this.transform.position = new Vector3(randX, randY, 0);
    }

    private void GenStars()
    {
        GameObject star = Instantiate(starPrefab, this.transform.position, Quaternion.identity, this.transform);
        star.GetComponent<StarScript>().ProcGen();
        this.stars.Add(star);
    }

    private void GenPlanets()
    {
        // STUB
    }


}
