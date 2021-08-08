using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData
{


    // RAW RESOURCES

    public List<int> rawResourceTypes = new List<int>() {
        Constants.RESOURCE_TYPE_WATER,
        Constants.RESOURCE_TYPE_GAS,
        Constants.RESOURCE_TYPE_STONE,
        Constants.RESOURCE_TYPE_METAL,
        Constants.RESOURCE_TYPE_ORGANICS
    };

    public IDictionary<int, string> rawResourceTypeToDisplayName = new Dictionary<int, string>()
    {
        { Constants.RESOURCE_TYPE_NONE, "none" },
        { Constants.RESOURCE_TYPE_WATER, "water" },
        { Constants.RESOURCE_TYPE_GAS, "gas" },
        { Constants.RESOURCE_TYPE_STONE, "stone" },
        { Constants.RESOURCE_TYPE_METAL, "metal" },
        { Constants.RESOURCE_TYPE_ORGANICS, "organics" }
    };

    public IDictionary<int, Color> rawResourceTypeToColor = new Dictionary<int, Color>()
    {
        { Constants.RESOURCE_TYPE_WATER, Color.blue },
        { Constants.RESOURCE_TYPE_GAS, Color.yellow },
        { Constants.RESOURCE_TYPE_STONE, Color.gray },
        { Constants.RESOURCE_TYPE_METAL, Color.magenta },
        { Constants.RESOURCE_TYPE_ORGANICS, Color.green }
    };


}
