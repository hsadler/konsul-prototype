using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureIOBehavior : MonoBehaviour
{


    public GameObject transitLinePrefab;


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryStructureIOPlacementEvent.AddListener(this.AddIO);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    // IMPLEMENTATION METHODS

    private void AddIO(GameObject from, GameObject to)
    {
        if (from == this.gameObject)
        {
            Vector3 direction = (to.transform.position - from.transform.position).normalized;
            Vector3 tlPos = this.transform.position + direction;
            GameObject transitLine = Instantiate(transitLinePrefab, tlPos, Quaternion.identity, this.transform);
            var lr = transitLine.GetComponent<LineRenderer>();
            var points = new Vector3[2];
            points[0] = Vector3.zero;
            points[1] = to.transform.position - transitLine.transform.position - direction;
            lr.SetPositions(points);
        }
    }


}
