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
        Player A = transform.Find("CanvasGame/A").GetComponent<Player>();
        Player B = transform.Find("CanvasGame/B").GetComponent<Player>();
        Player NPC = transform.Find("CanvasGame/NPC").GetComponent<Player>();
        Transform GameMap = transform.Find("CanvasGame/Content");

        GameData.PlayerDict.Add(0, A);
        GameData.PlayerDict.Add(1, B);

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
                citys[i].InitBlood();
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
        A.SetData(0, GameData.Config.PlayerA, teamA);
        B.SetData(1, GameData.Config.PlayerB, teamB);
        NPC.SetData(2, GameData.Config.NPC, teamNPC);
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
            MessageSender.GetOperations(GameData.EnemyPlayerId.ToString());
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

    [ContextMenu("Gen Json")]
    public void GenJson()
    {
        //var json = GetTestMsgStr();

        //Debug.Log(json);

        //TeamMoveToCity msg = JsonUtility.FromJson<TeamMoveToCity>(json);

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

        TeamMoveToCity teamMoveToCity = new TeamMoveToCity
        {
            MsgType = 0,
            CityData = new CityData() { Id = 1, PlayerId = 0 },
            TeamData = new TeamData() { Id = 1, PlayerId = 1 }
        };


        MessageSender.AddOperation(GameData.OurPlayerId.ToString(), teamMoveToCity);



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

