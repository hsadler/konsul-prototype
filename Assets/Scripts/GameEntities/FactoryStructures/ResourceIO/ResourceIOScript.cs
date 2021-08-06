using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceIOScript : MonoBehaviour
{


    public LineRenderer lineRenderer;
    public bool isSelected;

    private float unSelectedLineWidth = 0.12f;
    private float selectedLineWidth = 0.14f;
    private float unSelectedLineOpacity = 0.4f;
    private float selectedLineOpacity = 0.8f;


    // UNITY HOOKS

    void Start()
    {
        this.isSelected = false;
        this.SetLineWidthAndOpacity(this.unSelectedLineWidth, this.unSelectedLineOpacity);
        GalaxySceneManager.instance.factoryStructureIODelesectAllEvent.AddListener(Deselect);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void Select()
    {
        this.isSelected = true;
        this.SetLineWidthAndOpacity(this.selectedLineWidth, this.selectedLineOpacity);
    }

    public void Deselect()
    {
        this.isSelected = false;
        this.SetLineWidthAndOpacity(this.unSelectedLineWidth, this.unSelectedLineOpacity);
    }

    // IMPLEMENTATION METHODS

    private void SetLineWidthAndOpacity(float width, float opacity)
    {
        LineRenderer lr = this.lineRenderer;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.startColor = new Color(
            lr.startColor.r,
            lr.startColor.g,
            lr.startColor.b,
            opacity
        );
        lr.endColor = new Color(
            lr.endColor.r,
            lr.endColor.g,
            lr.endColor.b,
            opacity
        );
    }


}
