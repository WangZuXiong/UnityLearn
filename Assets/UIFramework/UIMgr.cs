using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr
{
    private Dictionary<int, UIView> _uiViewDict;
    private Dictionary<int, UIView> _visibleUIViewDict;

    public UIMgr()
    {
        _uiViewDict = new Dictionary<int, UIView>();
        _visibleUIViewDict = new Dictionary<int, UIView>();
    }

    public void OnUpdate(float deltaTime)
    {
        foreach (var item in _visibleUIViewDict)
        {
            item.Value.OnUpdate(deltaTime);
        }
    }

    public bool IsShow(int id)
    {
        return _visibleUIViewDict.ContainsKey(id);
    }

    public bool IsHide(int id)
    {
        return !_visibleUIViewDict.ContainsKey(id);
    }

    public void ShowUI(int id, object data)
    {
        if (_uiViewDict.TryGetValue(id, out UIView view))
        {
            if (view.IsCanShow(data))
            {
                if (!IsShow(id))
                {
                    _visibleUIViewDict.Add(id, view);
                    view.OnShowBefore();
                    view.OnShow(data);
                }
                else if (IsShow(id) && view.IsRepeat())
                {
                    view.OnShowBefore();
                    view.OnShow(data);
                }
            }
        }
    }

    public void HideUI(int id, object data)
    {
        if (IsHide(id))
        {
            return;
        }
        if (_uiViewDict.TryGetValue(id, out UIView view))
        {
            if (view.IsCanHide(data))
            {
                _visibleUIViewDict.Remove(id);
                view.OnHideBefore();
                view.OnHide(data);
            }
        }
    }

    public void RegUIView(int id, UIView view)
    {
        if (!_uiViewDict.ContainsKey(id))
        {
            _uiViewDict.Add(id, view);
            view.OnControllerInit();
            view.OnValueInit0();
            view.OnRegControllerEvent();
            view.OnValueInit1();
        }
    }

    public void UnregUIView(int id)
    {
        if (_uiViewDict.TryGetValue(id, out UIView view))
        {
            _uiViewDict.Remove(id);
            if (IsShow(id))
            {
                _visibleUIViewDict.Remove(id);
            }
            view.OnRemove();
        }
    }

    public UIView GetUIView(int id)
    {
        if (_uiViewDict.TryGetValue(id, out UIView view))
        {
            return view;
        }
        return null;
    }

    public GameObject CreateUIViewObj(string uiPrefabPath, Transform parent, bool active = false)
    {
        GameObject viewObj = ResMgr.Instantiation(uiPrefabPath, parent);
        if (viewObj.transform is RectTransform)
        {
            var rtrans = viewObj.transform as RectTransform;
            rtrans.offsetMin = Vector2.zero;
            rtrans.offsetMax = Vector2.zero;
            rtrans.anchorMin = Vector2.zero;
            rtrans.anchorMax = Vector2.one;
        }
        viewObj.SetActive(active);
        return viewObj;
    }

    public void OnCleanUp()
    {
        foreach (var item in _uiViewDict)
        {
            item.Value.OnRemove();
        }
        _uiViewDict.Clear();
        _visibleUIViewDict.Clear();
    }
}
