using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstPlayerInput
{


    // PLAYER INPUT

    // player input modes
    public const int MODE_INIT = 1;
    public const int MODE_PLACEMENT = 2;
    public const int MODE_FACTORY_ENTITY_SELECT = 3;
    public const int MODE_STRUCTURE_IO = 4;
    public const int MODE_STRUCTURE_IO_SELECT = 5;
    public const int MODE_FACTORY_ENTITY_MULTISELECT = 6;
    public const int MODE_MULTI_STRUCTURE_IO = 7;

    // player input key mappings
    public const KeyCode STRUCTURE_IO_MODE_KEY = KeyCode.E;
    public const KeyCode CYCLE_IO_SELECT_KEY = KeyCode.Tab;
    public const KeyCode REMOVAL_KEY = KeyCode.D;
    public const KeyCode CANCEL_REMOVAL_KEY = KeyCode.C;
    public const KeyCode REVERT_MODE_KEY = KeyCode.Q;
    public const KeyCode SMALL_ZOOM_KEY = KeyCode.LeftControl;
    public const KeyCode LARGE_ZOOM_KEY = KeyCode.LeftShift;
    public const KeyCode ADDITIVE_SELECTION_KEY = KeyCode.LeftShift;
    public const KeyCode QUIT_GAME_KEY = KeyCode.Escape;
    public const KeyCode ADMIN_MODE_TOGGLE_KEY = KeyCode.A;
    public const KeyCode ADMIN_CREATE_WORKER_KEY = KeyCode.W;
    public const KeyCode ADMIN_POPULATE_STORAGE_ALL_KEY = KeyCode.P;
    public const KeyCode ADMIN_POPULATE_STORAGE_RESOURCES_KEY = KeyCode.O;


}
