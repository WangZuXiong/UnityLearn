using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    public static GameConfig GameConfig;
    public Player A;
    public Player B;
    public Player NPC;

    public static int OurPlayerId;
    public static int EnemyPlayerId;

    public Transform GameMap;


    public Dictionary<int, Player> PlayerDict = new Dictionary<int, Player>();

    private void Awake()
    {
        Instance = this;
        Application.runInBackground = true;
    }

    private void Start()
    {
        // load Config
        var path = Path.Combine(Application.persistentDataPath, "Config.json");
        if (!File.Exists(path))
        {
            File.WriteAllText(path, Resources.Load<TextAsset>("Config").text);
        }
        var json = File.ReadAllText(path);

#if UNITY_EDITOR
        json = Resources.Load<TextAsset>("Config").text;
#endif

        GameConfig = JsonUtility.FromJson<GameConfig>(json);




        //Init
        PlayerDict.Add(0, A);
        PlayerDict.Add(1, B);

        var teamA = new List<City>();
        var teamB = new List<City>();
        var teamNPC = new List<City>();


        var citys = GameMap.GetComponentsInChildren<City>();
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

            if (citys[i].Player == A)
            {
                teamA.Add(citys[i]);
            }
            else if (citys[i].Player == B)
            {
                teamB.Add(citys[i]);
            }
            else
            {
                teamNPC.Add(citys[i]);
            }
        }
        A.Init(GameConfig.PlayerA, teamA);
        B.Init(GameConfig.PlayerB, teamB);
        NPC.Init(GameConfig.NPC, teamNPC);


    }

    internal static (Team winner, Team loser) TeamPK(Team team1, Team team2)
    {
        var total = team1.TeamConfig.FightingCapacity + team2.TeamConfig.FightingCapacity;

        var t = UnityEngine.Random.Range(0, total + 1);

        if (t < team1.TeamConfig.FightingCapacity)
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


    public void StartUpdateCoroutine()
    {
        SimpleCoroutineManager.Instance.StartCoroutine(UpdateCoroutine());
    }


    private IEnumerator UpdateCoroutine()
    {
        var wfs = new WaitForSeconds(GameConfig.ReqSpacing);

        while (true)
        {
            ReqGetOperations(EnemyPlayerId.ToString());
            yield return wfs;
        }
    }


    private void ReqGetOperations(string groupName)
    {

        var getOperationUri = string.Format(CMD.GetOperations, groupName);

        WebRequestManager.GetRequest(getOperationUri, (msgStrs) =>
        {
            //try
            //{


            //["{\"MsgType\":0,\"Body\":{\"TeamData\":{\"PlayerId\":1,\"Id\":1},\"CityData\":{\"PlayerId\":0,\"Id\":1}}}"]


            //Debug.Log(msgStrs);
            //"{\"MsgType\":0,\"Body\":{\"TeamData\":{\"PlayerId\":1,\"Id\":1},\"CityData\":{\"PlayerId\":0,\"Id\":1}}}"
            //{"MsgType":0,"Body":{"TeamData":{"PlayerId":1,"Id":1},"CityData":{"PlayerId":0,"Id":1}}}
            //"{\"MsgType\":0,\"Body\":{\"TeamData\":{\"PlayerId\":1,\"Id\":1},\"CityData\":{\"PlayerId\":0,\"Id\":1}}}"
            //"{\"MsgType\":0,\"Body\":{\"TeamData\":{\"PlayerId\":1,\"Id\":1},\"CityData\":{\"PlayerId\":0,\"Id\":1}}}"
            //msgStrs = "{\"MsgType\":0,\"Body\":{\"TeamData\":{\"PlayerId\":1,\"Id\":1},\"CityData\":{\"PlayerId\":0,\"Id\":1}}}";

            Debug.Log(msgStrs);


            var oldValue = new string(new char[] { '\"', ',', '\"' });
            var newValue = new string(new char[] { '\"', '@', '\"' });

            msgStrs = msgStrs.Replace(oldValue, newValue);
            msgStrs = msgStrs.Replace("[", string.Empty).Replace("]", string.Empty);






            var msgs = msgStrs.Split('@');


            for (int i = 0; i < msgs.Length; i++)
            {
                var msgStr = msgs[i];

                if (string.IsNullOrEmpty(msgStr))
                {
                    return;
                }


                msgStr = msgStr.Remove(0, 1);
                msgStr = msgStr.Remove(msgStr.Length - 1, 1);
                msgStr = msgStr.Replace("\\", "");


                Debug.Log(msgStr);

                //{\"MsgType\":0,\"Body\":{\"TeamData\":{\"PlayerId\":1,\"Id\":1},\"CityData\":{\"PlayerId\":0,\"Id\":1}}}
                //{"MsgType":0,"Body":{"TeamData":{"PlayerId":1,"Id":1},"CityData":{"PlayerId":0,"Id":1}}}
                Msg msg = JsonUtility.FromJson<Msg>(msgStr);

                switch (msg.MsgType)
                {
                    case 0:
                        var body = msg.Body;
                        var team = PlayerDict[body.TeamData.PlayerId].TeamDict[body.TeamData.Id];
                        var city = PlayerDict[body.CityData.PlayerId].CityDict[body.CityData.Id];

                        team.transform.SetParent(city.TeamContent);
                        city.Add(team);
                        break;
                }
            }

            //}
            //    catch
            //{

            //}



        }, null);
    }


    private void AddOperation(string groupName, string dataStr)
    {
        var uri = string.Format(CMD.AddOperation, groupName, dataStr);
        WebRequestManager.GetRequest(uri, null, null);
    }



    public void SendAddOperation(Msg msg)
    {
        var json = JsonUtility.ToJson(msg);
        AddOperation(OurPlayerId.ToString(), json);
    }




    string GetTestMsgStr()
    {
        TeamMoveToCity teamMoveToCity = new TeamMoveToCity
        {
            CityData = new CityData() { Id = 1, PlayerId = 0 },
            TeamData = new TeamData() { Id = 1, PlayerId = 1 }
        };
        Msg msg = new Msg
        {
            MsgType = 0,
            Body = teamMoveToCity
        };


        var json = JsonUtility.ToJson(msg);

        return json;
    }



    [ContextMenu("Gen Json")]
    public void GenJson()
    {
        var json = GetTestMsgStr();


        //SendAddOperation(msg);


        Debug.Log(json);
        Msg msg = JsonUtility.FromJson<Msg>(json);

        switch (msg.MsgType)
        {
            case 0:
                var body = msg.Body;
                var team = PlayerDict[body.TeamData.PlayerId].TeamDict[body.TeamData.Id];
                var city = PlayerDict[body.CityData.PlayerId].CityDict[body.CityData.Id];
                Debug.Log("body.CityData.Id:" + body.CityData.Id);
                //team.transform.SetParent(city.TeamContent);
                //city.Add(team);
                break;
        }


        AddOperation(OurPlayerId.ToString(), json);
        AddOperation(OurPlayerId.ToString(), json);


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

