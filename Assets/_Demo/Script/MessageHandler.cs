using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler
{
    internal static void Func(string msgStr)
    {
        TeamMoveToCity msg = JsonUtility.FromJson<TeamMoveToCity>(msgStr);

        switch (msg.MsgType)
        {
            case 0:
                var body = msg;
                var team = GameData.PlayerDict[body.TeamData.PlayerId].TeamDict[body.TeamData.Id];
                var city = GameData.PlayerDict[body.TeamData.PlayerId].CityDict[body.CityData.Id];

                team.transform.SetParent(city.TeamContent);
                city.Add(team);
                break;
        }
    }
}
