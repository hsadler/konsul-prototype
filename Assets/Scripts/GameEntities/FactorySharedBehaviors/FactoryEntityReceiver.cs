using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityReceiver : MonoBehaviour
{


    public Queue<int> feBuffer = new Queue<int>();

    private IFactoryStructure selfFactoryStructure;


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

    void OnTriggerEnter2D(Collider2D other)
    {
        // factory-entity consumption
        if (this.selfFactoryStructure.IsStructureActive && other.gameObject.CompareTag("FactoryEntity"))
        {
            // don't consume inactive structures
            var fs = other.gameObject.GetComponent<IFactoryStructure>();
            if (fs != null && !fs.IsStructureActive)
            {
                return;
            }
            // add to the buffer
            var fe = other.gameObject.GetComponent<IFactoryEntity>();
            if (fe.LauncherGameObjectId != this.gameObject.GetInstanceID())
            {
                this.feBuffer.Enqueue(fe.FactoryEntityType);
                Object.Destroy(other.gameObject);
            }
        }
    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS


}
