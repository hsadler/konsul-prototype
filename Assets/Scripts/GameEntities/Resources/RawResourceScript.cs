using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawResourceScript : MonoBehaviour
{


    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public int resourceType = Constants.ENTITY_TYPE_NONE;
    public int launcherGameObjectId;

    private float launchForce = 0.0f;
    private Vector3 launchDirection;
    private bool hasLaunched = false;


    // UNITY HOOKS

    void Start()
    {
        // set capsule color based on resource type
        this.sr.color = GalaxySceneManager.instance.sharedData.rawResourceTypeToColor[this.resourceType];
        GalaxySceneManager.instance.factoryResourceItemsInTransit += 1;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!this.hasLaunched)
        {
            this.rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward, launchDirection);
            this.hasLaunched = true;
        }
    }

    void OnDestroy()
    {
        GalaxySceneManager.instance.factoryResourceItemsInTransit -= 1;
    }

    // INTERFACE METHODS

    public void SetLaunchForceAndDirection(float force, Vector3 direction)
    {
        this.launchForce = force;
        this.launchDirection = direction;
    }

    // IMPLEMENTATION METHODS


}
