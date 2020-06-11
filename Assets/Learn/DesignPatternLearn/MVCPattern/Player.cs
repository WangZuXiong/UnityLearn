using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData PlayerData { get; set; }

    public Player()
    {
        PlayerData = new PlayerData();
        PlayerData.Name = "wang";
        PlayerData.Level = 1;
        PlayerData.Exp = 100;
    }
}
