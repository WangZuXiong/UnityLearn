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
    OnPointerDownTeam
}