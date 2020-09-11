using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    public TeamConfig TeamConfig;

    public float CD;
    public int Energy;
    public Image ImgCD;

    public Color Color;

    public Player Player;
    public City City;



    public Text TexName;
    public Slider SliderAttack;
    public Slider SliderEnergy;


    public bool isIn;
    public bool IsCanOperation = true;

    /// <summary>
    /// 驻留在City上面的时间
    /// </summary>
    /// 

    public int ResideTime;

    private IEnumerator CurrentRoutine;

    private void Awake()
    {
        SliderEnergy.fillRect.GetComponent<Image>().color = Color;
    }

    public void SetData(TeamConfig teamData, Player player)
    {
        TeamConfig = teamData;
        transform.name = TexName.text = "T." + TeamConfig.Id.ToString();
        SliderAttack.value = (float)TeamConfig.FightingCapacity / GameManager.GameConfig.MaxFightingCapacity;
        Player = player;
        Energy = GameManager.GameConfig.TeamEnergy;
        SliderEnergy.value = (float)Energy / GameManager.GameConfig.TeamEnergy;
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
        ResideTime = 0;

        if (!IsCanOperation || CD > 0 || Energy <= 0)
        {
            return;
        }

        isIn = true;

        ResetContent(false);
    }

    public void OnPointerUp()
    {
        if (!IsCanOperation || CD > 0 || Energy <= 0)
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

        if (!IsCanOperation || CD > 0 || Energy <= 0)
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

    public void ReduceBlood()
    {
        Energy--;
        SliderEnergy.value = (float)Energy / GameManager.GameConfig.TeamEnergy;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        var team = collision.transform.GetComponent<Team>();
        if (team == null)
        {
            return;
        }

        team.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        if (CD > 0 || team.CD > 0 || Energy <= 0)
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
                    team.ResetPlayerTeamContent();
                }

                //来之不是同一个player的话就进行PK
                if (team.Player != Player)
                {
                    //进入战斗CD状态
                    team.PlayCDAnimation(OnTeamPKDurationFinsh, GameManager.GameConfig.TeamPKDuration);
                    PlayCDAnimation(null, GameManager.GameConfig.TeamPKDuration);

                    //战斗结束之后的回调
                    void OnTeamPKDurationFinsh()
                    {
                        //PK
                        (Team winner, Team loser) result = GameManager.TeamPK(this, team);

                        Team winner = result.winner;
                        Team loser = result.loser;

                        GameManager.InitScore(winner, loser);
                        var pkCity = winner.City;


                        //var winnwerOriginalCity = winner.City;


                        //在非主城的情况下赢了的留下
                        if (loser.City.IsMainCity)
                        {
                            //回去
                            winner.Player.MainCity.Add(winner);
                        }

                        //输了的回到主球场
                        var loserOriginalCity = loser.City;
                        loser.Player.MainCity.Add(loser);

                        //扣精力
                        if (!winner.Player.IsNPC)
                        {
                            winner.ReduceBlood();
                        }

                        if (!loser.Player.IsNPC)
                        {
                            loser.ReduceBlood();
                        }


                        if (loser.Energy > 0)
                        {
                            //进入战斗结束冷却状态
                            loser.PlayCDAnimation(null, GameManager.GameConfig.AttackCD);
                        }
                        else
                        {
                            loser.Player.Recovery(loser);
                        }

                        if (winner.Energy > 0)
                        {
                            //进入战斗结束冷却状态
                            winner.PlayCDAnimation(null, GameManager.GameConfig.AttackCD);
                        }
                        else
                        {
                            winner.Player.Recovery(winner);
                        }

                        //进攻冷却时间结束之后的回调
                        void OnAttackCDFinish()
                        {
                            //当前PK的City为中立场

                            //开启倒计时


                            //



                            // 不论输赢，这个City是中立场的话，NPC回到主球场之后5min 这支球队将会返会原来的City
                            //if (loser.Player.IsNPC)
                            //{
                            //    loser.PlayCDAnimation(null, GameManager.GameConfig.NPCTeamReturnCity);
                            //    //city 开启倒计时，5min之后，winner回到主城，city回收原来的npc team

                            //    //5分钟之后 NPC返场

                            //    //loserOriginalCity.NPCTeam


                            //    GameUtil.Instance.Delay(loserOriginalCity, () =>
                            //    {
                            //        //winner 返回主球馆
                            //        winner.Player.MainCity.Add(winner);
                            //        //npc team 返场
                            //        loserOriginalCity.Add(loserOriginalCity.NPCTeam);

                            //    }, GameManager.GameConfig.NPCTeamReturnCity);

                            //}



                            if (pkCity.Player.IsNPC)
                            {
                                if (pkCity.NPCTeam.City != pkCity)
                                {
                                    pkCity.NPCTeam.PlayCDAnimation(null, GameManager.GameConfig.NPCTeamReturnCity);
                                }

                                GameUtil.Instance.Delay(pkCity, () =>
                                {
                                    //winner 返回主球馆
                                    winner.Player.MainCity.Add(winner);
                                    //npc team 返场
                                    pkCity.Add(pkCity.NPCTeam);

                                }, GameManager.GameConfig.NPCTeamReturnCity);
                            }

                        }

                        GameUtil.Instance.Delay(OnAttackCDFinish, GameManager.GameConfig.AttackCD);
                    }
                }
            }
        }
    }
}
