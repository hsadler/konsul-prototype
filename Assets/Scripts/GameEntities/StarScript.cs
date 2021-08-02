using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{


    public GameObject starLightBeamPrefab;

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

    private int[] starLightRotations = new int[4] { 0, 90, 180, -90 };


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
        this.GenStarColor();
        this.GenStarLight();
    }

    // IMPLEMENTATION METHODS

    private void GenStarSize()
    {
        this.sizeRadius = Random.Range(Constants.STAR_MIN_SIZE_RADIUS, Constants.STAR_MAX_SIZE_RADIUS);
        this.transform.localScale = new Vector3(this.sizeRadius * 2, this.sizeRadius * 2, 0);
    }

    private void GenStarColor()
    {
        this.starColor = this.starColors[Random.Range(0, this.starColors.Count)];
        this.gameObject.GetComponent<SpriteRenderer>().color = this.starColor * this.starColorBrightnessMultiplier;
    }

    private void GenStarLight()
    {
        // gen luminosity
        this.luminosity = Random.Range(Constants.STAR_MIN_LUMINOSITY, Constants.STAR_MAX_LUMINOSITY);
        // create star light beams based on star size and color
        foreach (int rot in this.starLightRotations)
        {
            for (int i = 0; i < this.sizeRadius; i++)
            {
                this.CreateLightBeam(i, rot);
                if (i > 0)
                {
                    this.CreateLightBeam(-i, rot);
                }
            }
        }
    }

    private void CreateLightBeam(int offset, int rotation)
    {
        var starLightBeam = Instantiate(
            this.starLightBeamPrefab,
            this.transform.position,
            Quaternion.identity
        );
        starLightBeam.transform.Rotate(new Vector3(0, 0, rotation));
        if (rotation == 0 || rotation == 180)
        {
            starLightBeam.transform.localPosition += new Vector3(offset, 0, 0);
        }
        else
        {
            starLightBeam.transform.localPosition += new Vector3(0, offset, 0);
        }
        var slbScript = starLightBeam.GetComponent<StarLightBeamScript>();
        var lineRenderer = slbScript.beam.GetComponent<LineRenderer>();
        // TODO NEXT
        // lineRenderer.startColor = this.starColor;
        // lineRenderer.endColor = this.starColor;
    }


}
