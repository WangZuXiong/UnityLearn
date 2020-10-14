using System;
using System.Text;
using UnityEngine;

public class MessageHandler
{
    internal static void HandleMsg(string msgStr)
    {
        var msg = JsonUtility.FromJson<Msg>(msgStr);
        var body = Encoding.UTF8.GetString(msg.Body);

        switch ((Operation)msg.MsgType)
        {
            case Operation.TeamMoveToCity: HandleTeamMoveToCity(JsonUtility.FromJson<TeamNCity>(body)); break;
            case Operation.OnPointerUpTeam: HandlePointerUpTeam(JsonUtility.FromJson<TeamData>(body)); break;
            case Operation.OnPointerDownTeam: HandlePointerDownTeam(JsonUtility.FromJson<TeamData>(body)); break;
            case Operation.PlaysCDAnimation: Handle2PlayCDAnimation(JsonUtility.FromJson<TwoTeamNFloat>(body)); break;
            case Operation.PlayCDAnimation: HandlePlayCDAnimation(JsonUtility.FromJson<TeamNFloat>(body)); break;
            case Operation.UpdateTeamScore: HandleUpdateTeamScore(JsonUtility.FromJson<TeamNFloat>(body)); break;
            case Operation.ReduceEnergy: HandleReduceEnergy(JsonUtility.FromJson<TeamNFloat>(body)); break;
            case Operation.UpdateCityBlood: HandleUpdateCityBlood(JsonUtility.FromJson<CityNFloat>(body)); break;
            case Operation.PlayerRecoveryTeam: HandlePlayerRecoveryTeam(JsonUtility.FromJson<TeamData>(body)); break;
            case Operation.OnMainCityBeAttack: HandleMainCityBeAttack(JsonUtility.FromJson<CityNFloat>(body)); break;
        }
    }

    private static void HandleMainCityBeAttack(CityNFloat data)
    {
        var city = GameData.PlayerDict[data.CityData.PlayerName].CityDict[data.CityData.Id];
        city.PlayCDAnimation(null, data.F);
    }

    private static void HandlePlayerRecoveryTeam(TeamData data)
    {
        var player = GameData.PlayerDict[data.PlayerName];
        var team = player.TeamDict[data.Id];
        player.Recovery(team);
    }

    private static void HandleUpdateCityBlood(CityNFloat data)
    {
        var city = GameData.PlayerDict[data.CityData.PlayerName].CityDict[data.CityData.Id];
        city.Blood = (int)data.F;
        city.InitBlood();
    }

    private static void HandleReduceEnergy(TeamNFloat data)
    {
        var team = GameData.PlayerDict[data.TeamData.PlayerName].TeamDict[data.TeamData.Id];
        team.ReduceEnergy((int)data.F);
    }

    private static void HandlePlayCDAnimation(TeamNFloat data)
    {
        var team = GameData.PlayerDict[data.TeamData.PlayerName].TeamDict[data.TeamData.Id];
        team.PlayCDAnimation(null, data.F);
    }

    private static void HandleUpdateTeamScore(TeamNFloat data)
    {
        var team = GameData.PlayerDict[data.TeamData.PlayerName].TeamDict[data.TeamData.Id];
        team.Player.Score = (int)data.F;
        team.Player.InitTexScore();
    }

    private static void Handle2PlayCDAnimation(TwoTeamNFloat data)
    {
        HandlePlayCDAnimation(data.A);
        HandlePlayCDAnimation(data.B);
    }

    static void HandleTeamMoveToCity(TeamNCity data)
    {
        var team = GameData.PlayerDict[data.TeamData.PlayerName].TeamDict[data.TeamData.Id];
        var city = GameData.PlayerDict[data.CityData.PlayerName].CityDict[data.CityData.Id];
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
