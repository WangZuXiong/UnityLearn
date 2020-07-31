using System;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationManager
{
    public enum Module : byte
    {
        Common,
    }


    private static Dictionary<Module, Dictionary<string, string>> _languageDict = new Dictionary<Module, Dictionary<string, string>>();

    private static readonly string BASTEPATH = "Language/";


    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    public static string GetString(Module module, string key)
    {
        if (!_languageDict.ContainsKey(module))
        {
            LoadLanguage(module);
        }

        if (!_languageDict[module].ContainsKey(key))
        {
            //Debug.LogError("不存在 module：" + module + " key:" + key);
            return string.Empty;
        }

        return _languageDict[module][key];
    }


    public static string GetString(string key)
    {
        return GetString(Module.Common, key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="module">模块名</param>
    /// <param name="objs">参数列表</param>
    /// <returns></returns>
    public static string GetStringFormat(Module module, string key, params object[] objs)
    {
        string format = GetString(module, key);
        return string.Format(format, objs);
    }

    /// <summary>
    /// 添加语言文案
    /// </summary>
    /// <param name="module">模块</param>
    private static void LoadLanguage(Module module)
    {
        string path = BASTEPATH + module.ToString() + "/" + "ChineseSimplified";
        TextAsset textAsset = null;
        if (textAsset == null)
        {
            Debug.LogError("确实语言配置文件:" + path);
            return;
        }
        LanguageList languageList = JsonUtility.FromJson<LanguageList>(textAsset.ToString());

        Dictionary<string, string> dict = new Dictionary<string, string>();
        for (int i = 0; i < languageList.languages.Count; i++)
        {
            string key = languageList.languages[i].key;
            string value = languageList.languages[i].value;
            dict.Add(key, value);
        }
        _languageDict.Add(module, dict);
    }

    public static void Clear()
    {
        _languageDict.Clear();
    }


    [Serializable]
    public class LanguageList
    {
        public List<LanguageItem> languages;
    }

    [Serializable]
    public class LanguageItem
    {
        public string key;
        public string value;
    }

}