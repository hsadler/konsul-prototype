using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FEData : MonoBehaviour
{


    // structure prefabs
    public GameObject harvesterPrefab;
    public GameObject distributorPrefab;
    public GameObject storagePrefab;
    public GameObject resourceProcessorPrefab;
    public GameObject furnacePrefab;
    public GameObject refineryPrefab;
    public GameObject assemblerPrefab;
    public GameObject biolabPrefab;
    public GameObject mirrorPrefab;
    public GameObject photovoltaicPrefab;
    public GameObject accumulatorPrefab;
    [Space(10)]
    // unit prefabs
    public GameObject workerPrefab;
    public GameObject probePrefab;
    public GameObject systemExpansionShipPrefab;

    [Space(20)]

    // factory entity sprites
    public Sprite noneSprite;

    public Sprite waterSprite;
    public Sprite gasSprite;
    public Sprite stoneSprite;
    public Sprite metalSprite;
    public Sprite organicsSprite;
    [Space(10)]
    public Sprite nitrogenSprite;
    public Sprite heliumSprite;
    public Sprite co2Sprite;
    public Sprite hydrogenSprite;
    public Sprite oxygenSprite;
    public Sprite silicatesSprite;
    public Sprite quartzSprite;
    public Sprite ironSprite;
    public Sprite copperSprite;
    public Sprite aluminumSprite;
    public Sprite leadSprite;
    public Sprite rareMetalsSprite;
    public Sprite biomassSprite;
    public Sprite cellCultureSprite;
    [Space(10)]
    public Sprite carbonSprite;
    public Sprite laserSprite;
    public Sprite radiationShieldingSprite;
    public Sprite steelSprite;
    public Sprite ceramicsSprite;
    public Sprite cementSprite;
    public Sprite glassSprite;
    public Sprite siliconSprite;
    public Sprite petroleumSprite;
    public Sprite plasticSprite;
    public Sprite electronicsSprite;
    public Sprite cpuSprite;
    public Sprite computerSprite;
    public Sprite programmedCellsSprite;
    [Space(10)]
    public Sprite harvesterSprite;
    public Sprite distributorSprite;
    public Sprite storageSprite;
    public Sprite resourceProcessorSprite;
    public Sprite furnaceSprite;
    public Sprite refinerySprite;
    public Sprite assemblerSprite;
    public Sprite biolabSprite;
    public Sprite mirrorSprite;
    public Sprite photovoltaicSprite;
    public Sprite accumulatorSprite;
    [Space(10)]
    public Sprite workerSprite;
    public Sprite probeSprite;
    public Sprite systemExpansionShipSprite;

    private IDictionary<int, FactoryEntityTemplate> feTypeToFETemplate;


    // UNITY HOOKS

    void Awake()
    {
        this.PopulateFactoryEntityTemplates();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public FactoryEntityTemplate GetFETemplate(int feType)
    {
        if (this.feTypeToFETemplate.ContainsKey(feType))
        {
            return this.feTypeToFETemplate[feType];
        }
        else
        {
            Debug.LogWarning("Factory Entity Template not found by feType: " + feType.ToString());
            return null;
        }
    }

    public List<int> GetAllFETypes()
    {
        return new List<int>(this.feTypeToFETemplate.Keys);
    }

    public string GetDisplayNameFromFEType(int feType)
    {
        return this.GetFETemplate(feType).displayName;
    }

    // IMPLEMENTATION METHODS

    private void PopulateFactoryEntityTemplates()
    {

        this.feTypeToFETemplate = new Dictionary<int, FactoryEntityTemplate>();

        // TODO: REFACTOR: consider removing NONE type FE Template
        // none
        var noneTemplate = new FactoryEntityTemplate(
            type: ConstFEType.NONE,
            group: ConstFEGroup.RESOURCE,
            displayName: "none",
            sprite: this.noneSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.NONE, noneTemplate);


        // RESOURCES

        // water
        var waterTemplate = new FactoryEntityTemplate(
            type: ConstFEType.WATER,
            group: ConstFEGroup.RESOURCE,
            displayName: "water",
            sprite: this.waterSprite,
            prefab: null
        );
        waterTemplate.SetProcessedTo(new Dictionary<int, int>() {
            { ConstFEType.HYDROGEN, 66 },
            { ConstFEType.OXYGEN, 33 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.WATER, waterTemplate);
        // gas
        var gasTemplate = new FactoryEntityTemplate(
            type: ConstFEType.GAS,
            group: ConstFEGroup.RESOURCE,
            displayName: "gas",
            sprite: this.gasSprite,
            prefab: null
        );
        gasTemplate.SetProcessedTo(new Dictionary<int, int>() {
            { ConstFEType.NITROGEN, 40 },
            { ConstFEType.HELIUM, 5 },
            { ConstFEType.CO2, 5 },
            { ConstFEType.HYDROGEN, 20 },
            { ConstFEType.OXYGEN, 30 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.GAS, gasTemplate);
        // stone
        var stoneTemplate = new FactoryEntityTemplate(
            type: ConstFEType.STONE,
            group: ConstFEGroup.RESOURCE,
            displayName: "stone",
            sprite: this.stoneSprite,
            prefab: null
        );
        stoneTemplate.SetProcessedTo(new Dictionary<int, int>() {
            { ConstFEType.SILICATES, 90 },
            { ConstFEType.QUARTZ, 10 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.STONE, stoneTemplate);
        // metal
        var metalTemplate = new FactoryEntityTemplate(
            type: ConstFEType.METAL,
            group: ConstFEGroup.RESOURCE,
            displayName: "metal",
            sprite: this.metalSprite,
            prefab: null
        );
        metalTemplate.SetProcessedTo(new Dictionary<int, int>() {
            { ConstFEType.IRON, 60 },
            { ConstFEType.COPPER, 20 },
            { ConstFEType.ALUMINUM, 10 },
            { ConstFEType.LEAD, 8 },
            { ConstFEType.RARE_METALS, 2 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.METAL, metalTemplate);
        // organics
        var organicsTemplate = new FactoryEntityTemplate(
            type: ConstFEType.ORGANICS,
            group: ConstFEGroup.RESOURCE,
            displayName: "organics",
            sprite: this.organicsSprite,
            prefab: null
        );
        organicsTemplate.SetProcessedTo(new Dictionary<int, int>() {
            { ConstFEType.BIOMASS, 98 },
            { ConstFEType.CELL_CULTURE, 2 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.ORGANICS, organicsTemplate);

        // nitrogen
        var nitrogenTemplate = new FactoryEntityTemplate(
            type: ConstFEType.NITROGEN,
            group: ConstFEGroup.RESOURCE,
            displayName: "nitrogen",
            sprite: this.nitrogenSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.NITROGEN, nitrogenTemplate);
        // helium
        var heliumTemplate = new FactoryEntityTemplate(
            type: ConstFEType.HELIUM,
            group: ConstFEGroup.RESOURCE,
            displayName: "helium",
            sprite: this.heliumSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.HELIUM, heliumTemplate);
        // co2
        var co2Template = new FactoryEntityTemplate(
            type: ConstFEType.CO2,
            group: ConstFEGroup.RESOURCE,
            displayName: "co2",
            sprite: this.co2Sprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.CO2, co2Template);
        // hydrogen
        var hydrogenTemplate = new FactoryEntityTemplate(
            type: ConstFEType.HYDROGEN,
            group: ConstFEGroup.RESOURCE,
            displayName: "hydrogen",
            sprite: this.hydrogenSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.HYDROGEN, hydrogenTemplate);
        // oxygen
        var oxygenTemplate = new FactoryEntityTemplate(
            type: ConstFEType.OXYGEN,
            group: ConstFEGroup.RESOURCE,
            displayName: "oxygen",
            sprite: this.oxygenSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.OXYGEN, oxygenTemplate);
        // silicates
        var silicatesTemplate = new FactoryEntityTemplate(
            type: ConstFEType.SILICATES,
            group: ConstFEGroup.RESOURCE,
            displayName: "silicates",
            sprite: this.silicatesSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.SILICATES, silicatesTemplate);
        // quartz
        var quartzTemplate = new FactoryEntityTemplate(
            type: ConstFEType.QUARTZ,
            group: ConstFEGroup.RESOURCE,
            displayName: "quartz",
            sprite: this.quartzSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.QUARTZ, quartzTemplate);
        // iron
        var ironTemplate = new FactoryEntityTemplate(
            type: ConstFEType.IRON,
            group: ConstFEGroup.RESOURCE,
            displayName: "iron",
            sprite: this.ironSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.IRON, ironTemplate);
        // copper
        var copperTemplate = new FactoryEntityTemplate(
            type: ConstFEType.COPPER,
            group: ConstFEGroup.RESOURCE,
            displayName: "copper",
            sprite: this.copperSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.COPPER, copperTemplate);
        // aluminum
        var aluminumTemplate = new FactoryEntityTemplate(
            type: ConstFEType.ALUMINUM,
            group: ConstFEGroup.RESOURCE,
            displayName: "aluminum",
            sprite: this.aluminumSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.ALUMINUM, aluminumTemplate);
        // lead
        var leadTemplate = new FactoryEntityTemplate(
            type: ConstFEType.LEAD,
            group: ConstFEGroup.RESOURCE,
            displayName: "lead",
            sprite: this.leadSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.LEAD, leadTemplate);
        // rare metals
        var rareMetalsTemplate = new FactoryEntityTemplate(
            type: ConstFEType.RARE_METALS,
            group: ConstFEGroup.RESOURCE,
            displayName: "rare metals",
            sprite: this.rareMetalsSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.RARE_METALS, rareMetalsTemplate);
        // biomass
        var biomassTemplate = new FactoryEntityTemplate(
            type: ConstFEType.BIOMASS,
            group: ConstFEGroup.RESOURCE,
            displayName: "biomass",
            sprite: this.biomassSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.BIOMASS, biomassTemplate);
        // cell culture
        var cellCultureTemplate = new FactoryEntityTemplate(
            type: ConstFEType.CELL_CULTURE,
            group: ConstFEGroup.RESOURCE,
            displayName: "cell culture",
            sprite: this.cellCultureSprite,
            prefab: null
        );
        this.feTypeToFETemplate.Add(ConstFEType.CELL_CULTURE, cellCultureTemplate);

        // carbon
        var carbonTemplate = new FactoryEntityTemplate(
            type: ConstFEType.CARBON,
            group: ConstFEGroup.RESOURCE,
            displayName: "carbon",
            sprite: this.carbonSprite,
            prefab: null
        );
        carbonTemplate.SetFurnacedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.BIOMASS, 10 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.CARBON, carbonTemplate);
        // laser
        var laserTemplate = new FactoryEntityTemplate(
            type: ConstFEType.LASER,
            group: ConstFEGroup.RESOURCE,
            displayName: "laser",
            sprite: this.laserSprite,
            prefab: null
        );
        laserTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.HELIUM, 10 },
            { ConstFEType.QUARTZ, 1 },
            { ConstFEType.IRON, 5 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.LASER, laserTemplate);
        // radiation shielding
        var radiationShieldingTemplate = new FactoryEntityTemplate(
            type: ConstFEType.RADIATION_SHIELDING,
            group: ConstFEGroup.RESOURCE,
            displayName: "radiation shielding",
            sprite: this.radiationShieldingSprite,
            prefab: null
        );
        radiationShieldingTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.ALUMINUM, 5 },
            { ConstFEType.LEAD, 20 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.RADIATION_SHIELDING, radiationShieldingTemplate);
        // steel
        var steelTemplate = new FactoryEntityTemplate(
            type: ConstFEType.STEEL,
            group: ConstFEGroup.RESOURCE,
            displayName: "steel",
            sprite: this.steelSprite,
            prefab: null
        );
        steelTemplate.SetFurnacedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.CARBON, 5 },
            { ConstFEType.IRON, 20 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.STEEL, steelTemplate);
        // ceramics
        var ceramicsTemplate = new FactoryEntityTemplate(
            type: ConstFEType.CERAMICS,
            group: ConstFEGroup.RESOURCE,
            displayName: "ceramics",
            sprite: this.ceramicsSprite,
            prefab: null
        );
        ceramicsTemplate.SetFurnacedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.SILICATES, 10 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.CERAMICS, ceramicsTemplate);
        // cement
        var cementTemplate = new FactoryEntityTemplate(
            type: ConstFEType.CEMENT,
            group: ConstFEGroup.RESOURCE,
            displayName: "cement",
            sprite: this.cementSprite,
            prefab: null
        );
        cementTemplate.SetRefinedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.STONE, 5 },
            { ConstFEType.SILICATES, 20 },
            { ConstFEType.WATER, 30 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.CEMENT, cementTemplate);
        // glass
        var glassTemplate = new FactoryEntityTemplate(
            type: ConstFEType.GLASS,
            group: ConstFEGroup.RESOURCE,
            displayName: "glass",
            sprite: this.glassSprite,
            prefab: null
        );
        glassTemplate.SetFurnacedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.SILICATES, 30 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.GLASS, glassTemplate);
        // silicon
        var siliconTemplate = new FactoryEntityTemplate(
            type: ConstFEType.SILICON,
            group: ConstFEGroup.RESOURCE,
            displayName: "silicon",
            sprite: this.siliconSprite,
            prefab: null
        );
        siliconTemplate.SetFurnacedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.QUARTZ, 10 },
            { ConstFEType.CARBON, 5 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.SILICON, siliconTemplate);
        // petroleum
        var petroleumTemplate = new FactoryEntityTemplate(
            type: ConstFEType.PETROLEUM,
            group: ConstFEGroup.RESOURCE,
            displayName: "petroleum",
            sprite: this.petroleumSprite,
            prefab: null
        );
        petroleumTemplate.SetRefinedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.CARBON, 20 },
            { ConstFEType.HYDROGEN, 5 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.PETROLEUM, petroleumTemplate);
        // plastic
        var plasticTemplate = new FactoryEntityTemplate(
            type: ConstFEType.PLASTIC,
            group: ConstFEGroup.RESOURCE,
            displayName: "plastic",
            sprite: this.plasticSprite,
            prefab: null
        );
        plasticTemplate.SetRefinedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.PETROLEUM, 20 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.PLASTIC, plasticTemplate);
        // electronics
        var electronicsTemplate = new FactoryEntityTemplate(
            type: ConstFEType.ELECTRONICS,
            group: ConstFEGroup.RESOURCE,
            displayName: "electronics",
            sprite: this.electronicsSprite,
            prefab: null
        );
        electronicsTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.CERAMICS, 5 },
            { ConstFEType.SILICON, 20 },
            { ConstFEType.COPPER, 10 },
            { ConstFEType.PLASTIC, 5 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.ELECTRONICS, electronicsTemplate);
        // cpu
        var cpuTemplate = new FactoryEntityTemplate(
            type: ConstFEType.CPU,
            group: ConstFEGroup.RESOURCE,
            displayName: "cpu",
            sprite: this.cpuSprite,
            prefab: null
        );
        cpuTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.SILICON, 10 },
            { ConstFEType.COPPER, 10 },
            { ConstFEType.RARE_METALS, 10 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.CPU, cpuTemplate);
        // computer
        var computerTemplate = new FactoryEntityTemplate(
            type: ConstFEType.COMPUTER,
            group: ConstFEGroup.RESOURCE,
            displayName: "computer",
            sprite: this.computerSprite,
            prefab: null
        );
        computerTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.ELECTRONICS, 10 },
            { ConstFEType.CPU, 4 },
            { ConstFEType.QUARTZ, 1 },
            { ConstFEType.ALUMINUM, 20 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.COMPUTER, computerTemplate);
        // programmed cells
        var programmedCellsTemplate = new FactoryEntityTemplate(
            type: ConstFEType.PROGRAMMED_CELLS,
            group: ConstFEGroup.RESOURCE,
            displayName: "programmed cells",
            sprite: this.programmedCellsSprite,
            prefab: null
        );
        programmedCellsTemplate.SetBiolabedFrom(new Dictionary<int, int>()
        {
            { ConstFEType.CELL_CULTURE, 10 },
            // TODO: FUTURE: add more constituents here
        });
        this.feTypeToFETemplate.Add(ConstFEType.PROGRAMMED_CELLS, programmedCellsTemplate);


        // STRUCTURES

        // harvester
        var harvesterTemplate = new FactoryEntityTemplate(
            type: ConstFEType.HARVESTER,
            group: ConstFEGroup.STRUCTURE,
            displayName: "harvester",
            sprite: this.harvesterSprite,
            prefab: this.harvesterPrefab
        );
        harvesterTemplate.SetAssembledFrom(new Dictionary<int, int>() {
            { ConstFEType.IRON, 10 },
            { ConstFEType.LASER, 1 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.HARVESTER, harvesterTemplate);
        // distributor
        var distributorTemplate = new FactoryEntityTemplate(
            type: ConstFEType.DISTRIBUTOR,
            group: ConstFEGroup.STRUCTURE,
            displayName: "distributor",
            sprite: this.distributorSprite,
            prefab: this.distributorPrefab
        );
        distributorTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.IRON, 5 },
            { ConstFEType.COPPER, 5 }
        });
        this.feTypeToFETemplate.Add(ConstFEType.DISTRIBUTOR, distributorTemplate);
        // storage
        var storageTemplate = new FactoryEntityTemplate(
            type: ConstFEType.STORAGE,
            group: ConstFEGroup.STRUCTURE,
            displayName: "storage",
            sprite: this.storageSprite,
            prefab: this.storagePrefab
        );
        storageTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.IRON, 5 },
            { ConstFEType.STONE, 20 }
        });
        this.feTypeToFETemplate.Add(ConstFEType.STORAGE, storageTemplate);
        // resource processor
        var resourceProcessorTemplate = new FactoryEntityTemplate(
            type: ConstFEType.RESOURCE_PROCESSOR,
            group: ConstFEGroup.STRUCTURE,
            displayName: "resource processor",
            sprite: this.resourceProcessorSprite,
            prefab: this.resourceProcessorPrefab
        );
        resourceProcessorTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.DISTRIBUTOR, 10 },
            { ConstFEType.STORAGE, 1 }
        });
        this.feTypeToFETemplate.Add(ConstFEType.RESOURCE_PROCESSOR, resourceProcessorTemplate);
        // furnace
        var furnaceTemplate = new FactoryEntityTemplate(
            type: ConstFEType.FURNACE,
            group: ConstFEGroup.STRUCTURE,
            displayName: "furnace",
            sprite: this.furnaceSprite,
            prefab: this.furnacePrefab
        );
        furnaceTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.IRON, 5 },
            { ConstFEType.COPPER, 2 },
            { ConstFEType.SILICATES, 10 }
        });
        this.feTypeToFETemplate.Add(ConstFEType.FURNACE, furnaceTemplate);
        // mirror
        var mirrorTemplate = new FactoryEntityTemplate(
            type: ConstFEType.MIRROR,
            group: ConstFEGroup.STRUCTURE,
            displayName: "mirror",
            sprite: this.mirrorSprite,
            prefab: this.mirrorPrefab
        );
        mirrorTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.ALUMINUM, 5 },
            { ConstFEType.GLASS, 20 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.MIRROR, mirrorTemplate);
        // photovoltaic
        var photovoltaicTemplate = new FactoryEntityTemplate(
            type: ConstFEType.PHOTOVOLTAIC,
            group: ConstFEGroup.STRUCTURE,
            displayName: "photovoltaic",
            sprite: this.photovoltaicSprite,
            prefab: this.photovoltaicPrefab
        );
        photovoltaicTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.ALUMINUM, 10 },
            { ConstFEType.ELECTRONICS, 5 },
            { ConstFEType.GLASS, 10 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.PHOTOVOLTAIC, photovoltaicTemplate);
        // accumulator
        var accumulatorTemplate = new FactoryEntityTemplate(
            type: ConstFEType.ACCUMULATOR,
            group: ConstFEGroup.STRUCTURE,
            displayName: "accumulator",
            sprite: this.accumulatorSprite,
            prefab: this.accumulatorPrefab
        );
        accumulatorTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.STEEL, 5 },
            { ConstFEType.CARBON, 5 },
            { ConstFEType.PLASTIC, 10 },
            { ConstFEType.WATER, 50 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.ACCUMULATOR, accumulatorTemplate);

        // TODO: FUTURE: add more structure templates

        // UNITS

        // worker
        // TODO: FUTURE: replace placeholder
        var workerTemplate = new FactoryEntityTemplate(
            type: ConstFEType.WORKER,
            group: ConstFEGroup.UNIT,
            displayName: "worker",
            sprite: this.workerSprite,
            prefab: this.workerPrefab
        );
        workerTemplate.SetAssembledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.IRON, 5 },
        });
        this.feTypeToFETemplate.Add(ConstFEType.WORKER, workerTemplate);

        // TODO: FUTURE: add more unit templates

    }


}
