using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{


    public const float CAMERA_SIZE_MIN = 10f;
    public const float CAMERA_SIZE_MAX = 1000f;
    public const float CAMERA_ZOOM_SPEED = 3f;
    public const float CAMERA_MOVE_SPEED = 4f;


    public const int PLAYER_INPUT_MODE_INIT = 1;
    public const int PLAYER_INPUT_MODE_PLACEMENT = 2;
    public const int PLAYER_INPUT_MODE_STRUCTURE_SELECT = 3;
    public const int PLAYER_INPUT_MODE_STRUCTURE_IO = 4;
    public const int PLAYER_INPUT_MODE_STRUCTURE_IO_SELECT = 5;

    public const KeyCode PLAYER_INPUT_STRUCTURE_IO_MODE_KEY = KeyCode.Space;
    public const KeyCode PLAYER_INPUT_CYCLE_IO_SELECT = KeyCode.Tab;
    public const KeyCode PLAYER_INPUT_STRUCTURE_REMOVAL_KEY = KeyCode.D;
    public const KeyCode PLAYER_INPUT_REVERT_MODE_KEY = KeyCode.Q;
    public const KeyCode PLAYER_INPUT_QUIT_GAME_KEY = KeyCode.Escape;


    public const int GALAXY_SIZE = 3000;

    public const int PLANETARY_SYSTEMS_COUNT = 200;
    public const int PLANETARY_SYSTEMS_MIN_PLANETS = 0;
    public const int PLANETARY_SYSTEMS_MAX_PLANETS = 8;

    public const int STAR_MIN_SIZE_RADIUS = 1;
    public const int STAR_MAX_SIZE_RADIUS = 4;

    public const float PLANET_MIN_SIZE_RADIUS = 0.25f;
    public const float PLANET_MAX_SIZE_RADIUS = 1f;
    public const int PLANET_MIN_ORBIT_RADIUS = 5;
    public const int PLANET_MAX_ORBIT_RADIUS = 30;
    public const int PLANET_MIN_ORBIT_SPEED = 1;
    public const int PLANET_MAX_ORBIT_SPEED = 10;

    public const int RESOURCE_TYPE_WATER = 1;
    public const int RESOURCE_TYPE_GAS = 2;
    public const int RESOURCE_TYPE_ROCK = 3;
    public const int RESOURCE_TYPE_METAL = 4;
    public const int RESOURCE_TYPE_ORGANIC = 5;

    public const int STRUCTURE_TYPE_HARVESTER = 1;
    public const int STRUCTURE_TYPE_STORAGE = 2;
    public const int STRUCTURE_TYPE_SPLITTER = 3;
    public const int STRUCTURE_TYPE_MERGER = 4;
    public const int STRUCTURE_TYPE_MIRROR = 5;
    public const int STRUCTURE_TYPE_PHOTOVOLTAIC = 6;
    public const int STRUCTURE_TYPE_ACCUMULATOR = 7;


}
