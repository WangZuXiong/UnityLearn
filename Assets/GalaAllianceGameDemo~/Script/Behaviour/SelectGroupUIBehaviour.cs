using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGroupUIBehaviour : MonoBehaviour
{
    public Toggle Tog0;
    public Toggle Tog1;
    public Button BtnComfirm;
    public GameUIBehaviour GameUI;

    public void OnBtnComfirmClick()
    {
        if (Tog0.isOn)
        {
            GameData.OurPlayerName = PlayerNameConstant.PlayerA;
            GameData.EnemyPlayerName = PlayerNameConstant.PlayerB;
            gameObject.Hide();
            GameUI.Show();
            GameManager.Instance.StartUpdateCoroutine();
        }
        else if (Tog1.isOn)
        {
            GameData.OurPlayerName = PlayerNameConstant.PlayerB;
            GameData.EnemyPlayerName = PlayerNameConstant.PlayerA;
            gameObject.Hide();
            GameUI.Show();
            GameManager.Instance.StartUpdateCoroutine();
        }
    }
}
