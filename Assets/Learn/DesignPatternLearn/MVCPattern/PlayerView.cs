using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{

    private Button _buttonAddExp;
    private Text _textLevel;
    private Text _textExp;

    private void Start()
    {
        _buttonAddExp = this.transform.Find("ButtonAddExp").GetComponent<Button>();
        _buttonAddExp.onClick.AddListener(OnButtonAddExpClick);
        _textLevel = this.transform.Find("TextLevel").GetComponent<Text>();
        _textExp = this.transform.Find("TextExp").GetComponent<Text>();
    }

    /// <summary>
    /// 更新数据的方法
    /// </summary>
    /// <param name="playerModel">需要更新的数据模型</param>
    public void UpdateInfo(PlayerModel playerModel)
    {
        _textLevel.text = "LV:" + playerModel.PlayerData.Level;
        _textExp.text = "EXP:" + playerModel.PlayerData.Exp;
    }

    //增加经验值
    private void OnButtonAddExpClick()
    {
        PlayerController.Instance.CaculateInfo();
    }

}
