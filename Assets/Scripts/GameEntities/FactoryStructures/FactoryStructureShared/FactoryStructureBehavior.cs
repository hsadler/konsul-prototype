using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureBehavior : MonoBehaviour
{


    public SpriteRenderer sr;

    private IFactoryStructure fs;

    private float prePlaceAlpha = 0.5f;
    private float ghostAlpha = 0.3f;
    private float aliveAlpha = 1f;


    // UNITY HOOKS

    void Awake()
    {
        this.fs = this.gameObject.GetComponent<IFactoryStructure>();
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        if (this.fs.IsStructureActive)
        {
            GalaxySceneManager.instance.factoryStructureCount -= 1;
        }
        GalaxySceneManager.instance.playerFactory.RemoveFactoryEntityFromRegistry(this.gameObject);
    }

    // INTERFACE METHODS

    public void ActivateStructure()
    {
        this.fs.IsStructureActive = true;
        this.GiveActiveAppearance();
        GalaxySceneManager.instance.factoryStructureCount += 1;
        GalaxySceneManager.instance.playerFactory.AddFactoryEntityToRegistry(this.gameObject);
    }

    public void GiveActiveAppearance()
    {
        this.SetSpriteAlpha(this.aliveAlpha);
    }

    public void GiveGhostAppearance()
    {
        this.SetSpriteAlpha(this.ghostAlpha);
    }

    public void GivePrePlacementAppearance()
    {
        this.SetSpriteAlpha(this.prePlaceAlpha);
    }

    // IMPLEMENTATION METHODS

    private void SetSpriteAlpha(float alpha)
    {
        Color oldColor = this.sr.color;
        this.sr.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
    }


}
