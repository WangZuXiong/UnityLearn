using System.Text;
using UnityEngine;

public  class MessageHandler
{
    internal static void HandleMsg(string msgStr)
    {
        var msg = JsonUtility.FromJson<Msg>(msgStr);
        var body = Encoding.UTF8.GetString(msg.Body.ToArray());

        switch ((Operation)msg.MsgType)
        {
            case Operation.TeamMoveToCity: HandleTeamMoveToCity(JsonUtility.FromJson<TeamNCity>(body)); break;
            case Operation.OnPointerUpTeam: HandlePointerUpTeam(JsonUtility.FromJson<TeamData>(body)); break;
            case Operation.OnPointerDownTeam: HandlePointerDownTeam(JsonUtility.FromJson<TeamData>(body)); break;
        }
    }

    static void HandleTeamMoveToCity(TeamNCity data)
    {
        var team = GameData.PlayerDict[data.TeamData.PlayerName].TeamDict[data.TeamData.Id];
        var city = GameData.PlayerDict[data.TeamData.PlayerName].CityDict[data.CityData.Id];
        //var team = GameData.PlayerDict["right"].TeamDict[data.TeamData.Id];
        //var city = GameData.PlayerDict["right"].CityDict[data.CityData.Id];

        team.transform.SetParent(city.TeamContent);
        city.Add(team);
    }

    static void HandlePointerUpTeam(TeamData data)
    {
        //data.PlayerName = "right"; 
        var team = GameData.PlayerDict[data.PlayerName].TeamDict[data.Id];
        team.IsInOperation = false;
        team.Mask.Hide();
    }

    static void HandlePointerDownTeam(TeamData data)
    {
        //data.PlayerName = "right";
        var team = GameData.PlayerDict[data.PlayerName].TeamDict[data.Id];
        team.IsInOperation = true;
        team.Mask.Show();
    }
}
