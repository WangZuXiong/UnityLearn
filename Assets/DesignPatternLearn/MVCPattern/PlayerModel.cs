using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 委托模型（当信息变化时候调用）
/// </summary>
/// <param name="playerModel"></param>
public delegate void OnValueChangeHandle(PlayerModel playerModel);

public class PlayerModel
{
    public PlayerData PlayerData { get; set; }
    public event OnValueChangeHandle OnValueChange;

    public void UpdateData()
    {
        if (PlayerData.Exp / 500 > PlayerData.Level)
        {
            PlayerData.Level = PlayerData.Exp / 500;
            PlayerData.Exp = 0;
        }
        OnValueChange?.Invoke(this);
    }
}
