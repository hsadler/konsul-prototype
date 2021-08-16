using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryEntityReceiver : MonoBehaviour
{


    // TODO: may not be any reason to use a queue here..
    public Queue<int> feBuffer = new Queue<int>();

    private IFactoryStructure selfFactoryStructure;

    private bool canReceive = true;


    // UNITY HOOKS

    void Awake()
    {
        this.selfFactoryStructure = this.gameObject.GetComponent<IFactoryStructure>();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // factory-entity consumption
        if (this.canReceive && this.selfFactoryStructure.IsStructureActive && other.gameObject.CompareTag("FactoryEntity"))
        {
            // don't consume inactive structures (bugfix for placement-cursor object)
            var fs = other.gameObject.GetComponent<IFactoryStructure>();
            if (fs != null && !fs.IsStructureActive)
            {
                return;
            }
            // don't consume factory entities not in transit
            var fe = other.gameObject.GetComponent<IFactoryEntity>();
            if (fe != null && !fe.InTransit)
            {
                return;
            }
            // don't consume if self is the launcher
            if (fe.LauncherGameObjectId == this.gameObject.GetInstanceID())
            {
                return;
            }
            // add to the buffer
            this.feBuffer.Enqueue(fe.FactoryEntityType);
            Object.Destroy(other.gameObject);
        }
    }

    // INTERFACE METHODS

    public void SetCanReceive(bool status)
    {
        this.canReceive = status;
    }

    public List<int> GetBuffer()
    {
        List<int> items = this.feBuffer.ToList<int>();
        this.feBuffer = new Queue<int>();
        return items;
    }

    // IMPLEMENTATION METHODS


}
