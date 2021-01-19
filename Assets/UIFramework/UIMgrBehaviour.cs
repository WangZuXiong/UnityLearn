using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIMgrBehaviour : MonoBehaviour
{
    public UIMgr UIMgr { get; private set; }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        UIMgr.OnUpdate(Time.deltaTime);
    }

    private void OnDestroy()
    {
        
    }

    public virtual void OnStart()
    {
       
    }

    public void Init()
    {
        UIMgr = new UIMgr();
    }

    protected void RegUIView<T>(int viewId, string viewResource, GameObject root, bool inactive = false, bool isTrackingLifeCycle = true) where T : UIView
    {
        GameObject viewRoot = UIMgr.CreateUIViewObj(viewResource, root.transform, inactive);
        //UIView view = new UIView(viewId,viewRoot,this);//无法创建抽象的类型的实例
        UIView view = System.Activator.CreateInstance(typeof(T), viewId, viewRoot, this) as UIView;
        if (isTrackingLifeCycle)
        {
            var lifeCycle = viewRoot.AddComponent<LifecycleBehaviour>();
            lifeCycle.View = view;
        }
        UIMgr.RegUIView(viewId, view);
    }
}
