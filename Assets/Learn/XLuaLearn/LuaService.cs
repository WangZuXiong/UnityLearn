using System;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;
[Hotfix]
public class LuaService : MonoBehaviour
{
    private LuaEnv _luaEnv;
    public static LuaService Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _luaEnv = new LuaEnv();
        _luaEnv.AddLoader(Loader);
        _luaEnv.DoString("require 'LuaScript'");
    }
 
    private void OnDisable()
    {
        //_luaEnv.DoString("require 'LuaDispose'");
    }

    private void OnDestroy()
    {
        _luaEnv.Dispose();
    }

    /// <summary>
    /// 仅调试是调用
    /// </summary>
    internal void ReLoadLuaScript()
    {
        Start();
    }

    private byte[] Loader(ref string filepath)
    {
        string path = Application.dataPath + "/XLuaLearn/LuaScript/" + filepath + ".lua";
        if (File.Exists(path))
        {
            return Encoding.UTF8.GetBytes(File.ReadAllText(path));
        }
        Debug.LogError("path:" + path);
        return null;
    }
}
