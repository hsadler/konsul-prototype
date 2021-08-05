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

    private void AddIO(GameObject fromGO, GameObject toGO)
    {
        if (fromGO == this.gameObject)
        {
            GameObject transitLine = Instantiate(transitLinePrefab, this.transform.position, Quaternion.identity);
            var lr = transitLine.GetComponent<LineRenderer>();
            var points = new Vector3[2];
            points[0] = Vector3.zero;
            points[1] = toGO.transform.position - fromGO.transform.position;
            lr.SetPositions(points);
        }
    }


}
