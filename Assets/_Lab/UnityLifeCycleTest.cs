using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public partial class UnityLifeCycleTest : MonoBehaviour
{
    private void Awake()
    {
        //脚本本disable 依旧能被执行
        Debug.LogError("Awake::" + gameObject.name);
    }

    private void OnEnable()
    {
        Debug.LogError("OnEnable::" + gameObject.name);
    }

    private void Main()
    {
        Debug.LogError("Main");
    }


    private void Start()
    {
        Debug.LogError("Start::" + gameObject.name);

    }
    private void Update()
    {
        Debug.LogError("Update::" + gameObject.name);
    }

    private void OnDisable()
    {
        Debug.LogError("OnDisable::" + gameObject.name);
    }

    private void OnDestroy()
    {
        //脚本本disable 依旧能被执行
        Debug.LogError("OnDestroy::" + gameObject.name);
    }

}