using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPanelController : BasePanelController
{
    private Transform _content;
    protected Transform _headContent;
    protected ScrollRect _scrollRect;
    private GameObject _itemOriginal;
    private Action<GameObject, int> _itemAction;

    protected override void Awake()
    {
        base.Awake();
        var original = UISystem.GetUICompontent(RawUICompontent.ScrollView);
        var scrollView = Instantiate(original, transform);
        _scrollRect = scrollView.GetComponent<ScrollRect>();
        _content = _scrollRect.content;
        _headContent = transform.Find("ScrollView(Clone)/HeadContent");
    }

    private void OnDestroy()
    {
        ClearList();
        _itemOriginal = null;
        _itemAction = null;
    }

    public void SetData<T>(IEnumerable<T> collection, GameObject headOriginal, Action<GameObject> HeadAction, GameObject itemOriginal, Action<GameObject, int> itemAction)
    {
        _itemOriginal = itemOriginal;
        _itemAction = itemAction;
        var head = Instantiate(headOriginal, _headContent);
        HeadAction.Execute(head);

        var index = 0;
        foreach (var item in collection)
        {
            var temp = Instantiate(itemOriginal, _content);
            itemAction.Execute(temp, index++);
        }
    }

    public void UpdateList<T>(IEnumerable<T> collection)
    {
        if (_headContent == null || _itemAction == null)
        {
            throw new Exception("先调用SetData()");
        }
        ClearList();
        var index = 0;
        foreach (var item in collection)
        {
            var temp = Instantiate(_itemOriginal, _content);
            _itemAction.Execute(temp, index++);
        }
    }

    private void ClearList()
    {
        for (int i = 0; i < _content.childCount; i++)
        {
            Destroy(_content.GetChild(i));
        }
    }
}
