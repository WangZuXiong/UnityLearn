using System;
using System.Collections.Generic;

[Serializable]
public struct Config
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
    /// <summary>
    /// Team 的体能
    /// </summary>
    public int TeamEnergy;
    /// <summary>
    /// winner扣除的体能
    /// </summary>
    public int WinEnergy;
    /// <summary>
    /// loser扣除的体能
    /// </summary>
    public int LoseEnergy;
    /// <summary>
    /// 请求间隔
    /// </summary>
    public float ReqSpacing;
    /// <summary>
    /// 占领中立场是每间隔InNeutralTime s获得的积分
    /// </summary>
    public int InNeutralScore;
    /// <summary>
    /// 
    /// </summary>
    public int InNeutralTime;
    /// <summary>
    /// City 的总血量
    /// </summary>
    public int CityTotalBlood;
    /// <summary>
    /// City 收到攻击是扣除的血量
    /// </summary>
    public int CityBlood;

    public List<TeamConfig> ATeamConfig;

    public List<TeamConfig> BTeamConfig;

    public List<TeamConfig> NPCTeamConfig;
}




[Serializable]
public struct TeamConfig
{
    public int Id;
    public int FightingCapacity;
}