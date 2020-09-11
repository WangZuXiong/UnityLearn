using System;

[Serializable]
public struct Msg
{
    public int MsgType;
    public string Body;
}


/// <summary>
/// 0 - Team 移动到 City
/// </summary>
[Serializable]
public struct TeamMoveToCity
{
    public TeamData TeamData;
    public CityData CityData;
}





public struct TeamData
{
    public int PlayerId;
    public int Id;
}

public struct CityData
{
    public int PlayerId;
    public int Id;
}



[Serializable]
public enum Behaviour
{
    TeamMoveToCity
}

public enum Group
{
    Red,
    Blue
}


[Serializable]
public struct TeamConfig
{
    public int Id;
    public int FightingCapacity;
}

public struct CMD
{
    public const string GetOperations = "http://192.168.3.16:6666/allianceC/getOperations?user={0}";
    public const string AddOperation = "http://192.168.3.16:6666/allianceC/addOperation?user={0}&operation={1}";
}