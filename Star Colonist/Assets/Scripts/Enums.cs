using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    //EDITOR
    public enum OptionalToggle
    {
        Ignore, Yes, No
    }

    //GAME OPTIONS
    public enum GameState
    {
        Debug, Load, Generation, Play, Build, Battle, GameOver
    }

    //MAP CREATION
    public enum HexEdgeType
    {
        Flat, Slope, Cliff
    }
}
