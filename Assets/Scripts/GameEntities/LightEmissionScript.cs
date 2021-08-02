using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmissionScript : MonoBehaviour
{


    public GameObject emissionGO;

    public int energy;
    public Vector3 emissionDirection;

    private float ttl = 1.0f;


    // UNITY HOOKS

    void Start()
    {
        Invoke("DestroySelf", this.ttl);
    }

    void Update()
    {
        this.Travel();
    }

    // INTERFACE METHODS

    public void SetColor(Color c)
    {
        this.emissionGO.GetComponent<SpriteRenderer>().color = c;
    }

    public void RotateEmissionVertical()
    {
        this.emissionGO.transform.Rotate(0, 0, 90);
    }

    // IMPLEMENTATION METHODS

    private void Travel()
    {
        this.gameObject.transform.Translate(this.emissionDirection * Constants.LIGHT_SPEED * Time.deltaTime, Space.World);
    }

    private void DestroySelf()
    {
        Object.Destroy(this.gameObject);
    }


}
