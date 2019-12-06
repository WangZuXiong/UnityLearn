using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class UnityUtility
{
    /// <summary>
    /// 16进制颜色码转化为RGBA颜色值
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string RGBAToHtmlString(Color color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        return hex;
    }

    /// <summary>
    /// 16进制颜色码转化为RGBA颜色值
    /// </summary>
    /// <param name="htmlString">#CC00FF</param>
    /// <returns>（204,0,255,255）</returns>
    public static Color HtmlStringToRGBA(string htmlString)
    {
        if (!htmlString.Contains("#"))
        {
            Debug.LogError(">>>输入有误！");
        }

        Color color;
        ColorUtility.TryParseHtmlString(htmlString, out color);
        return color;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindInChild(this Transform transform, string name)
    {
        Transform[] transforms = transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].name.Equals(name))
            {
                return transforms[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 随机打乱List顺序
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private static List<T> BreakSortList<T>(List<T> list)
    {
        var random = new System.Random();
        var newList = new List<T>();
        for (int i = 0; i < list.Count; i++)
        {
            newList.Insert(random.Next(newList.Count + 1), list[i]);
        }
        return newList;
    }

    /// <summary>
    /// 返回UI在Canvas的绝对坐标
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Vector2 GetUIPosRelativeCanvas(Canvas canvas, Transform transform)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, transform.position, canvas.GetComponent<Camera>(), out Vector2 pos))
        {
            return pos;
        }
        return pos;
    }


    /// <summary>
    /// 获取对象的属性和值
    /// </summary>
    /// <param name="obj">对象</param>
    /// <returns>返回属性与值一一对应的字典</returns>
    public static List<FieldInfo> GetFilesInfo<T>(T obj)
    {
        List<FieldInfo> infoList = new List<FieldInfo>();
        if (obj != null)
        {
            Type type = obj.GetType();

            FieldInfo[] propertyInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo item in propertyInfos)
            {
                Type type2 = item.GetType();
                infoList.Add(item);

            }

            return infoList;
        }
        return null;

        /*
            List<FieldInfo> infoDic = UnityUtility.GetFilesInfo(this);
            int infoCount = infoDic.Count;
            for (int i = 0; i < infoCount; i++)
            {
                try
                {
                    infoDic[i].SetValue(this, null);
                }
                catch (Exception)
                {

                }
                infoDic[i].SetValue(this, null);
            }
         */
    }



    public static string GetNodePath(this Transform node)
    {
        List<string> nodenames = new List<string>();
        nodenames.Add(node.name);
        Transform temp = node;
        while (null != temp.parent)
        {
            nodenames.Add(temp.parent.name);
            temp = temp.parent;
        }
        nodenames.Reverse();
        return string.Join(".", nodenames.ToArray());
    }
}
