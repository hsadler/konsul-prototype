using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityBufferQueue : MonoBehaviour
{


    public int capacity = 1;

    private int contentsCount = 0;
    private Queue<int> entityQueue = new Queue<int>();


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public bool Add(int feType)
    {
        if (this.contentsCount < this.capacity)
        {
            this.entityQueue.Enqueue(feType);
            this.contentsCount += 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetNext()
    {
        if (!this.IsEmpty())
        {
            this.contentsCount -= 1;
            return this.entityQueue.Dequeue();
        }
        else
        {
            return ConstFEType.NONE;
        }
    }

    public bool IsEmpty()
    {
        return this.entityQueue.Count == 0;
    }

    public bool CapacityFull()
    {
        return this.contentsCount >= this.capacity;
    }

    public string GetStatus()
    {
        return "capacity: " + this.contentsCount.ToString() + "/" + this.capacity.ToString();
    }

    // IMPLEMENTATION METHODS


}
