using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseWindowController : MonoBehaviour
{
    [Tooltip("勾选后，将使用遮罩背景")]
    [SerializeField]
    protected bool _useMask;
    [Tooltip("勾选后，点击背景遮罩也能关闭弹窗")]
    [SerializeField]
    protected bool _closeOnClickMask;
    [Tooltip("勾选后，打开改弹窗的前会关闭所有弹窗")]
    [SerializeField]
    protected bool _clearBeforeOpenWindow;

    protected Transform Parent { get; private set; }

    protected virtual void Awake()
    {

        if (_clearBeforeOpenWindow)
        {
            //some code
        }

        if (_clearBeforeOpenWindow)
        {
            //some code
        }



        var btnClose = GetBtnClose();
        if (btnClose != null)
        {
            btnClose.onClick.AddListener(OnBtnClose);
        }
    }

    public void SetParent(Transform parent)
    {
        Parent = parent;
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