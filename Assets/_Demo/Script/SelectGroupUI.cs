using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGroupUI : MonoBehaviour
{
    public Toggle Tog0;
    public Toggle Tog1;
    public Button BtnComfirm;
    public GameObject CanvasGame;

    public void OnBtnComfirmClick()
    {
        if (Tog0.isOn)
        {
            GameManager.OurPlayerId = 0;
            GameManager.EnemyPlayerId = 1;
            gameObject.Hide();
            CanvasGame.Show();
            GameManager.Instance.StartUpdateCoroutine();
        }
        else if (Tog1.isOn)
        {
            GameManager.OurPlayerId = 1;
            GameManager.EnemyPlayerId = 0;
            gameObject.Hide();
            CanvasGame.Show();
            GameManager.Instance.StartUpdateCoroutine();
        }
    }
}
