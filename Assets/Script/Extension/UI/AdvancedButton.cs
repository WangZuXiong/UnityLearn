using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedButton : Button
{
    private float _lastClickTime;
 
    public float _spand = 0.5f;

    public override void OnPointerClick(PointerEventData eventData)
    {
        //禁止频繁点击。默认500ms下只能点击1次，超过的点击事件会忽略。
        var current = Time.realtimeSinceStartup;
        if (current - _lastClickTime < _spand)
        {
            Debug.LogError("单击");
            base.OnPointerClick(eventData);
        }
        else
        {
            Debug.LogError("双击");
        }
        _lastClickTime = Time.realtimeSinceStartup;
    }
}