using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{


    // GAME SETTINGS

    // camera settings
    public const float CAMERA_SIZE_MIN = 10f;
    public const float CAMERA_SIZE_MAX = 1000f;
    public const float CAMERA_ZOOM_SPEED = 5f;
    public const float CAMERA_MOVE_SPEED = 4f;

    // galaxy settings
    public const int GALAXY_SIZE = 3000;

    // planetary system settings
    public const int PLANETARY_SYSTEMS_COUNT = 200;
    public const int PLANETARY_SYSTEMS_MIN_PLANETS = 0;
    public const int PLANETARY_SYSTEMS_MAX_PLANETS = 8;

    public const int STAR_MIN_SIZE_RADIUS = 1;
    public const int STAR_MAX_SIZE_RADIUS = 4;

    // planet settings
    public const float PLANET_MIN_SIZE_RADIUS = 0.25f;
    public const float PLANET_MAX_SIZE_RADIUS = 1f;
    public const int PLANET_MIN_ORBIT_RADIUS = 5;
    public const int PLANET_MAX_ORBIT_RADIUS = 30;
    public const int PLANET_MIN_ORBIT_SPEED = 1;
    public const int PLANET_MAX_ORBIT_SPEED = 10;


}
