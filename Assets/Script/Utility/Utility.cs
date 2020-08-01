using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Utility
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
    /// List Remove Advance 
    /// 对List排序无要求的情况下
    /// 可以先将要remove的元素与List尾部的元素交换位置，之后在remove
    /// 这样就少了整体向后偏移 的一个过程
    /// 效率能够提高
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="value"></param>
    public static void ListRemoveAdvance<T>(List<T> list, T value) 
    {
        if (value.Equals(list[list.Count - 1]))
        {
            list.Remove(value);
            return;
        }

        int index = list.FindIndex((item) =>
        {
            return item.Equals(value);
        });
        //将value移到最后
        var temp = list[index];
        list[index] = list[list.Count - 1];
        list[list.Count - 1] = temp;

        list.Remove(value);
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

        if (node == null)
        {
            return string.Empty;
        }
        nodenames.Add(node.name);
        Transform temp = node;
        while (null != temp.parent)
        {
            nodenames.Add(temp.parent.name);
            temp = temp.parent;
        }
        nodenames.Reverse();

        return string.Join("/", nodenames.ToArray());
    }

    public static bool Execute<T>(this EventHandler<T> self, object sender, T e) where T : EventArgs
    {
        if (self != null)
        {
            self(sender, e);
            return true;
        }
        return false;
    }

    public static bool Execute(this Action self)
    {
        if (self != null)
        {
            self();
            return true;
        }
        return false;
    }

    public static bool Execute<T>(this Action<T> self, T t)
    {
        if (self != null)
        {
            self(t);
            return true;
        }
        return false;
    }

    public static bool Execute<T1, T2>(this Action<T1, T2> self, T1 t1, T2 t2)
    {
        if (self != null)
        {
            self(t1, t2);
            return true;
        }
        return false;
    }

    public static bool Execute<T1, T2, T3>(this Action<T1, T2, T3> self, T1 t1, T2 t2, T3 t3)
    {
        if (self != null)
        {
            self(t1, t2, t3);
            return true;
        }
        return false;
    }

    public static bool Execute<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        if (self != null)
        {
            self(t1, t2, t3, t4);
            return true;
        }
        return false;
    }
}
