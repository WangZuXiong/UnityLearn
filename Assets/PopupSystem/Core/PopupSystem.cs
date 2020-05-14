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

    private Dictionary<int, (GameObject, Image, Button)> _parentDict = new Dictionary<int, (GameObject, Image, Button)>();
    private Dictionary<int, BaseWindowController> _popupDict = new Dictionary<int, BaseWindowController>();
    private RawImage _rawImg;
    private RenderTexture _renderTexture;
    private Camera _camera;

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
            var parent = transform.Find("Level_" + i.ToString());
            var img = parent.GetComponent<Image>();
            var btn = parent.GetComponent<Button>();
            _parentDict.Add(i, (parent.gameObject, img, btn));
        }
        _rawImg = transform.Find("RawImage").GetComponent<RawImage>();
        _camera = Camera.main;
    }

    private void OnDestroy()
    {
        _parentDict.Clear();
        _popupDict.Clear();
        ClearStaticBg();
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

        _parentDict[_index].Item1.Show();
        _parentDict[_index].Item2.color = basePopup.UseMask ? basePopup.MaskColor : Color.clear;
        _parentDict[_index].Item3.onClick.RemoveAllListeners();
        if (basePopup.CloseOnClickMask)
        {
            _parentDict[_index].Item3.onClick.AddListener(() =>
            {
                CloseCurrentPopup();
            });
        }

        if (basePopup.UseStaticBg && _renderTexture == null)
        {
            CreateStaticBg();
        }

        result.transform.SetParent(_parentDict[_index].Item1.transform);
        _popupDict.Add(_index++, result);
        return result;
    }

    public void CloseCurrentPopup()
    {
        if (ToppingPopup != null)
        {
            if (ToppingPopup.UseStaticBg)
            {
                ClearStaticBg();
            }
            Destroy(ToppingPopup.gameObject);
            var temp = _index - 1;
            _parentDict[temp].Item1.Hide();
            _popupDict.Remove(temp);
            _index--;
            Resources.UnloadUnusedAssets();
        }
    }

    private void CreateStaticBg()
    {
        //_renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        //_renderTexture.filterMode = FilterMode.Bilinear;
        //RenderTexture.active = _renderTexture;
        //_camera.targetTexture = _renderTexture;
        //_camera.Render();
        //_rawImg.texture = _renderTexture;
        //_rawImg.gameObject.Show();
        //RenderTexture.active = null;
        //_camera.targetTexture = null;



        _renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
        _renderTexture.filterMode = FilterMode.Bilinear;
        RenderTexture.active = _renderTexture;
        _camera.targetTexture = _renderTexture;
        _camera.Render();
        _rawImg.texture = _renderTexture;
        _rawImg.gameObject.Show();
        RenderTexture.active = null;
        _camera.targetTexture = null;
    }

    private void ClearStaticBg()
    {
        //RenderTexture.GetTemporary这个api要和RenderTexture.ReleaseTemporary 配套使用否则会内存泄漏      
        //RenderTexture.ReleaseTemporary(_renderTexture);

        if (_renderTexture != null)
        {
            _renderTexture.Release();
            _renderTexture = null;
        }
        _renderTexture = null;
        _rawImg.texture = null;
        _rawImg.gameObject.Hide();
    }

    public void CloseAllPopup()
    {
        if (_renderTexture != null)
        {
            ClearStaticBg();
        }
        foreach (var item in _popupDict)
        {
            Destroy(item.Value.gameObject);
        }
        _popupDict.Clear();
        foreach (var item in _parentDict)
        {
            item.Value.Item1.Hide();
        }
        _index = 0;
        Resources.UnloadUnusedAssets();
    }
}
