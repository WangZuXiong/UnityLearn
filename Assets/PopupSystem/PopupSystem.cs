using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopupSystem : MonoBehaviour
{
    [SerializeField]
    private int _count = 0;
    [SerializeField]
    private Transform _mask;
    /// <summary>
    /// 当前置顶的弹窗
    /// </summary>
    public BaseWindowController CurrentWindow
    {
        get
        {
            if (_count <= 0)
            {
                return null;
            }

            return transform.GetChild(_count).GetComponent<BaseWindowController>();
        }
    }

    private PopupSystem() { }

    public static PopupSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _mask.gameObject.SetActive(false);
    }

    public T Pop<T>(string path) where T : BaseWindowController
    {
        var original = Resources.Load<T>(path);
        var result = Instantiate(original, transform);
        _count++;
        _mask.SetSiblingIndex(_count - 1);
        _mask.gameObject.SetActive(true);

        var controller = (BaseWindowController)result.GetComponent<T>();
        if (controller.ClearBeforeOpenWindow)
        {
            //some code
        }

        if (controller.CloseOnClickMask)
        {
            //some code
        }

        return result;
    }

    public void CloseCurrentPop()
    {
        if (_count <= 0)
        {
            return;
        }

        var currentIndex = _count;
        Destroy(transform.GetChild(currentIndex).gameObject);
        _count--;
        _mask.gameObject.SetActive(_count > 0);
        if (_mask.gameObject.activeSelf)
        {
            _mask.SetSiblingIndex(_count - 1);
        }
    }

    public void CloseAllPop()
    {
        if (_count <= 0)
        {
            return;
        }

        _mask.SetAsFirstSibling();
        for (int i = 1; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
            _count--;
        }
        _mask.gameObject.SetActive(false);
    }
}
