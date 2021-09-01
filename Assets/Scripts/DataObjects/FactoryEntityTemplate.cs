using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityTemplate
{


    public int type;
    public int group;
    public string displayName;
    public Sprite sprite;
    public GameObject prefab;

    public bool isProcessable = false;
    public IDictionary<int, int> processedTo;

    public bool isAssemblerProduct = false;
    public IDictionary<int, int> assembledFrom;

    public bool isRefineryProduct = false;
    public IDictionary<int, int> refinedFrom;

    public bool isFurnaceProduct = false;
    public IDictionary<int, int> furnacedFrom;

    public bool isBiolabProduct = false;
    public IDictionary<int, int> biolabedFrom;


    public FactoryEntityTemplate(int type, int group, string displayName, Sprite sprite, GameObject prefab = null)
    {
        this.type = type;
        this.group = group;
        this.displayName = displayName;
        this.sprite = sprite;
        this.prefab = prefab;
    }

    // INTERFACE METHODS

    public void SetProcessedTo(IDictionary<int, int> products)
    {
        this.isProcessable = true;
        this.processedTo = products;
    }

    public void SetAssebledFrom(IDictionary<int, int> constituents)
    {
        this.isAssemblerProduct = true;
        this.assembledFrom = constituents;
    }

    public void SetRefinedFrom(IDictionary<int, int> constituents)
    {
        this.isRefineryProduct = true;
        this.refinedFrom = constituents;
    }

    public void SetFurnacedFrom(IDictionary<int, int> constituents)
    {
        this.isFurnaceProduct = true;
        this.furnacedFrom = constituents;
    }

    public void SetBiolabedFrom(IDictionary<int, int> constituents)
    {
        this.isBiolabProduct = true;
        this.biolabedFrom = constituents;
    }


}
