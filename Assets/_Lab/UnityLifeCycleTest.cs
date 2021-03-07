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
        //即使是Active父节点 还是能回调
        Debug.LogError("OnEnable::" + gameObject.name);
    }

    private void Main()
    {
        Debug.LogError("Main" + gameObject.name);
    }


    private void Start()
    {
        Debug.LogError("Start::" + gameObject.name);

    }

    private void FixedUpdate()
    {
        Debug.LogError("FixedUpdate");
    }

    private void Update()
    {
        Debug.LogError("Update::" + gameObject.name);
    }

    private void LateUpdate()
    {
        Debug.LogError("LateUpdate");
    }

    private void OnGUI()
    {
        Debug.LogError("OnGUI");
    }

    private void OnDisable()
    {
        //即使是Deactive父节点 还是能回调
        Debug.LogError("OnDisable::" + gameObject.name);
    }

    private void OnDestroy()
    {
        //脚本本disable 依旧能被执行
        Debug.LogError("OnDestroy::" + gameObject.name);
    }

}