using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public static Config Config;
    public static Dictionary<string, Player> PlayerDict = new Dictionary<string, Player>();
    public static string OurPlayerName = PlayerNameConstant.PlayerA;
    public static string EnemyPlayerName = PlayerNameConstant.PlayerB;
}


