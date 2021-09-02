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
        // harvester
        var harvesterTemplate = new FactoryEntityTemplate(
            type: ConstFEType.HARVESTER,
            group: ConstFEGroup.STRUCTURE,
            displayName: "harvester",
            sprite: this.harvesterSprite,
            prefab: this.harvesterPrefab
        );
        harvesterTemplate.SetAssebledFrom(new Dictionary<int, int>() {
            { ConstFEType.IRON, 5 },
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
    }


}
