using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoWindowController : BaseWindowController
{
    protected override void AddEvent()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void RemoveEvent()
    {
        //throw new System.NotImplementedException();
    }

    void Start()
    {
        Debug.LogError("demo window");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
