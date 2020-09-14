using System;

//用于数据交换



[Serializable]
public struct TeamData
{
    public int PlayerId;
    public int Id;
}

[Serializable]
public struct CityData
{
    public int PlayerId;
    public int Id;
}








[Serializable]
public class BaseMsg
{
    public int MsgType;
}

/// <summary>
/// 0 - Team 移动到 City
/// </summary>
public class TeamMoveToCity : BaseMsg
{
    public TeamData TeamData;
    public CityData CityData;
}

/// <summary>
/// 1 - 选中Team
/// </summary>
public class TeamDataOnPointerUp : BaseMsg
{
    public TeamData TeamData;
}

/// <summary>
/// 2 - 不选中Team
/// </summary>
public class TeamDataOnPointerDown : BaseMsg
{
    public TeamData TeamData;
}