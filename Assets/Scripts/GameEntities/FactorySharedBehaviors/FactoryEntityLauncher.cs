using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntityLauncher : MonoBehaviour
{


    // UNITY HOOKS

    void Start()
    {

    }

    void Update()
    {

    }

    // INTERFACE METHODS

    public void Launch(int feType, Vector3 direction, float impulse)
    {
        GameObject go = Instantiate(
            GalaxySceneManager.instance.playerFactory.inTransitFEPrefab,
            this.transform.position + direction,
            Quaternion.identity
        );
        var inTransitFE = go.GetComponent<InTransitFEScript>();
        inTransitFE.FactoryEntityType = feType;
        inTransitFE.LauncherGameObjectId = this.gameObject.GetInstanceID();
        var feLaunchable = go.GetComponent<FactoryEntityLaunchable>();
        feLaunchable.SetLaunchForceAndDirection(impulse, direction);
        feLaunchable.Launch();
    }

    // IMPLEMENTATION METHODS


}
