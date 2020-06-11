using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model和View之间没有任何关系
/// 数据的更新和通知都是Controller完成的
/// </summary>
public class PlayerController : MonoBehaviour
{
    public PlayerModel PlayerModel { get; set; }
    public PlayerView PlayerView { get; set; }
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //初始化两个实例
        PlayerModel = new PlayerModel();
        PlayerView = this.transform.GetComponent<PlayerView>();
        //注册事件
        PlayerModel.OnValueChange += PlayerView.UpdateInfo;
    }

    /// <summary>
    /// 通过计算得到Model
    /// </summary>
    /// <returns></returns>
    public PlayerModel CaculateInfo()
    {
        PlayerModel playerModel = GetPlayerModel();
        //对数据计算
        playerModel.PlayerData.Exp += 100;
        //更新数据
        playerModel.UpdateData();

        Debug.Log("经验值+100");
        return playerModel;
    }


    public PlayerModel GetPlayerModel()
    {
        //模拟从网络请求数据，在这里我们直接使用的是游戏对象本身的信息
        Player player = GameObject.Find("Player").GetComponent<Player>();
        PlayerModel.PlayerData = player.PlayerData;
        return PlayerModel;
    }

    private void OnDestroy()
    {
        PlayerModel.OnValueChange -= PlayerView.UpdateInfo;
    }
}
