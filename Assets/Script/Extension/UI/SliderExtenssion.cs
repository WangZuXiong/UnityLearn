using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderExtenssion : Slider
{
    //public bool isChangeValue = true;
    public int State = 2;//默认状态2，按下时候1，松开时候0
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        print("鼠标按下");
        //isChangeValue = false;
        State = 1;
    }
    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        print("松开鼠标");
        //isChangeValue = true;
        State = 0;
    }
}
