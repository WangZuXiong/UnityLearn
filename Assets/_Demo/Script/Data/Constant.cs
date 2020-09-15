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
    /// PK
    /// </summary>
    TeamPK,
    /// <summary>
    /// 扣除精力
    /// </summary>
    ReduceEnergy
}