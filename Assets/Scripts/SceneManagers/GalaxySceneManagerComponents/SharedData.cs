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
    // TODO: DEPRECATED
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
        ConstFEType.NITROGEN,
        ConstFEType.HELIUM,
        ConstFEType.CO2,
        ConstFEType.HYDROGEN,
        ConstFEType.OXYGEN,
        ConstFEType.SILICATES,
        ConstFEType.QUARTZ,
        ConstFEType.IRON,
        ConstFEType.COPPER,
        ConstFEType.ALUMINUM,
        ConstFEType.LEAD,
        ConstFEType.RARE_METALS,
        ConstFEType.BIOMASS,
        ConstFEType.CELL_CULTURE,
    };

    // factory structures
    public List<int> factoryStructureTypes = new List<int>()
    {
        ConstFEType.HARVESTER,
        ConstFEType.DISTRIBUTOR,
        ConstFEType.STORAGE,
        ConstFEType.RESOURCE_PROCESSOR,
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

    // display string map
    public IDictionary<int, string> factoryEntityTypeToDisplayString = new Dictionary<int, string>()
    {
        { ConstFEType.NONE, "none" },
        // raw resources
        { ConstFEType.WATER, "water" },
        { ConstFEType.GAS, "gases" },
        { ConstFEType.STONE, "raw stone" },
        { ConstFEType.METAL, "raw metals" },
        { ConstFEType.ORGANICS, "organics" },
        // intermediate resources
        { ConstFEType.NITROGEN, "nitrogen" },
        { ConstFEType.HELIUM, "helium" },
        { ConstFEType.CO2, "co2" },
        { ConstFEType.HYDROGEN, "hydrogen" },
        { ConstFEType.OXYGEN, "oxygen" },
        { ConstFEType.SILICATES, "silicates" },
        { ConstFEType.QUARTZ, "quartz" },
        { ConstFEType.IRON, "iron" },
        { ConstFEType.COPPER, "copper" },
        { ConstFEType.ALUMINUM, "aluminum" },
        { ConstFEType.LEAD, "lead" },
        { ConstFEType.RARE_METALS, "rare metals" },
        { ConstFEType.BIOMASS, "biomass" },
        { ConstFEType.CELL_CULTURE, "cell culture" },
        // structure
        { ConstFEType.HARVESTER, "harvester" },
        { ConstFEType.DISTRIBUTOR, "distributor" },
        { ConstFEType.STORAGE, "storage" },
        { ConstFEType.RESOURCE_PROCESSOR, "resource processor" },
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
