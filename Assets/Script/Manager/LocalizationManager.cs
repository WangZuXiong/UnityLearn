using System;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationManager
{
    private static readonly Dictionary<string, string> _languageDict = new Dictionary<string, string>();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public static string GetString(string key)
    {
        if (!_languageDict.TryGetValue(key, out string value))
        {
            return string.Empty;
        }
        return value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="args">参数列表</param>
    /// <returns></returns>
    public static string GetStringFormat(string key, params object[] args)
    {
        string format = GetString(key);
        if (string.IsNullOrEmpty(format))
        {
            return string.Empty;
        }
        return string.Format(format, args);
    }

    /// <summary>
    /// 添加语言文案
    /// </summary>
    /// <param name="languageJson"></param>
    public static void InitLanguageDict(string languageJson)
    {
        _languageDict.Clear();
        LanguageList languageList = JsonUtility.FromJson<LanguageList>(languageJson);

        for (int i = 0; i < languageList.languages.Count; i++)
        {
            string key = languageList.languages[i].key;
            if (!_languageDict.ContainsKey(key))
            {
                string value = languageList.languages[i].value;
                _languageDict.Add(key, value);
            }
            else
            {
                Debug.LogError("重复key：" + key);
            }
        }
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