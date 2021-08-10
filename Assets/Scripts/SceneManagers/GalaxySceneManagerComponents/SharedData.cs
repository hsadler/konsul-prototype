using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharedData
{


    // raw resources
    public List<int> rawResourceTypes = new List<int>()
    {
        Constants.RESOURCE_TYPE_WATER,
        Constants.RESOURCE_TYPE_GAS,
        Constants.RESOURCE_TYPE_STONE,
        Constants.RESOURCE_TYPE_METAL,
        Constants.RESOURCE_TYPE_ORGANICS
    };
    public IDictionary<int, Color> rawResourceTypeToColor = new Dictionary<int, Color>()
    {
        { Constants.RESOURCE_TYPE_WATER, Color.blue },
        { Constants.RESOURCE_TYPE_GAS, Color.yellow },
        { Constants.RESOURCE_TYPE_STONE, Color.gray },
        { Constants.RESOURCE_TYPE_METAL, Color.magenta },
        { Constants.RESOURCE_TYPE_ORGANICS, Color.green }
    };

    // intermediate resources
    public List<int> intermediateResourceTypes = new List<int>()
    {
    };

    // factory structures
    public List<int> factoryStructureTypes = new List<int>()
    {
        Constants.FACTORY_STRUCTURE_TYPE_HARVESTER,
        Constants.FACTORY_STRUCTURE_TYPE_DISTRIBUTOR,
        Constants.FACTORY_STRUCTURE_TYPE_STORAGE,
        Constants.FACTORY_STRUCTURE_TYPE_MIRROR,
        Constants.FACTORY_STRUCTURE_TYPE_PHOTOVOLTAIC,
        Constants.FACTORY_STRUCTURE_TYPE_ACCUMULATOR,
    };

    // factory units
    public List<int> factoryUnitTypes = new List<int>()
    {
        Constants.FACTORY_UNIT_TYPE_WORKER,
        Constants.FACTORY_UNIT_TYPE_PROBE,
        Constants.FACTORY_UNIT_TYPE_SYSTEM_EXPANSION_SHIP,
    };

    // all factory entities
    public List<int> allFactoryEntityTypes;
    public IDictionary<int, string> factoryEntityTypeToDisplayString = new Dictionary<int, string>()
    {
        { Constants.ENTITY_TYPE_NONE, "none" },
        // resources
        { Constants.RESOURCE_TYPE_WATER, "water" },
        { Constants.RESOURCE_TYPE_GAS, "gas" },
        { Constants.RESOURCE_TYPE_STONE, "stone" },
        { Constants.RESOURCE_TYPE_METAL, "metal" },
        { Constants.RESOURCE_TYPE_ORGANICS, "organics" },
        // structure
        { Constants.FACTORY_STRUCTURE_TYPE_HARVESTER, "harvester" },
        { Constants.FACTORY_STRUCTURE_TYPE_DISTRIBUTOR, "distributor" },
        { Constants.FACTORY_STRUCTURE_TYPE_STORAGE, "storage" },
        { Constants.FACTORY_STRUCTURE_TYPE_MIRROR, "mirror" },
        { Constants.FACTORY_STRUCTURE_TYPE_PHOTOVOLTAIC, "photovoltaic" },
        { Constants.FACTORY_STRUCTURE_TYPE_ACCUMULATOR, "accumulator" },
        // units
        { Constants.FACTORY_UNIT_TYPE_WORKER, "worker" },
        { Constants.FACTORY_UNIT_TYPE_PROBE, "probe" },
        { Constants.FACTORY_UNIT_TYPE_SYSTEM_EXPANSION_SHIP, "system expansion ship" },
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
