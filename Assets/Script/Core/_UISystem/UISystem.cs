using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UISystem
{
    private static GameObject _originalRoot;


    private static Dictionary<string, GameObject> _rawUICompontentDict = new Dictionary<string, GameObject>();

    static UISystem()
    {
        var allCompontent = Resources.LoadAll<GameObject>("prefabs/_CompontentLibrary/RawCompontent");
        foreach (var item in allCompontent)
        {
            _rawUICompontentDict.Add(item.name, item);
        }
    }

    public static GameObject GetUICompontent(RawUICompontent rawUICompontent)
    {
        if (!_rawUICompontentDict.ContainsKey(rawUICompontent.ToString()))
        {
            throw new System.Exception("不存在组件：" + rawUICompontent.ToString());
        }
        return _rawUICompontentDict[rawUICompontent.ToString()];
    }

    public static GameObject CreateItemOriginal(string name, params RawUICompontentModel[] rawUICompontentModels)
    {
        var contentOriginal = GetUICompontent(RawUICompontent.Content);
        var content = GameObject.Instantiate(contentOriginal);
        content.name = string.Format("{0}(Original)", name);
        foreach (var item in rawUICompontentModels)
        {
            var original = GetUICompontent(item.RawUICompontent);
            var temp = GameObject.Instantiate(original, content.transform);
            switch (item.RawUICompontent)
            {
                case RawUICompontent.Text:
                    var textCompontentModel = item as TextCompontentModel;
                    if (textCompontentModel != null)
                    {
                        var text = temp.GetComponent<Text>();
                        text.alignment = textCompontentModel.Alignment;
                        text.fontStyle = textCompontentModel.FontStyle;
                        text.text = textCompontentModel.Text;
                    }
                    break;
                case RawUICompontent.Button:
                    var btnCommpontentModel = item as ButtonCompontentModel;
                    if (btnCommpontentModel != null)
                    {
                        temp.transform.Find("Text").GetComponent<Text>().text = btnCommpontentModel.Text;
                    }
                    break;
            }
            temp.transform.name = item.Name;
        }
        content.transform.SetParent(GetRoot().transform);
        return content;
    }

    private static GameObject GetRoot()
    {
        if (_originalRoot == null)
        {
            _originalRoot = new GameObject("--[Original Root]--");
        }
        return _originalRoot;
    }
}


public enum RawUICompontent
{
    /// <summary>
    /// 容器
    /// </summary>
    Content,
    Text,
    Button,
    InputField,
    ScrollView,
    BaseWindow
}

public class RawUICompontentModel
{
    public string Name;
    public RawUICompontent RawUICompontent;
}

public class TextCompontentModel : RawUICompontentModel
{
    public string Text = string.Empty;
    public FontStyle FontStyle = FontStyle.Normal;
    public TextAnchor Alignment = TextAnchor.MiddleCenter;

    public TextCompontentModel()
    {
        RawUICompontent = RawUICompontent.Text;
    }
}

public class ButtonCompontentModel : RawUICompontentModel
{
    public string Text = string.Empty;

    public ButtonCompontentModel()
    {
        RawUICompontent = RawUICompontent.Button;
    }
}
