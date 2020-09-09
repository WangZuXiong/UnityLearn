using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    public TeamData TeamData;

    public float CD;
    public Image ImgCD;

    public Player Player;
    public City City;



    public Text TexName;
    public Slider SliderAttack;


    public bool isIn;
    public bool IsCanOperation = true;

    /// <summary>
    /// 驻留在City上面的时间
    /// </summary>
    public int ResideTime;

    private IEnumerator CurrentRoutine;


    public void SetData(TeamData teamData, Player player)
    {
        TeamData = teamData;
        transform.name = TexName.text = "T." + TeamData.Id.ToString();
        SliderAttack.value = (float)TeamData.FightingCapacity / GameManager.GameConfig.MaxFightingCapacity;
        Player = player;
    }

    public void PlayCDAnimation(Action callback, float total)
    {
        if (CurrentRoutine != null)
        {
            StopCoroutine(CurrentRoutine);
        }
        CD = total;
        CurrentRoutine = CDAnimationCoroutine(callback, total);
        StartCoroutine(CurrentRoutine);
    }

    private IEnumerator CDAnimationCoroutine(Action callback, float total)
    {
        var wfs = new WaitForSeconds(0.1f);
        while (CD > 0)
        {
            yield return wfs;
            CD -= 0.1f;
            ImgCD.fillAmount = CD / total;
        }
        callback?.Invoke();
    }


    internal void ResetCityTeamContent()
    {
        if (City != null)
        {
            City.TeamContent.GetComponent<ContentSizeFitter>().enabled = true;
            City.TeamContent.GetComponent<GridLayoutGroup>().enabled = true;
        }
    }

    internal void ResetPlayerTeamContent()
    {
        if (Player != null)
        {
            Player.TeamContent.GetComponent<ContentSizeFitter>().enabled = true;
            Player.TeamContent.GetComponent<GridLayoutGroup>().enabled = true;
        }
    }

    void ResetContent(bool enable)
    {
        if (transform.GetComponentInParent<Player>() != null)
        {
            Player.TeamContent.GetComponent<ContentSizeFitter>().enabled = enable;
            Player.TeamContent.GetComponent<GridLayoutGroup>().enabled = enable;
        }
        else
        {
            City.TeamContent.GetComponent<ContentSizeFitter>().enabled = enable;
            City.TeamContent.GetComponent<GridLayoutGroup>().enabled = enable;
        }
    }

    public void OnPointerDown()
    {
        if (!IsCanOperation || CD > 0)
        {
            return;
        }

        isIn = true;

        ResetContent(false);
    }

    public void OnPointerUp()
    {
        if (!IsCanOperation || CD > 0)
        {
            return;
        }


        isIn = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        if (City != null)
        {
            ResideTime += 1;
        }

        if (!IsCanOperation || CD > 0)
        {
            return;
        }

        if (isIn)
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
            else if (Input.GetMouseButton(0))
            {
                transform.position = Input.mousePosition;
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(DelayResetContent());
            }
        }
        transform.localEulerAngles = Vector3.zero;
    }


    IEnumerator DelayResetContent()
    {
        yield return new WaitForSeconds(0.1f);
        ResetContent(true);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CD > 0)
        {
            return;
        }

        var team = collision.transform.GetComponent<Team>();
        if (team == null)
        {
            return;
        }
        //驻留时间较长的就是被攻击的  
        var t = ResideTime > team.ResideTime;

        if (t)
        {
            //当前是被攻击的team

            //父节点的City容量够不够？
            if (City.Teams.Count < City.Capacity)
            {
                if (team.transform.parent != City.TeamContent.transform)
                {
                    City.Add(team);
                    collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    team.ResetPlayerTeamContent();
                }

                //来之不是同一个player的话就进行PK
                if (team.Player != Player)
                {
                    //进入战斗CD状态
                    team.PlayCDAnimation(() =>
                    {
                        //PK
                        var (winner, loser) = GameManager.TeamPK(this, team);
                        GameManager.InitScore(winner, loser);
                        //赢了的留下，输了的回到主球场
                        var original = loser.City;
                        loser.Player.MainCity.Add(loser);
                        //进入战斗结束冷却状态
                        loser.PlayCDAnimation(null, GameManager.GameConfig.AttackCD);
                        winner.PlayCDAnimation(() =>
                        {
                            // 不论输赢，这个City是中立场的话，NPC回到主球场之后5min 这支球队将会返会原来的City
                            if (loser.Player.IsNPC)
                            {
                                loser.PlayCDAnimation(() =>
                                {
                                    //winner 返回主球馆
                                    winner.Player.MainCity.Add(winner);
                                    //loser 返场
                                    original.Add(loser);
                                }, GameManager.GameConfig.NPCTeamReturnCity);
                            }
                        }, GameManager.GameConfig.AttackCD);
                    }, GameManager.GameConfig.TeamPKDuration);

                    PlayCDAnimation(null, GameManager.GameConfig.TeamPKDuration);
                }
            }
        }
    }
}
