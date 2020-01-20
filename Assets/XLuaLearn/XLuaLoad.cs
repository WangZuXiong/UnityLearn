using System.Collections.Generic;
using UnityEngine;

public class XLuaLoad : MonoBehaviour
{
    private Dictionary<int, List<string>> _dict = new Dictionary<int, List<string>>();

    private void Awake()
    {
        _dict.Add(1, new List<string>());
        _dict[1].Add("a");
        _dict[1].Add("b");
    }

    private void Start()
    {
        Debug.LogError(_dict[1][0]);
        Debug.LogError(_dict[1][1]);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //重新load
            LuaService.Instance.LoadLuaScript();
            Play();
            Play1();
        }
    }

   

    private void Play()
    {
        Debug.Log("do lua ");
        // do lua
    }

    private void Play1()
    {
        Debug.Log("do lua ");
        // do lua
    }

    private void Func()
    {
        Debug.LogError("this message is from Chsharp");
    }
}
