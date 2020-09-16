using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public CityData CityData;

    public int Capacity;
    public int Blood;

    public bool IsMainCity;


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

            if (!team.Player.IsNPC)
            {
                MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                {
                    CityData = CityData,
                    TeamData = team.TeamData
                });
            }

            collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            team.transform.localScale = Vector3.one;
            team.ResetPlayerTeamContent();


            if (IsMainCity && team.Player != Player)
            {
                //扣血
                Blood -= GameData.Config.CityBlood;
                InitBlood();

                MessageSender.AddOperation(Operation.UpdateCityBlood, new CityNFloat()
                {
                    CityData = CityData,
                    F = Blood
                });

                //回去
                team.Player.MainCity.Add(team);

                MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                {
                    CityData = team.Player.MainCity.CityData,
                    TeamData = team.TeamData
                });


                team.Player.Score += GameData.Config.MainCityScore;
                team.Player.InitTexScore();
                MessageSender.AddOperation(Operation.UpdateTeamScore, new TeamNFloat()
                {
                    TeamData = team.TeamData,
                    F = team.Player.Score
                });

                team.ReduceEnergy(GameData.Config.MainCityEnergy);
                MessageSender.AddOperation(Operation.ReduceEnergy, new TeamNFloat()
                {
                    TeamData = team.TeamData,
                    F = GameData.Config.MainCityEnergy
                });

                if (team.Energy <= 0)
                {
                    team.Player.Recovery(team);
                    MessageSender.AddOperation(Operation.PlayerRecoveryTeam, team.TeamData);
                }
            }
        }
    }

    public void SetData(int id, int totalBlood)
    {

        Blood = totalBlood;
        CityData = new CityData
        {
            PlayerName = Player.PlayerName,
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
    }


    public void InitTexCount()
    {
        TexCount.text = string.Format("{0}/{1}", Teams.Count.ToString(), Capacity.ToString());
    }

    internal void InitBlood()
    {
        TexBlood.text = string.Format("{0}/{1}", Blood.ToString(), GameData.Config.CityTotalBlood.ToString());
        SliderBlood.value = (float)Blood / GameData.Config.CityTotalBlood;
    }
}
