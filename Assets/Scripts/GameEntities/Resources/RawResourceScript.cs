using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawResourceScript : MonoBehaviour
{


    public Rigidbody2D rb;

    public int resourceType = Constants.RESOURCE_TYPE_NONE;

    private float launchForce = 0.0f;
    private Vector3 launchDirection;
    private bool hasLaunched = false;


    // UNITY HOOKS

    void Start()
    {
        this.rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!this.hasLaunched)
        {
            this.rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
            // TODO NEXT: DEBUG THE INITIAL ROTATION
            // this.transform.LookAt(launchDirection);
            this.hasLaunched = true;
        }
    }

    // INTERFACE METHODS

    public void SetLaunchForceAndDirection(float force, Vector3 direction)
    {
        this.launchForce = force;
        this.launchDirection = direction;
    }

    // IMPLEMENTATION METHODS


}
