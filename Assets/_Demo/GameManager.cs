using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameConfig GameConfig;

    public static Camera Camera;

    public Player A;
    public Player B;
    public Player NPC;


    private void Awake()
    {
        Camera = Camera.main;
    }

    private IEnumerator Start()
    {




        var t = Resources.LoadAsync<TextAsset>("Config");
        yield return t;
        GameConfig = JsonUtility.FromJson<GameConfig>(t.asset.ToString());

        A.Init(GameConfig.PlayerA);
        B.Init(GameConfig.PlayerB);
        NPC.Init(GameConfig.NPC);

        var citys = transform.Find("Content/Neutral").GetComponentsInChildren<City>();
        for (int i = 0; i < NPC.Teams.Length; i++)
        {
            citys[i].Add(NPC.Teams[i]);
        }
    }


    [ContextMenu("Gen Json")]
    public void GenJson()
    {
        GameConfig = new GameConfig
        {
            AttackCD = 15,
            TeamPKDuration = 5,
            MaxFightingCapacity = 10000,

        };
        GameConfig.PlayerA = new List<TeamData>();
        for (int i = 1; i <= 40; i++)
        {
            GameConfig.PlayerA.Add(new TeamData() { Id = i, FightingCapacity = 1000 }); ;
        }

        GameConfig.PlayerB = GameConfig.PlayerA;
        var json = JsonUtility.ToJson(GameConfig, true);

        Debug.Log(json);




    }

    internal static (Team winner, Team loser) TeamPK(Team team1, Team team2)
    {
        var total = team1.TeamData.FightingCapacity + team2.TeamData.FightingCapacity;

        var t = UnityEngine.Random.Range(0, total + 1);

        if (t < team1.TeamData.FightingCapacity)
        {
            return (team1, team2);
        }

        return (team2, team1);
    }

    public static void InitScore(Team winner, Team loser)
    {
        winner.Player.Score += GameConfig.WinScore;
        winner.Player.InitTexScore();
        loser.Player.Score -= GameConfig.LoseScore;
        loser.Player.InitTexScore();
    }

}
