using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour
{
    [SerializeField]
    private string _name1 = "cube";
    [SerializeField]
    private string _name2 = "cube1";
    [SerializeField]
    private string _path1 = "AssetBundleLearn/AssetBundles/cube";
    [SerializeField]
    private string _path2 = "AssetBundleLearn/AssetBundles/cube1";
    [SerializeField]
    private Transform _root;
    private void Start()
    {
        _path1 = Application.dataPath + "/" + _path1;
        _path2 = Application.dataPath + "/" + _path2;

        StartCoroutine(DownAB<GameObject>(_name1, _path1));
        StartCoroutine(DownAB<GameObject>(_name2, _path2));
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
        Instantiate(gameObject, _root);
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
        WWW www = WWW.LoadFromCacheOrDownload(_path1, 1);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }
        AssetBundle ab = www.assetBundle;
        GameObject go = ab.LoadAsset<GameObject>(_name1);
        Instantiate(go);
    }

    /// <summary>
    /// 二进制文件  同步加载
    /// </summary>
    private void LoadBinaryAB()
    {
        var binary = File.ReadAllBytes(_path1);
        var ab = AssetBundle.LoadFromMemory(binary);
        var go = ab.LoadAsset<GameObject>(_name1);
        Instantiate(go);
    }

    /// <summary>
    /// 二进制文件 异步加载
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadBinaryABAsync()
    {
        var binary = File.ReadAllBytes(_path1);
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(binary);
        yield return request;
        AssetBundle ab = request.assetBundle;
        GameObject cube = ab.LoadAsset<GameObject>(_name1);
        Instantiate(cube);
    }

    /// <summary>
    /// 从文件进行加载 同步
    /// </summary>
    private void LoadFileAB()
    {
        AssetBundle ab = AssetBundle.LoadFromFile(_path1);
        GameObject go = ab.LoadAsset<GameObject>(_name1);
        Instantiate(go);
    }

    /// <summary>
    /// 从文件进行加载 异步
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadFileABAsync()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(_path1);
        yield return request;
        AssetBundle ab = request.assetBundle;
        GameObject go = ab.LoadAsset<GameObject>(_name1);
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
