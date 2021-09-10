using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureBehavior : MonoBehaviour
{


    public SpriteRenderer sr;
    public LineRenderer buildProgressBarLR;

    private IFactoryEntity fe;
    private IFactoryStructure fs;

    private float prePlaceAlpha = 0.5f;
    private float ghostAlpha = 0.3f;
    private float aliveAlpha = 1f;

    private IDictionary<int, int> constituentPartsRequired;
    private Stack<int> constituentPartsReceived = new Stack<int>();


    // UNITY HOOKS

    void Awake()
    {
        this.fe = this.gameObject.GetComponent<IFactoryEntity>();
        this.fs = this.gameObject.GetComponent<IFactoryStructure>();
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        this.InitBuildProgressBar();
        this.constituentPartsRequired = GalaxySceneManager.instance.feData.GetFETemplate(this.fe.FactoryEntityType).assembledFrom;
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
        this.InitBuildProgressBar();
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

    public bool AddConstituentPart(int feTypeReceived)
    {
        int totalPartsRequiredAmount = 0;
        int partReceivedRequiredAmount = 0;
        foreach (KeyValuePair<int, int> entry in this.constituentPartsRequired)
        {
            int partFeType = entry.Key;
            int requiredAmount = entry.Value;
            totalPartsRequiredAmount += requiredAmount;
            if (partFeType == feTypeReceived)
            {
                partReceivedRequiredAmount = requiredAmount;
            }
        }
        int totalPartReceivedAmount = 0;
        foreach (var currFeType in this.constituentPartsReceived)
        {
            if (currFeType == feTypeReceived)
            {
                totalPartReceivedAmount += 1;
            }
        }
        if (totalPartReceivedAmount > partReceivedRequiredAmount)
        {
            return false;
        }
        else
        {
            this.constituentPartsReceived.Push(feTypeReceived);
            // structure fully built, set active
            if (totalPartsRequiredAmount == this.constituentPartsReceived.Count)
            {
                this.ActivateStructure();
            }
            else
            {
                float fractionComplete = (float)this.constituentPartsReceived.Count / (float)totalPartsRequiredAmount;
                this.SetBuildProgressBar(fractionComplete);
            }
            return true;
        }
    }

    public int RemoveLastConstituentPart()
    {
        if (this.constituentPartsReceived.Count > 0)
        {
            int removedPart = this.constituentPartsReceived.Pop();
            int totalPartsRequiredAmount = 0;
            foreach (int partCount in this.constituentPartsRequired.Values)
            {
                totalPartsRequiredAmount += partCount;
            }
            float fractionComplete = (float)this.constituentPartsReceived.Count / (float)totalPartsRequiredAmount;
            this.SetBuildProgressBar(fractionComplete);
            return removedPart;
        }
        else
        {
            return ConstFEType.NONE;
        }
    }

    // IMPLEMENTATION METHODS

    private void SetSpriteAlpha(float alpha)
    {
        Color oldColor = this.sr.color;
        this.sr.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
    }

    private void InitBuildProgressBar()
    {
        this.buildProgressBarLR.SetPosition(1, Vector3.zero);
    }

    private void SetBuildProgressBar(float fraction)
    {
        this.buildProgressBarLR.SetPosition(1, new Vector3(fraction, 0, 0));
    }

}
