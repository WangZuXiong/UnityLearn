using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIBehaviour : MonoBehaviour
{
    public static void InitScore(Team winner, Team loser)
    {
        winner.Player.Score += GameData.Config.WinScore;
        winner.Player.InitTexScore();
        loser.Player.Score -= GameData.Config.LoseScore;
        loser.Player.InitTexScore();
    }

    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }
}
