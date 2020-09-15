using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Application.runInBackground = true;
    }

    private void Start()
    {
        InitConfig();
        InitData();
    }

    private void InitConfig()
    {
        var path = Path.Combine(Application.persistentDataPath, "Config.json");
        if (!File.Exists(path))
        {
            File.WriteAllText(path, Resources.Load<TextAsset>("Config").text);
        }
        var json = File.ReadAllText(path);

#if UNITY_EDITOR
        json = Resources.Load<TextAsset>("Config").text;
#endif

        GameData.Config = JsonUtility.FromJson<Config>(json);
    }

    private void InitData()
    {
        Player A = GameObject.Find("CanvasGame/A").GetComponent<Player>();
        Player B = GameObject.Find("CanvasGame/B").GetComponent<Player>();
        Player NPC = GameObject.Find("CanvasGame/NPC").GetComponent<Player>();
        GameData.PlayerDict.Clear();
        GameData.PlayerDict.Add(PlayerNameConstant.PlayerA, A);
        GameData.PlayerDict.Add(PlayerNameConstant.PlayerB, B);
        GameData.PlayerDict.Add(PlayerNameConstant.PlayerNPC, NPC);

        var teamACities = new List<City>();
        var teamBCities = new List<City>();
        var teamNPCCities = new List<City>();

        Transform GameMap = GameObject.Find("CanvasGame").transform;
        var citys = GameMap.GetComponentsInChildren<City>();

        for (int i = 0; i < citys.Length; i++)
        {
            if (citys[i].Player == A)
            {
                teamACities.Add(citys[i]);
            }
            else if (citys[i].Player == B)
            {
                teamBCities.Add(citys[i]);
            }
            else if (citys[i].Player == NPC)
            {
                teamNPCCities.Add(citys[i]);
            }
        }


        A.SetData(PlayerNameConstant.PlayerA, GameData.Config.ATeamConfig, teamACities);
        B.SetData(PlayerNameConstant.PlayerB, GameData.Config.BTeamConfig, teamBCities);
        NPC.SetData(PlayerNameConstant.PlayerNPC, GameData.Config.NPCTeamConfig, teamNPCCities);


        //将NPC的Team移入中立city
        var tempIndex = 0;
        foreach (var item in NPC.CityDict)
        {
            var city = item.Value;
            if (city.IsMainCity)
            {
                continue;
            }
            city.Add(NPC.TeamDict[++tempIndex]);
        }

        //初始化mian city的血量
        A.MainCity.InitBlood();
        B.MainCity.InitBlood();

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

    public void StartUpdateCoroutine()
    {
        SimpleCoroutineManager.Instance.StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        var wfs = new WaitForSeconds(GameData.Config.ReqSpacing);

        while (true)
        {
            MessageSender.GetOperations(GameData.EnemyPlayerName);
            //MessageSender.GetOperations(GameData.OurPlayerName);
            yield return wfs;
        }
    }

    //string GetTestMsgStr()
    //{
    //    TeamMoveToCity teamMoveToCity = new TeamMoveToCity
    //    {
    //        MsgType = 0,
    //        CityData = new CityData() { Id = 1, PlayerId = 0 },
    //        TeamData = new TeamData() { Id = 1, PlayerId = 1 }
    //    };


    //    var json = JsonUtility.ToJson(teamMoveToCity);

    //    return json;
    //}


    //{"MsgType":2,"Body":[123,34,80,108,97,121,101,114,78,97,109,101,34,58,34,108,101,102,116,34,44,34,73,100,34,58,50,125]}
    //public string str;
    [ContextMenu("Gen Json")]
    public void GenJson()
    {

        //var msg = JsonUtility.FromJson<Msg>(str);

        //Debug.Log(msg.MsgType);
        //var body = System.Text.Encoding.UTF8.GetString(msg.Body.ToArray());
        //var t = JsonUtility.FromJson<TeamNCity>(body);
        //Debug.Log(t.CityData.PlayerName);



        //switch (msg.MsgType)
        //{
        //    case 0:
        //        var body = msg;
        //        var team = GameData.PlayerDict[body.TeamData.PlayerId].TeamDict[body.TeamData.Id];
        //        var city = GameData.PlayerDict[body.CityData.PlayerId].CityDict[body.CityData.Id];

        //        team.transform.SetParent(city.TeamContent);
        //        city.Add(team);
        //        break;
        //}


        //BaseMsg baseMsg = new BaseMsg();

        //TeamMoveToCity teamMoveToCity = new TeamMoveToCity
        //{
        //    CityData = new CityData() { Id = 1, PlayerId = 0 },
        //    TeamData = new TeamData() { Id = 1, PlayerId = 1 }
        //};

        //baseMsg.MsgType = 0;
        //baseMsg.Body = JsonUtility.ToJson(teamMoveToCity);
        //var msgStr = JsonUtility.ToJson(baseMsg);

        //MessageSender.AddOperation(GameData.OurPlayerId.ToString(), msgStr);



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

