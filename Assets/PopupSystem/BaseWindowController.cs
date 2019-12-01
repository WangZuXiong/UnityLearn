using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindowController : MonoBehaviour
{
    protected virtual void Awake()
    {
        var btnClose = GetBtnClose();
        if (btnClose != null)
        {
            btnClose.onClick.AddListener(OnBtnClose);
        }
    }

    protected virtual void OnBtnClose()
    {
        PopupSystem.Instance.CloseCurrentPop();
    }

    private Button GetBtnClose()
    {
        return transform.Find("BtnClose").GetComponent<Button>();
    }
}