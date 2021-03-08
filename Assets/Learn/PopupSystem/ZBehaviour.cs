using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 资源加载等常用模块可以集成到基类
/// </summary>
public class ZBehaviour : MonoBehaviour
{
    private AssetMgr _assetMgr = new AssetMgr();

    protected async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
    {
        Debug.Log("path:" + path);
        var result = await _assetMgr.LoadAssetAsync<T>(path);
        return result;
    }

    protected async Task<IList<T>> LoadAssetsAsync<T>(List<string> keys) where T : UnityEngine.Object
    {
        var result = await _assetMgr.LoadAssetsAsync<T>(keys);
        return result;
    }

    protected async Task<GameObject> InstantiateAsync(string path, Transform parent = null, bool instantiateInWorldSpace = false)
    {
        var result = await _assetMgr.InstantiateAsync(path, parent, instantiateInWorldSpace);
        return result;
    }

    protected async Task<T> InstantiateAsync<T>(string path, Transform parent = null, bool instantiateInWorldSpace = false)
    {
#if UNITY_EDITOR

        if (typeof(T) == typeof(GameObject))
        {
            Debug.LogError("请使用 InstantiateAsync()方法，而不是InstantiateAsync<T>()");
        }
#endif  

        var result = await InstantiateAsync(path, parent, instantiateInWorldSpace);
        return result.GetComponent<T>();
    }

    /// <summary>
    /// 加载图集中的某个精灵
    /// </summary>
    /// <param name="path"></param>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    protected Task<Sprite> LoadAtlasSpriteAsync(string path, string spriteName)
    {
        return _assetMgr.LoadSpriteAsync(path, spriteName);
    }

    /// <summary>
    /// 框架层面去回收资源 使用着无需关心资源的释放
    /// </summary>
    public void Release()
    {
        _assetMgr.Release();
        _assetMgr.Dispose();
    }
}
