using System;
using System.Collections.Generic;

//用于数据交换



[Serializable]
public class TeamData
{
    public string PlayerName;
    public int Id;
}

[Serializable]
public class CityData
{
    public string PlayerName;
    public int Id;
}

[Serializable]
public class Msg
{
    public int MsgType;
    public List<byte> Body;
}

[Serializable]
public class TeamNCity
{
    public TeamData TeamData;
    public CityData CityData;
}