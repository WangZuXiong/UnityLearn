using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public int Capacity;
    public Player Player;
    public Text TexCount;
    public Button BtnAdd;
    public Button BtnReduce;
    public Transform TeamContent;

    public List<Team> Teams = new List<Team>();


    private void Awake()
    {
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
        }
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
    }

    public void InitTexCount()
    {
        TexCount.text = string.Format("{0}/{1}", Teams.Count.ToString(), Capacity.ToString());
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
}
