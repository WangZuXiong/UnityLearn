using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class DownloadAssetManager
{
    private static DownloadAssetImpl _downloadAssetImpl = new DownloadAssetImpl();

    public static void DownloadTextAsync(string url, Action<string> successCallback, Action errorCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_downloadAssetImpl.DownloadText(url, successCallback, errorCallback));
    }

    public static void DownloadTextureAsync(string url, Action<Texture2D> successCallback, Action errorCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_downloadAssetImpl.DownloadTexture(url, successCallback, errorCallback));
    }

    public static void DownloadAssetBundleAsync(AssetBundleConfig config, Action<AssetBundle> successCallback, Action errorCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_downloadAssetImpl.DownloadAssetBundle(config, successCallback, errorCallback));
    }

    public static void ReleaseAllAssetBundle()
    {
        _downloadAssetImpl.ReleaseAll();
    }
}

public class DownloadAssetImpl
{
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
    public IEnumerator DownloadAssetBundle(AssetBundleConfig config, Action<AssetBundle> successCallback, Action errorCallback)
    {
        if (_assetBundleDict.TryGetValue(config, out AssetBundle assetBundle))
        {
            successCallback?.Invoke(assetBundle);
            yield break;
        }

        var cachePath = string.Format("{0}/{1}", Application.persistentDataPath, config.RelativeUrl);

        if (!TryGetCacheByPath(cachePath, out Cache cache))
        {
            errorCallback?.Invoke();
            yield break;
        }

        Caching.currentCacheForWriting = cache;

        var url = string.Format("{0}/{1}/{2}", config.BaseUrl, config.RelativeUrl, config.FileName);

        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url, config.Version, 0))
        {
            uwr.timeout = 5;
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error + "\n" + url);
                errorCallback?.Invoke();
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                if (bundle != null)
                {
                    successCallback?.Invoke(bundle);
                    _assetBundleDict.Add(config, bundle);
                }
                else
                {
                    errorCallback?.Invoke();
                }
            }
        }
    }

    private bool TryGetCacheByPath(string path, out Cache cache)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        List<string> cachePaths = new List<string>();
        Caching.GetAllCachePaths(cachePaths);
        Cache tempCache;

        if (cachePaths.Contains(path))
        {
            tempCache = Caching.GetCacheByPath(path);
        }
        else
        {
            tempCache = Caching.AddCache(path);
        }

        //Make sure your new Cache is valid
        if (!tempCache.valid)
        {
            cache = new Cache();
            return false;
        }

        cache = tempCache;
        return true;
    }


    private Dictionary<AssetBundleConfig, AssetBundle> _assetBundleDict = new Dictionary<AssetBundleConfig, AssetBundle>();

    public void Release(AssetBundle assetBundle)
    {

    }

    public void ReleaseAll()
    {
        foreach (var ab in _assetBundleDict)
        {
            //忽略常驻内存的
            if (ab.Key)
            {
                continue;
            }

            ab.Value.Unload(true);
        }
        _assetBundleDict.Clear();
    }
}



public struct AssetBundleConfig
{
    public string BaseUrl;
    public string RelativeUrl;
    public string FileName;
    public uint Version;


    public override bool Equals(object obj)
    {
        return GetHashCode().Equals(obj.GetHashCode());
    }

    public override int GetHashCode()
    {
        return (RelativeUrl + FileName).GetHashCode();
    }
}

/*
        CachedAssetBundle cachedAssetBundle = new CachedAssetBundle();
        cachedAssetBundle.hash = new Hash128(2, 2, 1, 1);
        cachedAssetBundle.name = "custom name";
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url, cachedAssetBundle))
 */
