using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIView
{
    public UIView(int viewId, GameObject viewRoot, UIMgrBehaviour behaviour)
    {
        ViewId = viewId;
        ViewRoot = viewRoot;
        Behaviour = behaviour;
    }


    public int ViewId { get; private set; }

    public GameObject ViewRoot { get; private set; }

    public UIMgrBehaviour Behaviour { get; private set; }


    public virtual void OnControllerInit() { }

    public virtual void OnValueInit0() { }

    public virtual void OnRegControllerEvent() { }

    public virtual void OnValueInit1() { }

    public virtual void OnRemove()
    {
        ViewId = -1;
        GameObject.Destroy(ViewRoot);
        ViewRoot = null;
        Behaviour = null;
    }

    //update
    public bool IsUpdate { get; private set; }

    public virtual void StartUpdate()
    {
        IsUpdate = true;
    }

    public virtual void StopUpdate()
    {
        IsUpdate = false;
    }

    public virtual void OnUpdate(float deltaTime) { }

    //display
    public virtual bool IsCanShow(object data)
    {
        return true;
    }

    public virtual void OnShowBefore() { }

    public virtual void OnShow(object data)
    {
        if (ViewRoot != null && !ViewRoot.activeInHierarchy)
        {
            ViewRoot.SetActive(true);
        }
    }

    public virtual bool IsCanHide(object data)
    {
        return false;
    }

    public virtual void OnHideBefore() { }

    public virtual void OnHide(object data)
    {
        if (ViewRoot != null && ViewRoot.activeInHierarchy)
        {
            ViewRoot.SetActive(false);
        }
    }


    //repeat
    public virtual bool IsRepeat()
    {
        return false;
    }


    //lifecycle
    public virtual void OnStart()
    {

    }


    public T FindComponent<T>(string path)
    {
        return ViewRoot.transform.Find(path).GetComponent<T>();
    }

}
