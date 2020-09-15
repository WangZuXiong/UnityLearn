using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team : MonoBehaviour
{
    public TeamConfig TeamConfig;
    public TeamData TeamData;

    public float CD;
    public int Energy;
    public Image ImgCD;

    public Color Color;

    public Player Player;
    public City City;

    public Text TexName;
    public Slider SliderAttack;
    public Slider SliderEnergy;


    public GameObject Mask;


    public bool isIn;
    public bool IsCanOperation = true;

    /// <summary>
    /// 操作中
    /// </summary>
    public bool IsInOperation = false;

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

    public void SetData(TeamConfig config, Player player)
    {
        TeamConfig = config;
        transform.name = TexName.text = "T." + TeamConfig.Id.ToString();
        SliderAttack.value = (float)TeamConfig.FightingCapacity / GameData.Config.MaxFightingCapacity;
        Player = player;
        Energy = GameData.Config.TeamEnergy;
        SliderEnergy.value = (float)Energy / GameData.Config.TeamEnergy;


        TeamData = new TeamData
        {
            Id = config.Id,
            PlayerName = player.PlayerName
        };
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

        if (!IsCanOperation || CD > 0 || Energy <= 0 || IsInOperation || !Player.PlayerName.Equals(GameData.OurPlayerName))
        {
            return;
        }

        isIn = true;

        ResetContent(false);


        MessageSender.AddOperation(Operation.OnPointerDownTeam, TeamData);
        Mask.Show();
    }

    public void OnPointerUp()
    {
        if (!IsCanOperation || CD > 0 || Energy <= 0)
        {
            return;
        }

        isIn = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


        MessageSender.AddOperation(Operation.OnPointerUpTeam, TeamData);
        Mask.Hide();
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

    public void ReduceEnergy()
    {
        Energy--;
        SliderEnergy.value = (float)Energy / GameData.Config.TeamEnergy;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInOperation)
        {
            return;
        }


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

                    if (!team.Player.IsNPC)
                    {
                        MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                        {
                            CityData = City.CityData,
                            TeamData = team.TeamData
                        });
                    }
                    team.ResetPlayerTeamContent();
                }

                //来之不是同一个player的话就进行PK
                if (team.Player != Player)
                {
                    //进入战斗CD状态
                    team.PlayCDAnimation(OnTeamPKDurationFinsh, GameData.Config.TeamPKDuration);
                    PlayCDAnimation(null, GameData.Config.TeamPKDuration);

                    MessageSender.AddOperation(Operation.PlaysCDAnimation, new TwoTeamNFloat()
                    {
                        A = new TeamNFloat()
                        {
                            TeamData = team.TeamData,
                            F = GameData.Config.TeamPKDuration
                        },
                        B = new TeamNFloat()
                        {
                            TeamData = TeamData,
                            F = GameData.Config.TeamPKDuration
                        }
                    });

                    //战斗结束之后的回调
                    void OnTeamPKDurationFinsh()
                    {
                        //PK
                        (Team winner, Team loser) result = GameManager.TeamPK(this, team);

                        Team winner = result.winner;
                        Team loser = result.loser;

                        GameUIBehaviour.InitScore(winner, loser);
                        var pkCity = winner.City;


                        MessageSender.AddOperation(Operation.TeamPK, new TwoTeamData()
                        {
                            ATeamData = winner.TeamData,
                            BTeamData = loser.TeamData
                        });


                        //在非主城的情况下赢了的留下
                        if (loser.City.IsMainCity)
                        {
                            //回去
                            winner.Player.MainCity.Add(winner);

                            MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                            {
                                CityData = winner.Player.MainCity.CityData,
                                TeamData = winner.TeamData
                            });
                        }

                        //输了的回到主球场
                        loser.Player.MainCity.Add(loser);
                        MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                        {
                            CityData = loser.Player.MainCity.CityData,
                            TeamData = loser.TeamData
                        });

                        //扣精力
                        if (!winner.Player.IsNPC)
                        {
                            winner.ReduceEnergy();
                            MessageSender.AddOperation(Operation.ReduceEnergy, winner.TeamData);
                        }

                        if (!loser.Player.IsNPC)
                        {
                            loser.ReduceEnergy();
                            MessageSender.AddOperation(Operation.ReduceEnergy, loser.TeamData);
                        }

                        if (loser.Energy > 0)
                        {
                            //进入战斗结束冷却状态
                            loser.PlayCDAnimation(null, GameData.Config.AttackCD);

                            MessageSender.AddOperation(Operation.PlayCDAnimation, new TeamNFloat()
                            {
                                TeamData = loser.TeamData,
                                F = GameData.Config.AttackCD
                            });
                        }
                        else
                        {
                            loser.Player.Recovery(loser);
                        }

                        if (winner.Energy > 0)
                        {
                            //进入战斗结束冷却状态
                            winner.PlayCDAnimation(null, GameData.Config.AttackCD);

                            MessageSender.AddOperation(Operation.PlayCDAnimation, new TeamNFloat()
                            {
                                TeamData = winner.TeamData,
                                F = GameData.Config.AttackCD
                            });
                        }
                        else
                        {
                            winner.Player.Recovery(winner);
                        }

                        //进攻冷却时间结束之后的回调
                        void OnAttackCDFinish()
                        {
                            if (pkCity.Player.IsNPC)
                            {
                                if (pkCity.NPCTeam.City != pkCity)
                                {
                                    pkCity.NPCTeam.PlayCDAnimation(null, GameData.Config.NPCTeamReturnCity);

                                    MessageSender.AddOperation(Operation.PlayCDAnimation, new TeamNFloat()
                                    {
                                        TeamData = pkCity.NPCTeam.TeamData,
                                        F = GameData.Config.NPCTeamReturnCity
                                    });
                                }

                                GameUtil.Instance.Delay(pkCity, () =>
                                {
                                    //winner 返回主球馆
                                    winner.Player.MainCity.Add(winner);
                                    //npc team 返场
                                    pkCity.Add(pkCity.NPCTeam);


                                    MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                                    {
                                        CityData = winner.Player.MainCity.CityData,
                                        TeamData = winner.TeamData
                                    });

                                    MessageSender.AddOperation(Operation.TeamMoveToCity, new TeamNCity()
                                    {
                                        CityData = pkCity.CityData,
                                        TeamData = pkCity.NPCTeam.TeamData
                                    });


                                }, GameData.Config.NPCTeamReturnCity);
                            }

                        }

                        GameUtil.Instance.Delay(OnAttackCDFinish, GameData.Config.AttackCD);
                    }
                }
            }
        }
    }
}
