using foo;
using System;
using System.IO;
using UnityEngine;

public class RefCommonLoader
{
    public const string BASE_URL = "";

    public static void LoadTextureByAssetTypeAndName(GameObject obj, string name, Action<Texture2D> callback, bool cache = true, AssetLoadCallback erroBack = null, bool isPreLoad = false, bool canbeNull = false)
    {
        string url = Path.Combine(BASE_URL, name) + ".png";
        string savePath = Path.Combine(Application.persistentDataPath, name);
        AssetSetter.SetImageSetter(obj, url, savePath, name, (path, asset, userdata) =>
        {
            Texture2D text = asset as Texture2D;
            callback.Execute(text);
        }, cache, erroBack, canbeNull, isPreLoad);
    }
}
