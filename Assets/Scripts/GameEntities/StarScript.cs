using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{


    public GameObject lightEmissionPrefab;

    public int sizeRadius;
    public int luminosity;

    public Color starColor;
    private List<Color32> starColors = new List<Color32>
    {
        new Color32(147, 200, 203, 255),
        new Color32(233, 241, 250, 255),
        new Color32(187, 58, 15, 255),
        new Color32(229, 150, 97, 255)
    };
    private float starColorBrightnessMultiplier = 1.2f;

    private Vector3[] directions = new Vector3[4] {
        Vector3.up,
        Vector3.down,
        Vector3.right,
        Vector3.left
    };


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
        this.GenStarSize();
        // gen star color
        this.starColor = this.starColors[Random.Range(0, this.starColors.Count)];
        this.gameObject.GetComponent<SpriteRenderer>().color = this.starColor * this.starColorBrightnessMultiplier;
        // gen star luminosity
        this.luminosity = Random.Range(Constants.STAR_MIN_LUMINOSITY, Constants.STAR_MAX_LUMINOSITY);
        InvokeRepeating("EmitLight", 0.0f, 1.0f / luminosity);
    }

    // IMPLEMENTATION METHODS

    private void GenStarSize()
    {
        this.sizeRadius = Random.Range(Constants.STAR_MIN_SIZE_RADIUS, Constants.STAR_MAX_SIZE_RADIUS);
        this.transform.localScale = new Vector3(this.sizeRadius, this.sizeRadius, 0);
    }

    private void EmitLight()
    {
        Vector3 randDirection = this.directions[Random.Range(0, this.directions.Length)];
        GameObject lightEmission = Instantiate(this.lightEmissionPrefab, this.transform.position, Quaternion.identity);
        var leScript = lightEmission.GetComponent<LightEmissionScript>();
        if (randDirection.Equals(Vector3.up) || randDirection.Equals(Vector3.down))
        {
            leScript.RotateEmissionVertical();
        }
        leScript.emissionDirection = randDirection;
        leScript.SetColor(this.starColor);
    }


}
