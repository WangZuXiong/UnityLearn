using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListWindowController : BaseWindowController
{
    private Transform _content;
    protected Transform _headContent;

    protected override void Awake()
    {
        base.Awake();
        var original = UISystem.GetUICompontent(RawUICompontent.ScrollView);
        var scrollView = Instantiate(original, transform);
        _content = scrollView.GetComponent<ScrollRect>().content;
        _headContent = transform.Find("ScrollView(Clone)/HeadContent");
    }


    public void SetData<T>(IEnumerable<T> collection, GameObject headOriginal, Action<GameObject> HeadAction, GameObject itemOriginal, Action<GameObject, int> itemAction)
    {
        var head = Instantiate(headOriginal, _headContent);
        HeadAction.Execute(head);

        var index = 0;
        foreach (var item in collection)
        {
            var temp = Instantiate(itemOriginal, _content);
            itemAction.Execute(temp, index++);
        }
    }
}
