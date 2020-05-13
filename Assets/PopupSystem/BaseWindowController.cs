using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindowController : MonoBehaviour
{
    [Tooltip("勾选后，将使用遮罩背景")]
    public bool UseMask = true;
    [Tooltip("勾选后，点击背景遮罩也能关闭弹窗")]
    public bool CloseOnClickMask = true;
    [Tooltip("勾选后，打开改弹窗的前会关闭所有弹窗")]
    public bool ClearBeforeOpenWindow = true;

    protected virtual void Awake()
    {
        var btnClose = GetBtnClose();
        if (btnClose != null)
        {
            btnClose.onClick.AddListener(OnBtnClose);
        }
    }

    protected virtual void OnEnable()
    {
        //tween
    }

    protected virtual void OnBtnClose()
    {
        //tween
        PopupSystem.Instance.CloseCurrentPop();
    }

    private Button GetBtnClose()
    {
        return transform.Find("BtnClose").GetComponent<Button>();
    }
}