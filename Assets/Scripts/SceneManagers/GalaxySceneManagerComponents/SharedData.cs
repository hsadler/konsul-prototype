using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharedData
{


    // raw resources
    public List<int> rawResourceTypes = new List<int>()
    {
        ConstFEType.WATER,
        ConstFEType.GAS,
        ConstFEType.STONE,
        ConstFEType.METAL,
        ConstFEType.ORGANICS,
    };
    public IDictionary<int, Color> rawResourceTypeToColor = new Dictionary<int, Color>()
    {
        { ConstFEType.WATER, Color.blue },
        { ConstFEType.GAS, Color.yellow },
        { ConstFEType.STONE, Color.gray },
        { ConstFEType.METAL, Color.magenta },
        { ConstFEType.ORGANICS, Color.green },
    };

    // intermediate resources
    public List<int> intermediateResourceTypes = new List<int>()
    {
    };

    // factory structures
    public List<int> factoryStructureTypes = new List<int>()
    {
        ConstFEType.HARVESTER,
        ConstFEType.DISTRIBUTOR,
        ConstFEType.STORAGE,
        ConstFEType.RAW_RESOURCE_PROCESSOR,
        ConstFEType.MIRROR,
        ConstFEType.PHOTOVOLTAIC,
        ConstFEType.ACCUMULATOR,
    };

    // factory units
    public List<int> factoryUnitTypes = new List<int>()
    {
        ConstFEType.WORKER,
        ConstFEType.PROBE,
        ConstFEType.SYSTEM_EXPANSION_SHIP,
    };

    // all factory entities
    public List<int> allFactoryEntityTypes;
    public IDictionary<int, string> factoryEntityTypeToDisplayString = new Dictionary<int, string>()
    {
        { ConstFEType.NONE, "none" },
        // resources
        { ConstFEType.WATER, "water" },
        { ConstFEType.GAS, "gas" },
        { ConstFEType.STONE, "stone" },
        { ConstFEType.METAL, "metal" },
        { ConstFEType.ORGANICS, "organics" },
        // structure
        { ConstFEType.HARVESTER, "harvester" },
        { ConstFEType.DISTRIBUTOR, "distributor" },
        { ConstFEType.STORAGE, "storage" },
        { ConstFEType.RAW_RESOURCE_PROCESSOR, "raw resource processor" },
        { ConstFEType.MIRROR, "mirror" },
        { ConstFEType.PHOTOVOLTAIC, "photovoltaic" },
        { ConstFEType.ACCUMULATOR, "accumulator" },
        // units
        { ConstFEType.WORKER, "worker" },
        { ConstFEType.PROBE, "probe" },
        { ConstFEType.SYSTEM_EXPANSION_SHIP, "system expansion ship" },
    };


    public SharedData()
    {
        // aggregate all resource types to a single list
        IEnumerable<int> bucket = new List<int>();
        bucket = bucket
            .Concat(this.rawResourceTypes)
            .Concat(this.intermediateResourceTypes)
            .Concat(this.factoryStructureTypes)
            .Concat(this.factoryUnitTypes);
        this.allFactoryEntityTypes = new List<int>(bucket);
    }


}
