using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindowController : MonoBehaviour
{
    [Tooltip("勾选后，将使用遮罩背景")]
    public bool UseMask = true;
    [Tooltip("勾选后，点击背景遮罩也能关闭弹窗")]
    public bool CloseOnClickMask = false;
    [Tooltip("勾选后，打开改弹窗的前会关闭所有弹窗")]
    public bool ClearBeforeOpenWindow = false;
    [Tooltip("勾选后，使用静态背景")]
    public bool UseStaticBg = false;
    [Tooltip("遮罩背景的颜色")]
    public Color MaskColor = Color.white;

    protected Button _btnClose;

    protected virtual void Awake()
    {
        _btnClose = GetBtnClose();
        if (_btnClose != null)
        {
            _btnClose.onClick.AddListener(OnBtnClose);
        }
    }

    private void Start()
    {

    }

    protected virtual void OnEnable()
    {
        //tween
    }

    protected virtual void OnBtnClose()
    {
        //tween
        PopupSystem.Instance.CloseCurrentPopup();
        Resources.UnloadUnusedAssets();
    }

    private Button GetBtnClose()
    {
        return transform.Find("BtnClose").GetComponent<Button>();
    }

}