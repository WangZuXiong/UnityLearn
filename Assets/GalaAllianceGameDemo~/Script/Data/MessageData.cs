using System;
using System.Collections.Generic;

//用于数据交换



[Serializable]
public struct TeamData
{
    public string PlayerName;
    public int Id;
}

[Serializable]
public struct CityData
{
    public string PlayerName;
    public int Id;
}

[Serializable]
public struct Msg
{
    public int MsgType;
    public byte[] Body;
}

[Serializable]
public struct TeamNCity
{
    public TeamData TeamData;
    public CityData CityData;
}


[Serializable]
public struct TwoTeamData
{
    public TeamData ATeamData;
    public TeamData BTeamData;
}

[Serializable]
public struct TwoTeamNFloat
{
    public TeamNFloat A;
    public TeamNFloat B;
}

[Serializable]
public struct TeamNFloat
{
    public TeamData TeamData;
    public float F;
}


[Serializable]
public struct CityNFloat
{
    public CityData CityData;
    public float F;
}