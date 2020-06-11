using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : MonoSingleton<UIFactory>
{
    private readonly string UIBASEPATH = "Prefabs/UI/";


    public BaseView GetUIView(string uiName)
    {
        GameObject original = Resources.Load<GameObject>(UIBASEPATH + uiName); //assetSerice
        GameObject go = Instantiate(original, GetUIParent());

        if (uiName == "SetView")
        {
            return go.GetComponent<SetView>();
        }
        else
        {
            return null;
        }
    }

    public T GetUI<T>(string path) where T : Object
    {
        T uiOriginal = Resources.Load<T>(UIBASEPATH + path);
        if (uiOriginal == null)
        {
            Debug.LogError("路径有误：" + UIBASEPATH + path);
            return null;
        }
        T ui = Instantiate(uiOriginal);
        return ui;
    }


    public T GetUI<T>(string path, Transform parent) where T : Object
    {
        T uiOriginal = Resources.Load<T>(UIBASEPATH + path);
        if (uiOriginal == null)
        {
            Debug.LogError("路径有误：" + UIBASEPATH + path);
            return null;
        }
        T ui = Instantiate(uiOriginal, parent);
        return ui;
    }




    private Transform GetUIParent()
    {
        //根据UI的类型取父节点
        //if (_viewName == "set")
        //{
        //}
        return GameObject.Find("UICanvas").transform;
    }
}
