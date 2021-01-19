using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemTest : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private void Awake()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.BeginDrag
        };
        entry.callback.AddListener((data) => { OnBeginDrag1((PointerEventData)data); });
        trigger.triggers.Add(entry);


        EventTrigger.Entry entry1 = new EventTrigger.Entry
        {
            eventID = EventTriggerType.EndDrag
        };
        entry1.callback.AddListener((data) => { OnEndDrag1((PointerEventData)data); });
        trigger.triggers.Add(entry1);
    }

    private void OnEndDrag1(PointerEventData data)
    {
        Debug.LogError("OnEndDrag >>> EventTriggerType.EndDrag");
    }

    private void OnBeginDrag1(PointerEventData data)
    {
        Debug.LogError("OnBeginDrag >>>  EventTriggerType.BeginDrag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogError("OnBeginDrag >>> IBeginDragHandler");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.LogError("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogError("OnEndDrag >>> IEndDragHandler");
    }





}
