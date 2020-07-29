using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAssetManager
{
    private static DownloadAssetManager _instance;
    public static DownloadAssetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DownloadAssetManager();
            }
            return _instance;
        }
    }

    public IEnumerator DownloadText(string url, Action<string> successCallback, Action errorCallback)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                errorCallback?.Invoke();
            }
            else
            {
                var text = uwr.downloadHandler.text;
                successCallback?.Invoke(text);
            }
        }
    }

    public IEnumerator DownloadTexture(string url, Action<Texture2D> successCallback, Action errorCallback)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                errorCallback?.Invoke();
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                successCallback?.Invoke(texture);
            }
        }
    }

    //Caching 类用于管理使用 UnityWebRequestAssetBundle.GetAssetBundle() 下载的缓存 AssetBundle。

    public IEnumerator DownloadAssetBundle(AssetConfig config, Action<AssetBundle> successCallback, Action errorCallback)
    {
        Debug.Log(Application.persistentDataPath);

        var cachePath = string.Format("{0}/{1}", Application.persistentDataPath, config.RelativeUrl);

        if (Path2Cache(cachePath, out Cache cache))
        {
            Caching.currentCacheForWriting = cache;

            var url = string.Format("{0}/{1}/{2}", config.BaseUrl, config.RelativeUrl, config.FileName);

            using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url, config.Version, 0))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                    errorCallback?.Invoke();
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    successCallback?.Invoke(bundle);
                }
            }
        }
        else
        {
            errorCallback?.Invoke();
            yield return null;
        }
    }


    private bool Path2Cache(string path, out Cache cache)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        Cache newCache = Caching.AddCache(path);

        //Make sure your new Cache is valid
        if (!newCache.valid)
        {
            cache = new Cache();
            return false;
        }

        cache = newCache;
        return true;
    }


    public struct AssetConfig
    {
        public string BaseUrl;
        public string RelativeUrl;
        public string FileName;
        public uint Version;
    }
}


/*
        CachedAssetBundle cachedAssetBundle = new CachedAssetBundle();
        cachedAssetBundle.hash = new Hash128(2, 2, 1, 1);
        cachedAssetBundle.name = "custom name";
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url, cachedAssetBundle))
 */
