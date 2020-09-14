using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int Id;

    public Transform TeamContent;
    public Team[] Teams;

    public int Score;
    public Text TexScore;


    public City MainCity;
    public Dictionary<int, City> CityDict { private set; get; } = new Dictionary<int, City>();


    public Dictionary<int, Team> TeamDict { private set; get; } = new Dictionary<int, Team>();


    public bool IsNPC;

    private void Awake()
    {
        IsNPC = transform.name.Equals("NPC");
    }

    public void SetData(int id, List<TeamConfig> teamDatas, List<City> cities)
    {
        Id = id;
        InitTexScore();
        Teams = GetComponentsInChildren<Team>();
        for (int i = 0; i < Teams.Length; i++)
        {
            Teams[i].SetData(teamDatas[i], this);
            TeamDict.Add(teamDatas[i].Id, Teams[i]);
        }

        for (int i = 0; i < cities.Count; i++)
        {
            CityDict.Add(i, cities[i]);
            cities[i].SetData(i);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var team = collision.transform.GetComponent<Team>();

        if (team != null && team.Player == this && team.transform.parent != transform)
        {
            collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Recovery(team);
            team.ResetCityTeamContent();
        }
    }


    public void InitTexScore()
    {
        TexScore.text = "Score:" + Score.ToString();
    }

    public void Recovery(Team team)
    {
        team.transform.SetParent(TeamContent);
        if (team.City != null)
        {
            team.City.Teams.Remove(team);
            team.City.InitTexCount();
        }
    }
}


