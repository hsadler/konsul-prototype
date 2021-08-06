using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStructureIOBehavior : MonoBehaviour
{


    public GameObject resourceIOPrefab;
    public int ioLimit = 100;

    private List<GameObject> resourceIOs = new List<GameObject>();
    // required for preventing dupes
    private IDictionary<string, GameObject> lineCoordsToResourceIO = new Dictionary<string, GameObject>();
    private IDictionary<int, Vector3> resourceIOIDToDirection = new Dictionary<int, Vector3>();
    private GameObject currentSelectedResourceIO;
    private GameObject currentSendingResourceIO;


    // UNITY HOOKS

    void Start()
    {
        GalaxySceneManager.instance.factoryStructureIOPlacementEvent.AddListener(this.AddResourceIO);
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
            int currSelectedIndex = this.GetSelectedResourceIOIndex();
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
            // unselect old
            this.resourceIOs[currSelectedIndex].GetComponent<ResourceIOScript>().Deselect();
            // set new as selected with selected state
            selectedRsIO.GetComponent<ResourceIOScript>().Select();
            this.currentSelectedResourceIO = selectedRsIO;
            return selectedRsIO;
        }
    }

    public void RemoveCurrentSelectedResourceIO()
    {
        int currSelectedIndex = this.GetSelectedResourceIOIndex();
        if (currSelectedIndex > -1)
        {
            GameObject currentSelectedResourceIO = this.resourceIOs[currSelectedIndex];
            // remove from both data structures
            this.resourceIOs.RemoveAt(currSelectedIndex);
            this.lineCoordsToResourceIO.Remove(this.GetFormattedLineCoordinatesFromResourceIO(this.currentSelectedResourceIO));
            Object.Destroy(currentSelectedResourceIO);
        }
    }

    public bool ResourceIOsExist()
    {
        return this.resourceIOs.Count > 0;
    }

    public Vector3 GetNextSendDirection()
    {
        int oldRsIOIndex = this.GetSendingResourceIOIndex();
        GameObject nextResourceIO;
        if (oldRsIOIndex == -1 || oldRsIOIndex == this.resourceIOs.Count - 1)
        {
            nextResourceIO = this.resourceIOs[0];
        }
        else
        {
            nextResourceIO = this.resourceIOs[oldRsIOIndex + 1];
        }
        this.currentSendingResourceIO = nextResourceIO;
        return this.resourceIOIDToDirection[nextResourceIO.GetInstanceID()];
    }

    // IMPLEMENTATION METHODS

    private void AddResourceIO(GameObject from, GameObject to)
    {
        if (this.resourceIOs.Count == this.ioLimit)
        {
            return;
        }
        if (from == this.gameObject)
        {
            float offset = 0.25f;
            Vector3 direction = (to.transform.position - from.transform.position).normalized;
            Vector3 tlPos = this.transform.position + (direction * offset);
            GameObject resourceIO = Instantiate(resourceIOPrefab, tlPos, Quaternion.identity, this.transform);
            var lr = resourceIO.GetComponent<LineRenderer>();
            var points = new Vector3[2];
            points[0] = Vector3.zero;
            points[1] = to.transform.position - resourceIO.transform.position - (direction * offset);
            lr.SetPositions(points);
            // check for a dupe, if exists, cancel
            string formattedCoords = this.GetFormattedLineCoordinatesFromResourceIO(resourceIO);
            if (this.lineCoordsToResourceIO.ContainsKey(formattedCoords))
            {
                Object.Destroy(resourceIO);
                return;
            }
            // add to data structures
            this.resourceIOs.Add(resourceIO);
            this.lineCoordsToResourceIO.Add(formattedCoords, resourceIO);
            this.resourceIOIDToDirection.Add(resourceIO.GetInstanceID(), direction);
        }
    }

    private int GetSelectedResourceIOIndex()
    {
        int currSelectedIndex = -1;
        // determine current selected index if there is one
        for (int i = 0; i < this.resourceIOs.Count; i++)
        {
            GameObject rsIO = this.resourceIOs[i];
            if (this.currentSelectedResourceIO == rsIO)
            {
                currSelectedIndex = i;
            }
        }
        return currSelectedIndex;
    }

    private int GetSendingResourceIOIndex()
    {
        int currSendingIndex = -1;
        // determine current sending index if there is one
        for (int i = 0; i < this.resourceIOs.Count; i++)
        {
            GameObject rsIO = this.resourceIOs[i];
            if (this.currentSendingResourceIO == rsIO)
            {
                currSendingIndex = i;
            }
        }
        return currSendingIndex;
    }

    private string GetFormattedLineCoordinatesFromResourceIO(GameObject resourceIO)
    {
        LineRenderer lr = resourceIO.GetComponent<LineRenderer>();
        string formattedCoords = lr.GetPosition(0).ToString() + lr.GetPosition(1).ToString();
        return formattedCoords;
    }


}
