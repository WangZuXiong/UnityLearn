using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoWindowController : BaseWindowController
{
    protected override void Awake()
    {
        base.Awake();
        ClearBeforeOpenWindow = false;
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
