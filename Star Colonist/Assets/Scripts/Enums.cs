using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    //GAME OPTIONS
    public enum GameState
    {
        Debug, Load, Generation, Play, Build, Battle, GameOver
    }

    public enum HexEdgeType
    {
        Flat, Slope, Cliff
    }
}
