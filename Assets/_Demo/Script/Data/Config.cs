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
    /// Player Winner加分
    /// </summary>
    public int WinScore;
    /// <summary>
    /// Player Loser扣分
    /// </summary>
    public int LoseScore;
    /// <summary>
    /// NPC的球队输了之后的返场时间
    /// </summary>
    public int NPCTeamReturnCity;
    /// <summary>
    /// Team的总体能
    /// </summary>
    public int TeamEnergy;
    /// <summary>
    /// Team Winner扣除的体能
    /// </summary>
    public int WinEnergy;
    /// <summary>
    /// Team Loser扣除的体能
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
    /// Main City 总血量
    /// </summary>
    public int CityTotalBlood;
    /// <summary>
    /// Main City 遭受攻击时扣除血量
    /// </summary>
    public int CityBlood;
    /// <summary>
    /// 进攻对方Main City获得的积分
    /// </summary>
    public int MainCityScore;
    /// <summary>
    /// 进攻对方Main City扣除的体能
    /// </summary>
    public int MainCityEnergy;
    /// <summary>
    /// Main City 被攻击的CD
    /// </summary>
    public int MainCityAttackCD;

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