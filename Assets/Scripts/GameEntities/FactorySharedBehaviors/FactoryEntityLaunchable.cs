using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityLaunchable : MonoBehaviour
{

    public Rigidbody2D rb;

    private IFactoryEntity fe;

    private float launchForce = 0.0f;
    private Vector3 launchDirection;
    private bool doLaunch = false;


    // UNITY HOOKS

    void Awake()
    {
        this.fe = this.GetComponent<IFactoryEntity>();
    }

    void Start()
    {
        GalaxySceneManager.instance.factoryEntityItemsInTransit += 1;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (this.doLaunch)
        {
            fe.InTransit = true;
            this.rb.AddForce(this.launchDirection * this.launchForce, ForceMode2D.Impulse);
            this.doLaunch = false;
        }
    }

    void OnDestroy()
    {
        GalaxySceneManager.instance.factoryEntityItemsInTransit -= 1;
    }

    // INTERFACE METHODS

    public void Launch()
    {
        this.doLaunch = true;
    }

    public void SetLaunchForceAndDirection(float force, Vector3 direction)
    {
        this.launchForce = force;
        this.launchDirection = direction;
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, this.launchDirection);
    }

    // IMPLEMENTATION METHODS


}
