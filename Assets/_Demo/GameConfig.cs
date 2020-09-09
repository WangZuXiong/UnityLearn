using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameConfig
{
    /// <summary>
    /// team的进攻冷却时间
    /// </summary>
    public float AttackCD;
    /// <summary>
    /// Team和Team PK耗时
    /// </summary>
    public float TeamPKDuration;
    /// <summary>
    /// 战力上限
    /// </summary>
    public int MaxFightingCapacity;
    /// <summary>
    /// 赢了+几分
    /// </summary>
    public int WinScore;
    /// <summary>
    /// 
    /// </summary>
    public int LoseScore;
    /// <summary>
    /// NPC的球队输了之后的返场时间
    /// </summary>
    public int NPCTeamReturnCity;


    public List<TeamData> PlayerA;

    public List<TeamData> PlayerB;

    public List<TeamData> NPC;
}

[Serializable]
public struct TeamData
{
    public int Id;
    public int FightingCapacity;
}