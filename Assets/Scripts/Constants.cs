using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{


    // PLAYER INPUT

    // player input modes
    public const int PLAYER_INPUT_MODE_INIT = 1;
    public const int PLAYER_INPUT_MODE_PLACEMENT = 2;
    public const int PLAYER_INPUT_MODE_FACTORY_ENTITY_SELECT = 3;
    public const int PLAYER_INPUT_MODE_STRUCTURE_IO = 4;
    public const int PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT = 5;

    // player input key mappings
    public const KeyCode PLAYER_INPUT_STRUCTURE_IO_MODE_KEY = KeyCode.E;
    public const KeyCode PLAYER_INPUT_REVERT_MODE_KEY = KeyCode.Q;
    public const KeyCode PLAYER_INPUT_CYCLE_IO_SELECT = KeyCode.Tab;
    public const KeyCode PLAYER_INPUT_REMOVAL_KEY = KeyCode.D;
    public const KeyCode PLAYER_INPUT_QUIT_GAME_KEY = KeyCode.Escape;
    public const KeyCode PLAYER_INPUT_ADMIN_MODE_TOGGLE = KeyCode.A;
    public const KeyCode PLAYER_INPUT_ADMIN_CREATE_WORKER = KeyCode.W;
    public const KeyCode PLAYER_INPUT_ADMIN_POPULATE_STORAGE = KeyCode.P;


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


    // FACTORY ENTITY TYPES (values must be unique across groups)

    public const int ENTITY_TYPE_NONE = -1;

    // resource types
    public const int RESOURCE_ENTITY_TYPE_WATER = 101;
    public const int RESOURCE_ENTITY_TYPE_GAS = 102;
    public const int RESOURCE_ENTITY_TYPE_STONE = 103;
    public const int RESOURCE_ENTITY_TYPE_METAL = 104;
    public const int RESOURCE_ENTITY_TYPE_ORGANICS = 105;

    // factory structures types
    public const int FACTORY_STRUCTURE_ENTITY_TYPE_HARVESTER = 201;
    public const int FACTORY_STRUCTURE_ENTITY_TYPE_DISTRIBUTOR = 202;
    public const int FACTORY_STRUCTURE_ENTITY_TYPE_STORAGE = 203;
    public const int FACTORY_STRUCTURE_ENTITY_TYPE_MIRROR = 205;
    public const int FACTORY_STRUCTURE_ENTITY_TYPE_PHOTOVOLTAIC = 206;
    public const int FACTORY_STRUCTURE_ENTITY_TYPE_ACCUMULATOR = 207;

    // factory unit types
    public const int FACTORY_UNIT_ENTITY_TYPE_WORKER = 301;
    public const int FACTORY_UNIT_ENTITY_TYPE_PROBE = 302;
    public const int FACTORY_UNIT_ENTITY_TYPE_SYSTEM_EXPANSION_SHIP = 303;


    // WORKERS/TASKS

    public const int WORKER_TASK_TYPE_BUILD = 1;
    public const int WORKER_TASK_TYPE_REMOVE = 2;
    public const float MAX_WORKER_TO_WORKER_TASK_DISTANCE = 100f;


}
