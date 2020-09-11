using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameConfig GameConfig;

    //public static Camera Camera;

    public Player A;
    public Player B;
    public Player NPC;


    private void Awake()
    {
        Application.runInBackground = true;
        //Camera = Camera.main;
    }

    private void /*IEnumerator*/ Start()
    {
        var path = Path.Combine(Application.persistentDataPath, "Config.json");

        if (!File.Exists(path))
        {
            File.WriteAllText(path, Resources.Load<TextAsset>("Config").text);
        }

        var json = File.ReadAllText(path);


        //var t = Resources.LoadAsync<TextAsset>("Config");
        //yield return t;
        //GameConfig = JsonUtility.FromJson<GameConfig>(t.asset.ToString());

        GameConfig = JsonUtility.FromJson<GameConfig>(json);


        A.Init(GameConfig.PlayerA);
        B.Init(GameConfig.PlayerB);
        NPC.Init(GameConfig.NPC);

        var citys = GetComponentsInChildren<City>();
        var tempIndex = 0;
        for (int i = 0; i < citys.Length; i++)
        {
            if (citys[i].IsNeutral)
            {
                citys[i].Add(NPC.Teams[tempIndex++]);
            }
            else if (citys[i].IsMainCity)
            {
                citys[i].Init();
            }
        }
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



    public void ResetGame()
    {
        SceneManager.LoadScene("Demo");
    }


    [ContextMenu("Gen Json")]
    public void GenJson()
    {
        var uri = string.Format("http://192.168.3.16:6666/allianceC/addOperation?user={0}&operation={1}", "right", "123");

        WebRequestManager.GetRequest(uri, null, null);




        var getOperationUri = string.Format("http://192.168.3.16:6666/allianceC/getOperations?user={0}", "right");

        WebRequestManager.GetRequest(getOperationUri, null, null);

        //GameConfig = new GameConfig
        //{
        //    AttackCD = 15,
        //    TeamPKDuration = 5,
        //    MaxFightingCapacity = 10000,

        //};
        //GameConfig.PlayerA = new List<TeamData>();
        //for (int i = 1; i <= 40; i++)
        //{
        //    GameConfig.PlayerA.Add(new TeamData() { Id = i, FightingCapacity = 1000 }); ;
        //}

        //GameConfig.PlayerB = GameConfig.PlayerA;
        //var json = JsonUtility.ToJson(GameConfig, true);

        //Debug.Log(json);




    }
}


