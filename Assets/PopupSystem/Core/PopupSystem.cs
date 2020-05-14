using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour
{
    [SerializeField]
    private int _index = 0;
    [SerializeField]
    private int _popupMaxCount = 3;
    private Dictionary<int, Transform> _parentDict = new Dictionary<int, Transform>();

    private Dictionary<int, BaseWindowController> _popupDict = new Dictionary<int, BaseWindowController>();

    private PopupSystem() { }

    public static PopupSystem Instance { get; private set; }

    /// <summary>
    /// 当前置顶的弹窗
    /// </summary>
    public BaseWindowController ToppingPopup
    {
        get
        {
            if (_popupDict.TryGetValue(_index - 1, out BaseWindowController baseWindowController))
            {
                return baseWindowController;
            }

            return null;
        }
    }

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < _popupMaxCount; i++)
        {
            _parentDict.Add(i, transform.Find("Level_" + i.ToString()));
        }
    }

    private void OnDestroy()
    {
        _parentDict.Clear();
        _popupDict.Clear();
    }

    public T GetPopup<T>(string path) where T : BaseWindowController
    {
        if (_index - 1 > _popupMaxCount)
        {
            throw new Exception("弹窗过多，建议从设计上减负");
        }

        var original = Resources.Load<T>(path);
        if (original == null)
        {
            throw new Exception(string.Format("path::{0} type::{1}", path, typeof(T).ToString()));
        }
        var result = Instantiate(original, transform);
        var basePopup = (BaseWindowController)result;
        if (basePopup.ClearBeforeOpenWindow)
        {
            CloseAllPopup();
        }
        var parent = _parentDict[_index];
        parent.gameObject.Show();
        parent.GetComponent<Image>().color = basePopup.UseMask ? basePopup.MaskColor : Color.clear;
        parent.GetComponent<Button>().onClick.RemoveAllListeners();
        if (basePopup.CloseOnClickMask)
        {
            parent.GetComponent<Button>().onClick.AddListener(() =>
            {
                CloseCurrentPopup();
            });
        }
        result.transform.SetParent(parent);
        _popupDict.Add(_index++, result);
        return result;
    }

    public void CloseCurrentPopup()
    {
        if (ToppingPopup != null)
        {
            Destroy(ToppingPopup.gameObject);
            var temp = _index - 1;
            _parentDict[temp].gameObject.Hide();
            _popupDict.Remove(temp);
            _index--;
            Resources.UnloadUnusedAssets();
        }
    }

    public void CloseAllPopup()
    {
        foreach (var item in _popupDict)
        {
            Destroy(item.Value.gameObject);
        }
        _popupDict.Clear();
        foreach (var item in _parentDict)
        {
            item.Value.gameObject.Hide();
        }
        _index = 0;
        Resources.UnloadUnusedAssets();
    }
}
