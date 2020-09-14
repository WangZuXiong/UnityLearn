using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public CityData CityData;

    public int Capacity;
    public int Blood = 10;

    public bool IsMainCity;
    public bool IsNeutral;


    public Player Player;
    public List<Team> Teams = new List<Team>();


    public Team NPCTeam;
    public Text TexCount;
    public Text TexBlood;
    public Slider SliderBlood;
    public Transform TeamContent;

    private void Awake()
    {
        IsMainCity = transform.name.Equals("MainCity");
        IsNeutral = transform.name.Equals("NeutralCity");
        InitTexCount();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Teams.Count >= Capacity)
        {
            return;
        }

        var team = collision.transform.GetComponent<Team>();
        if (team != null && team.transform.parent != transform)
        {
            Add(team);

            collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            team.ResetPlayerTeamContent();


            if (IsMainCity && team.Player != Player)
            {
                //扣血
                Blood--;
                InitBlood();

                //回去
                team.Player.MainCity.Add(team);
            }
        }
    }

    public void SetData(int id)
    {
        CityData = new CityData
        {
            PlayerId = Player.Id,
            Id = id
        };
    }


    public void Add(Team team)
    {
        if (team.City != null)
        {
            team.City.Teams.Remove(team);
            team.City.InitTexCount();
        }

        team.City = this;
        team.ResideTime = 0;
        team.transform.SetParent(TeamContent);
        Teams.Add(team);
        InitTexCount();

        if (team.Player.IsNPC)
        {
            NPCTeam = team;
        }

        MessageSender.AddOperation(GameData.OurPlayerId.ToString(), new TeamMoveToCity()
        {
            CityData = CityData,
            TeamData = team.TeamData
        });
    }


    public void InitTexCount()
    {
        TexCount.text = string.Format("{0}/{1}", Teams.Count.ToString(), Capacity.ToString());
    }

    internal void InitBlood()
    {
        TexBlood.text = string.Format("{0}/{1}", Blood.ToString(), GameData.Config.MainCityTotalBlood.ToString());
        SliderBlood.value = (float)Blood / GameData.Config.MainCityTotalBlood;
    }
}
