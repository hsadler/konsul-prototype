using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{


    // camera settings
    public const float CAMERA_SIZE_MIN = 10f;
    public const float CAMERA_SIZE_MAX = 1000f;
    public const float CAMERA_ZOOM_SPEED = 5f;
    public const float CAMERA_MOVE_SPEED = 4f;


    // player input modes
    public const int PLAYER_INPUT_MODE_INIT = 1;
    public const int PLAYER_INPUT_MODE_PLACEMENT = 2;
    public const int PLAYER_INPUT_MODE_STRUCTURE_SELECT = 3;
    public const int PLAYER_INPUT_MODE_STRUCTURE_IO = 4;
    public const int PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT = 5;

    // player input key mappings
    public const KeyCode PLAYER_INPUT_STRUCTURE_IO_MODE_KEY = KeyCode.E;
    public const KeyCode PLAYER_INPUT_REVERT_MODE_KEY = KeyCode.Q;
    public const KeyCode PLAYER_INPUT_CYCLE_IO_SELECT = KeyCode.Tab;
    public const KeyCode PLAYER_INPUT_REMOVAL_KEY = KeyCode.D;
    public const KeyCode PLAYER_INPUT_QUIT_GAME_KEY = KeyCode.Escape;
    public const KeyCode PLAYER_INPUT_ADMIN_MODE_TOGGLE = KeyCode.A;
    public const KeyCode PLAYER_INPUT_ADMIN_POPULATE_STORAGE = KeyCode.P;


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

    // resource types
    public const int RESOURCE_TYPE_NONE = -1;
    public const int RESOURCE_TYPE_WATER = 1;
    public const int RESOURCE_TYPE_GAS = 2;
    public const int RESOURCE_TYPE_STONE = 3;
    public const int RESOURCE_TYPE_METAL = 4;
    public const int RESOURCE_TYPE_ORGANICS = 5;

    // factory structures types
    public const int FACTORY_STRUCTURE_TYPE_HARVESTER = 1;
    public const int FACTORY_STRUCTURE_TYPE_DISTRIBUTOR = 2;
    public const int FACTORY_STRUCTURE_TYPE_STORAGE = 3;
    public const int FACTORY_STRUCTURE_TYPE_MIRROR = 5;
    public const int FACTORY_STRUCTURE_TYPE_PHOTOVOLTAIC = 6;
    public const int FACTORY_STRUCTURE_TYPE_ACCUMULATOR = 7;

    // factory unit types
    public const int FACTORY_UNIT_TYPE_WORKER = 101;
    public const int FACTORY_UNIT_TYPE_PROBE = 102;
    public const int FACTORY_UNIT_TYPE_SYSTEM_EXPANSION_SHIP = 103;


}
