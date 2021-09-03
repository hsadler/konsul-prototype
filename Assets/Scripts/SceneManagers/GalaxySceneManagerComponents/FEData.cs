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
    public GameObject mirrorPrefab;
    public GameObject photovoltaicPrefab;
    public GameObject accumulatorPrefab;
    // unit prefabs
    public GameObject workerPrefab;
    public GameObject probePrefab;
    public GameObject systemExpansionShipPrefab;

    [Space(20)]

    // factory entity sprites
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
    public Sprite harvesterSprite;
    public Sprite distributorSprite;
    public Sprite storageSprite;
    public Sprite resourceProcessorSprite;
    public Sprite mirrorSprite;
    public Sprite photovoltaicSprite;
    public Sprite accumulatorSprite;

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

    // IMPLEMENTATION METHODS

    private void PopulateFactoryEntityTemplates()
    {

        this.feTypeToFETemplate = new Dictionary<int, FactoryEntityTemplate>();

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

        // TODO: add more resource templates


        // STRUCTURES

        // harvester
        var harvesterTemplate = new FactoryEntityTemplate(
            type: ConstFEType.HARVESTER,
            group: ConstFEGroup.STRUCTURE,
            displayName: "harvester",
            sprite: this.harvesterSprite,
            prefab: this.harvesterPrefab
        );
        harvesterTemplate.SetAssebledFrom(new Dictionary<int, int>() {
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
        distributorTemplate.SetAssebledFrom(new Dictionary<int, int>()
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
        storageTemplate.SetAssebledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.IRON, 5 },
            { ConstFEType.STONE, 20 }
        });
        this.feTypeToFETemplate.Add(ConstFEType.STORAGE, storageTemplate);
        // resource processor
        var resourceProcessor = new FactoryEntityTemplate(
            type: ConstFEType.RESOURCE_PROCESSOR,
            group: ConstFEGroup.STRUCTURE,
            displayName: "resource processor",
            sprite: this.resourceProcessorSprite,
            prefab: this.resourceProcessorPrefab
        );
        resourceProcessor.SetAssebledFrom(new Dictionary<int, int>()
        {
            { ConstFEType.DISTRIBUTOR, 10 },
            { ConstFEType.STORAGE, 1 }
        });
        this.feTypeToFETemplate.Add(ConstFEType.RESOURCE_PROCESSOR, resourceProcessor);

        // TODO: add more structure templates

        // TODO: add unit templates

    }


}
