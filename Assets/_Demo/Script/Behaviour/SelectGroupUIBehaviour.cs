using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGroupUIBehaviour : MonoBehaviour
{
    public Toggle Tog0;
    public Toggle Tog1;
    public Button BtnComfirm;
    public GameObject CanvasGame;

    public void OnBtnComfirmClick()
    {
        if (Tog0.isOn)
        {
            GameData.OurPlayerId = 0;
            GameData.EnemyPlayerId = 1;
            gameObject.Hide();
            CanvasGame.Show();
            GameManager.Instance.StartUpdateCoroutine();
        }
        else if (Tog1.isOn)
        {
            GameData.OurPlayerId = 1;
            GameData.EnemyPlayerId = 0;
            gameObject.Hide();
            CanvasGame.Show();
            GameManager.Instance.StartUpdateCoroutine();
        }
    }
}
