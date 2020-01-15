using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class LoadAssetBundle : MonoBehaviour
{
    private LoadAssetBundle()
    {

    }

    private void Awake()
    {
        Instance = this;
    }

    public static LoadAssetBundle Instance { get; private set; }

    public void LoadAssetBundleAsync<T>(string resName, string url, Action<T> callBack) where T : UnityEngine.Object
    {
        StartCoroutine(LoadAssetbundleByUnityWebRequest(resName, url, callBack));
    }



    /// <summary>
    /// 读取Manifest文件，获取它们的依赖关系并且加载出来
    /// </summary>
    private void Func()
    {
        AssetBundle manifestAB = AssetBundle.LoadFromFile("AssetBundles/AssetBundles");
        AssetBundleManifest manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] strArr = manifest.GetAllDependencies("scene/cubewall.ab");
        foreach (var item in strArr)
        {
            AssetBundle.LoadFromFile("AssetBundles/" + item);
        }
    }





    #region 获取AssetBundle对象的常用API

    /// <summary>
    /// 远程下载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resName"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    private IEnumerator LoadAssetbundleByUnityWebRequest<T>(string resName, string url, Action<T> callBack) where T : UnityEngine.Object
    {
        UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return unityWebRequest.SendWebRequest();
        //AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
        AssetBundle assetBundle = (unityWebRequest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        if (assetBundle == null)
        {
            throw new Exception("assetBundle = null");
        }
        T temp = assetBundle.LoadAsset<T>(resName);
        callBack?.Invoke(temp);
        assetBundle.Unload(false);
    }

    /// <summary>
    /// WWW
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator LoadAssetbundleByWWW(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        AssetBundle assetBundle = www.assetBundle;
        www.Dispose();
    }

    /// <summary>
    /// WWW.LoadFromCacheOrDownload
    /// </summary>
    /// <returns></returns>
    [System.Obsolete]
    private IEnumerator LoadFromCacheOrDownload(string path)
    {
        while (Caching.ready == false)
        {
            yield return null;
        }
        WWW www = WWW.LoadFromCacheOrDownload(path, 1);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }
        AssetBundle ab = www.assetBundle;
        www.Dispose();
    }

    /// <summary>
    /// 从文件进行加载 
    /// </summary>
    /// <returns></returns>
    private void LoadFromFile(string path)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(path);
    }

    /// <summary>
    /// 从文件进行加载 异步
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadFromFileAsync(string path)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        AssetBundle ab = request.assetBundle;
    }

    /// <summary>
    /// 二进制文件
    /// Unity的建议是——不要使用这个API，因为会导致资产在内存中冗余
    /// </summary>
    /// <param name="path"></param>
    private void LoadFromMemory(string path)
    {
        var binary = File.ReadAllBytes(path);
        AssetBundle ab = AssetBundle.LoadFromMemory(binary);
    }

    /// <summary>
    /// 二进制文件 异步加载
    /// Unity的建议是——不要使用这个API，因为会导致资产在内存中冗余
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadFromMemoryAsync(string path)
    {
        var binary = File.ReadAllBytes(path);
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(binary);
        yield return request;
        AssetBundle ab = request.assetBundle;
    }


    private void LoadFromStream(Stream stream)
    {
        AssetBundle assetBundle = AssetBundle.LoadFromStream(stream);
    }


    private IEnumerator LoadFromStreamAsync(Stream stream)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
        yield return request;
        AssetBundle assetBundle = request.assetBundle;
    }

    #endregion


    #region 从AssetBundle加载资源的常用API

    private void LoadAsset(AssetBundle assetBundle, string name)
    {
        object obj1 = assetBundle.LoadAsset(name);
        object obj2 = assetBundle.LoadAsset(name, typeof(GameObject));

    }

    private IEnumerator LoadAssetAsync(AssetBundle assetBundle, string name)
    {
        object obj1 = assetBundle.LoadAssetAsync(name);
        yield return obj1;

        object obj2 = assetBundle.LoadAssetAsync(name, typeof(GameObject));
        yield return obj2;
    }

    private void LoadAllAssets(AssetBundle assetBundle)
    {
        object[] objs1 = assetBundle.LoadAllAssets();
        object[] objs2 = assetBundle.LoadAllAssets(typeof(GameObject));
    }

    private IEnumerator LoadAllAssetsAsync(AssetBundle assetBundle)
    {
        AssetBundleRequest obj1 = assetBundle.LoadAllAssetsAsync();
        yield return obj1;
        UnityEngine.Object[] temp = obj1.allAssets;
        AssetBundleRequest obj2 = assetBundle.LoadAllAssetsAsync(typeof(GameObject));
        yield return obj2;
    }

    /// <summary>
    /// 加载包含多个嵌入式对象的复合Asset时
    /// 例如嵌入动画的FBX模型或嵌入多个精灵的sprite图集
    /// </summary>
    /// <param name="assetBundle"></param>
    /// <param name="name"></param>
    private void LoadAssetWithSubAsset(AssetBundle assetBundle, string name)
    {
        var ab = assetBundle.LoadAssetWithSubAssets(name);
    }

    private IEnumerator LoadAssetWithSubAssetsAsync(AssetBundle assetBundle, string name)
    {
        yield return assetBundle.LoadAssetWithSubAssetsAsync(name);
    }

    #endregion


    #region 卸载

    /// <summary>
    /// 卸载场景物件（GameObject）
    /// </summary>
    /// <param name="gameObject"></param>
    private void UnloadGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="texture2D"></param>
    private void UnloadAsset(Texture2D texture2D)
    {
        Resources.UnloadAsset(texture2D);
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    private void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// AssetBundle卸载
    /// </summary>
    /// <param name="assetBundle"></param>
    private void UnLoadAssetbundle(AssetBundle assetBundle)
    {
        //卸载AssetBundle对象时保留内存中已加载的资源
        assetBundle.Unload(false);
        //卸载AssetBundle对象时卸载内存中已加载的资源，由于该方法容易引起资源引用丢失，因此并不建议经常使用
        assetBundle.Unload(true);
    }

    #endregion
}
