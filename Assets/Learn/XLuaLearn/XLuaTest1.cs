using System.Collections.Generic;
using UnityEngine;

public class XLuaTest1 : MonoBehaviour
{
    private readonly Dictionary<int, string> _dict = new Dictionary<int, string>();

    private void Awake()
    {
        _dict.Add(1, "A");
        _dict.Add(2, "B");
        _dict.Add(3, "C");
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //重新load
            LuaService.Instance.ReLoadLuaScript();
            Foo();
            Foo1(999);

            //Foo(1);
            //Foo("1");
        }
    }

    private void Foo()
    {
        Debug.LogError("Foo C#");
    }

    private void Foo1(int t)
    {
        Debug.LogError(t.ToString() + "Foo1 C#");
    }


    //热更重载函数 test

    public void Foo(int x)
    {
        Debug.Log(x);
    }


    public void Foo(string x)
    {
        Debug.Log(x);
    }
}
