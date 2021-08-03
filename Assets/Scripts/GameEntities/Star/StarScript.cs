using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{


    public GameObject starLightBeamPrefab;

    public int sizeRadius;

    public int starType;
    private int[] starTypes = new int[4] { 1, 2, 3, 4 };

    public Color starColor;
    private Color32[] starColors = new Color32[4]
    {
        // red
        new Color32(187, 58, 15, 255),
        // yellow
        new Color32(230, 188, 108, 255),
        // blue
        new Color32(113, 214, 234, 255),
        // white
        new Color32(233, 241, 250, 255)
    };
    private float starColorBrightnessMultiplier = 1.5f;

    public int luminosity;
    public int[] luminosities = new int[4] { 10, 20, 30, 40 };

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
        this.GenStarType();
        this.GenStarSize();
        this.SetStarColor();
        this.SetStarLight();
    }

    // IMPLEMENTATION METHODS

    private void GenStarType()
    {
        // assign random type
        this.starType = this.starTypes[Random.Range(0, this.starTypes.Length)];
    }

    private void GenStarSize()
    {
        // assign random size
        this.sizeRadius = Random.Range(Constants.STAR_MIN_SIZE_RADIUS, Constants.STAR_MAX_SIZE_RADIUS + 1);
        this.transform.localScale = new Vector3(this.sizeRadius * 2, this.sizeRadius * 2, 0);
    }

    private void SetStarColor()
    {
        // assign color from star type
        this.starColor = this.starColors[this.starType - 1];
        this.gameObject.GetComponent<SpriteRenderer>().color = this.starColor * this.starColorBrightnessMultiplier;
    }

    private void SetStarLight()
    {
        // assign luminosity from star type
        this.luminosity = this.luminosities[Random.Range(0, this.luminosities.Length)];
        // create star light beams based on star size and star type (color)
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
        // create gameobject
        var starLightBeam = Instantiate(
            this.starLightBeamPrefab,
            this.transform.position,
            Quaternion.identity
        );
        // do rotations and offsets
        starLightBeam.transform.Rotate(new Vector3(0, 0, rotation));
        if (rotation == 0 || rotation == 180)
        {
            starLightBeam.transform.localPosition += new Vector3(offset, 0, 0);
        }
        else
        {
            starLightBeam.transform.localPosition += new Vector3(0, offset, 0);
        }
        // set beam luminosity, color, and length
        var slbScript = starLightBeam.GetComponent<StarLightBeamScript>();
        slbScript.luminosity = this.luminosity;
        var lineRenderer = slbScript.beam.GetComponent<LineRenderer>();
        // TODO: tuning values
        lineRenderer.startColor = new Color(this.starColor.r, this.starColor.g, this.starColor.b, lineRenderer.startColor.a);
        lineRenderer.endColor = new Color(this.starColor.r, this.starColor.g, this.starColor.b, lineRenderer.endColor.a);
        lineRenderer.SetPosition(
            1,
            new Vector3(0, lineRenderer.GetPosition(1).y + (this.sizeRadius * 10), 0)
        );
    }


}
