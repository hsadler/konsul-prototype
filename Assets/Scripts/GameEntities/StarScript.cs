using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{


    public int sizeRadius;
    public int luminosity;

    private List<Color32> starColors = new List<Color32>
    {
        new Color32(147, 200, 203, 255),
        new Color32(233, 241, 250, 255),
        new Color32(238, 95, 47, 255),
        new Color32(229, 150, 97, 255)
    };
    private float starColorBrightnessMultiplier = 2f;


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
        this.sizeRadius = Random.Range(Constants.STAR_MIN_SIZE_RADIUS, Constants.STAR_MAX_SIZE_RADIUS);
        this.transform.localScale = new Vector3(this.sizeRadius, this.sizeRadius, 0);
        // gen star color
        Color starColor = this.starColors[Random.Range(0, this.starColors.Count)];
        this.gameObject.GetComponent<SpriteRenderer>().color = starColor * this.starColorBrightnessMultiplier;
        // gen star luminosity
        this.luminosity = Random.Range(Constants.STAR_MIN_LUMINOSITY, Constants.STAR_MAX_LUMINOSITY);
    }

    // IMPLEMENTATION METHODS

    private void EmitLight()
    {
        // STUB
    }


}
