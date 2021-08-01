using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{


    public int sizeRadius;
    public int luminosity;


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
        // gen star size
        this.sizeRadius = Random.Range(Constants.STAR_MIN_RADIUS, Constants.STAR_MAX_RADIUS);
        this.transform.localScale = new Vector3(this.sizeRadius, this.sizeRadius, 0);
        // gen star luminosity
        this.luminosity = Random.Range(Constants.STAR_MIN_LUMINOSITY, Constants.STAR_MAX_LUMINOSITY);
    }

    // IMPLEMENTATION METHODS

    private void EmitLight()
    {
        // STUB
    }


}
