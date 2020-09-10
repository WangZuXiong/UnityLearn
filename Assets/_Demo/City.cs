using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public int Capacity;
    public int Blood = 10;
    public Player Player;
    public Text TexCount;
    public Text TexBlood;
    public Slider SliderBlood;
    //public Button BtnAdd;
    //public Button BtnReduce;
    public Transform TeamContent;

    public List<Team> Teams = new List<Team>();

    public Team NPCTeam;

    public bool IsMainCity;
    public bool IsNeutral;


    private void Awake()
    {
        IsMainCity = transform.name.Equals("MainCity");
        IsNeutral = transform.name.Equals("NeutralCity");
        InitTexCount();
        //BtnAdd.onClick.AddListener(OnBtnAddClick);
        //BtnReduce.onClick.AddListener(OnBtnReduceClick);
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

    public void Init()
    {
        InitBlood();
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


    void InitBlood()
    {
        TexBlood.text = string.Format("{0}/{1}", Blood.ToString(), GameManager.GameConfig.MainCityTotalBlood.ToString());
        SliderBlood.value = (float)Blood / GameManager.GameConfig.MainCityTotalBlood;
    }

    //private void OnBtnReduceClick()
    //{
    //    if (Count > 0)
    //    {
    //        Player.TotalCount++;
    //        Player.TotalText.text = "Total:" + Player.TotalCount.ToString();

    //        Count--;
    //        TexCount.text = Count.ToString();
    //    }
    //}

    //private void OnBtnAddClick()
    //{
    //    if (Player.TotalCount > 0)
    //    {
    //        Player.TotalCount--;
    //        Player.TotalText.text = "Total:" + Player.TotalCount.ToString();

    //        Count++;
    //        TexCount.text = Count.ToString();
    //    }
    //}



    //public Team WinnerTeam;



    //public void OnNPCTeamReturnCityCallback()
    //{
    //    if (WinnerTeam != null)
    //    {
    //        //winner 返回主球馆
    //        WinnerTeam.Player.MainCity.Add(WinnerTeam);
    //    }

    //    if (NPCTeam != null)
    //    {

    //        //loser 返场
    //        Add(NPCTeam);
    //    }
    //}
}
