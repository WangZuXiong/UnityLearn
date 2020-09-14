using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public static Config Config;
    public static Dictionary<int, Player> PlayerDict = new Dictionary<int, Player>();
    public static int OurPlayerId;
    public static int EnemyPlayerId;
}
