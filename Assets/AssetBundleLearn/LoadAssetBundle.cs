using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//https://blog.uwa4d.com/archives/ABTheory.html
public class LoadAssetBundle : MonoBehaviour
{
    public string Name1 = "cube";
    public string Name2 = "cube1";
    public string Path1 = @"D:\wangzuxiong\UnityLearn\Assets\AssetBundleLearn\AssetBundles\cube";
    public string Path2 = @"D:\wangzuxiong\UnityLearn\Assets\AssetBundleLearn\AssetBundles\cube1";

    private void Start()
    {
        StartCoroutine(DownAB<GameObject>(Name1, Path1));
        StartCoroutine(DownAB<GameObject>(Name2, Path2));
    }

    /// <summary>
    /// 远程下载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resName"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    private IEnumerator DownAB<T>(string resName, string url) where T : Object
    {
        UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return unityWebRequest.SendWebRequest();
        //AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
        AssetBundle assetBundle = (unityWebRequest.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        Debug.LogError(assetBundle == null);
        T gameObject = assetBundle.LoadAsset<T>(resName);
        Instantiate(gameObject, Camera.main.transform);
    }

    /// <summary>
    /// WWW.LoadFromCacheOrDownload
    /// </summary>
    /// <returns></returns>
    [System.Obsolete]
    private IEnumerator LoadCacheOrDownloadFromFile()
    {
        while (Caching.ready == false)
        {
            yield return null;
        }
        WWW www = WWW.LoadFromCacheOrDownload(Path1, 1);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }
        AssetBundle ab = www.assetBundle;
        GameObject go = ab.LoadAsset<GameObject>(Name1);
        Instantiate(go);
    }

    /// <summary>
    /// 二进制文件  同步加载
    /// </summary>
    private void LoadBinaryAB()
    {
        var binary = File.ReadAllBytes(Path1);
        var ab = AssetBundle.LoadFromMemory(binary);
        var go = ab.LoadAsset<GameObject>(Name1);
        Instantiate(go);
    }

    /// <summary>
    /// 二进制文件 异步加载
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadBinaryABAsync()
    {
        var binary = File.ReadAllBytes(Path1);
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(binary);
        yield return request;
        AssetBundle ab = request.assetBundle;
        GameObject cube = ab.LoadAsset<GameObject>(Name1);
        Instantiate(cube);
    }

    /// <summary>
    /// 从文件进行加载 同步
    /// </summary>
    private void LoadFileAB()
    {
        AssetBundle ab = AssetBundle.LoadFromFile(Path1);
        GameObject go = ab.LoadAsset<GameObject>(Name1);
        Instantiate(go);
    }

    /// <summary>
    /// 从文件进行加载 异步
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadFileABAsync()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Path1);
        yield return request;
        AssetBundle ab = request.assetBundle;
        GameObject go = ab.LoadAsset<GameObject>(Name1);
        Instantiate(go);
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
}
