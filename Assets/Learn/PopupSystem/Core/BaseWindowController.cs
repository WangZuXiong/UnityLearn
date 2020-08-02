using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWindowController : MonoBehaviour
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
            _btnClose.onClick.AddListener(OnBtnCloseClick);
        }
    }

    protected virtual void OnBtnCloseClick()
    {
        float tweenDuration = PlayFadeOutTween();
        Invoke("Close", tweenDuration);
    }

    protected virtual void Close()
    {
        PopupSystem.Instance.CloseCurrentPopup();
        Resources.UnloadUnusedAssets();
    }

    protected virtual Button GetBtnClose()
    {
        return transform.Find("BtnClose").GetComponent<Button>();
    }

    /// <summary>
    /// 淡入动画
    /// </summary>
    /// <returns>动画时长 secondsDelay</returns>
    protected virtual float PlayFadeInTween()
    {
        //tween
        return 0;
    }

    /// <summary>
    /// 淡出动画
    /// </summary>
    /// <returns>动画时长 secondsDelay</returns>
    protected virtual float PlayFadeOutTween()
    {
        //tween
        return 0;
    }

    /// <summary>
    /// 注册事件监听
    /// </summary>
    protected abstract void AddEvent();

    /// <summary>
    /// 注销事件监听
    /// </summary>
    protected abstract void RemoveEvent();

}