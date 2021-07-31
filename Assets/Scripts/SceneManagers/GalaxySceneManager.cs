using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxySceneManager : MonoBehaviour
{


    public GameObject starSystemPrefab;

    private int galaxySize = 500;
    private int starsQuantity = 200;


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
        this.GenerateStarSystems();
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void GenerateStarSystems()
    {
        for (int i = 0; i < this.starsQuantity; i++)
        {
            int randX = Random.Range(-(this.galaxySize / 2), this.galaxySize / 2);
            int randY = Random.Range(-(this.galaxySize / 2), this.galaxySize / 2);
            Instantiate(this.starSystemPrefab, new Vector3(randX, randY, 0), Quaternion.identity);
        }
    }


}
