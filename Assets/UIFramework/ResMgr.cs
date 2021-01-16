using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResMgr
{
    public static T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public static T Instantiation<T>(string path, Transform parent, string name = null)
    {
        return Instantiation(path, parent, name).GetComponent<T>();
    }

    public static GameObject Instantiation(string path, Transform parent, string name = null)
    {
        var original = Load<GameObject>(path);
        var obj = Object.Instantiate(original,parent);
        if (!string.IsNullOrEmpty(name))
        {
            obj.name = name;
        }
        if (obj is GameObject)
        {
            var goObj = obj as GameObject;
            goObj.transform.localPosition = Vector3.zero;
            goObj.transform.rotation = Quaternion.identity;
            goObj.transform.localScale = Vector3.one;
        }
        return obj;
    }
}
