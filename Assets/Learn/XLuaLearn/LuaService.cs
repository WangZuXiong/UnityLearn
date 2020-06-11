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
        LoadLuaScript();
    }

    private void OnDisable()
    {
        _luaEnv.DoString("require 'LuaDispose'");
    }

    private void OnDestroy()
    {
        _luaEnv.Dispose();
    }

    public void LoadLuaScript()
    {
        //if (_luaEnv == null)
        {
            _luaEnv = new LuaEnv();
        }
        //_luaEnv.ClearLoadList();
        _luaEnv.AddLoader(MyLoader);
        _luaEnv.DoString("require 'LuaScript'");
    }

    private byte[] MyLoader(ref string fileName)
    {
        string path = Application.dataPath + "/XLuaLearn/LuaScript/" + fileName + ".lua";

        Debug.Log("path:" + path);
        if (File.Exists(path))
        {
            return Encoding.UTF8.GetBytes(File.ReadAllText(path));
        }
        return null;
    }
}
