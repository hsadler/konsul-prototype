using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetarySystemScript : MonoBehaviour
{


    public GameObject starPrefab;
    public GameObject planetPrefab;

    public List<GameObject> stars = new List<GameObject>();
    public List<GameObject> planets = new List<GameObject>();

    public int xCoord;
    public int yCoord;


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
        // STUB
    }

    // IMPLEMENTATION METHODS

    private void GenLocation()
    {
        // STUB
    }

    private void GenStars()
    {
        // STUB
    }

    private void GenPlanets()
    {
        // STUB
    }


}
