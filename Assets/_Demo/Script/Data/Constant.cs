public struct PlayerNameConstant
{
    public const string PlayerA = "left";
    public const string PlayerB = "right";
    public const string PlayerNPC = "NPC";

}

public enum Operation
{
    /// <summary>
    /// Team 移动到 City
    /// </summary>
    TeamMoveToCity,
    /// <summary>
    /// 选中Team
    /// </summary>
    OnPointerUpTeam,
    /// <summary>
    /// 不选中Team
    /// </summary>
    OnPointerDownTeam,
    /// <summary>
    /// 播放CD动画
    /// </summary>
    PlaysCDAnimation,
    /// <summary>
    /// 单个player  播放CD动画
    /// </summary>
    PlayCDAnimation,
    /// <summary>
    /// Team 分数变化
    /// </summary>
    UpdateTeamScore,    
    /// <summary>
    /// 扣除精力
    /// </summary>
    ReduceEnergy,
    /// <summary>
    /// 扣除血量
    /// </summary>
    UpdateCityBlood,
    /// <summary>
    /// Player 回收 Team
    /// </summary>
    PlayerRecoveryTeam
}