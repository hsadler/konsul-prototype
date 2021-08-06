using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureIOBehavior : MonoBehaviour
{


    public GameObject resourceIOPrefab;

    public List<GameObject> resourceIOs = new List<GameObject>();


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryStructureIOPlacementEvent.AddListener(this.AddIO);
    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public GameObject RotateSelection()
    {
        // return early if empty
        if (this.resourceIOs.Count == 0)
        {
            return null;
        }
        else
        {
            GameObject selectedRsIO = null;
            int currSelectedIndex = -1;
            // determine current selected index if there is one
            for (int i = 0; i < this.resourceIOs.Count; i++)
            {
                GameObject currRsIO = this.resourceIOs[i];
                var rsIOScript = currRsIO.GetComponent<ResourceIOScript>();
                if (rsIOScript.isSelected)
                {
                    rsIOScript.Deselect();
                    currSelectedIndex = i;
                }
            }
            // select next
            if (currSelectedIndex > -1)
            {
                if (currSelectedIndex == this.resourceIOs.Count - 1)
                {
                    selectedRsIO = this.resourceIOs[0];
                }
                else
                {
                    selectedRsIO = this.resourceIOs[currSelectedIndex + 1];
                }
            }
            // none selected, select first 
            else
            {
                selectedRsIO = this.resourceIOs[0];
            }
            // set selected with selected state
            selectedRsIO.GetComponent<ResourceIOScript>().Select();
            return selectedRsIO;
        }
    }

    // IMPLEMENTATION METHODS

    private void AddIO(GameObject from, GameObject to)
    {
        if (from == this.gameObject)
        {
            Vector3 direction = (to.transform.position - from.transform.position).normalized;
            Vector3 tlPos = this.transform.position + direction;
            GameObject resourceIO = Instantiate(resourceIOPrefab, tlPos, Quaternion.identity, this.transform);
            var lr = resourceIO.GetComponent<LineRenderer>();
            var points = new Vector3[2];
            points[0] = Vector3.zero;
            points[1] = to.transform.position - resourceIO.transform.position - direction;
            lr.SetPositions(points);
            this.resourceIOs.Add(resourceIO);
        }
    }


}
