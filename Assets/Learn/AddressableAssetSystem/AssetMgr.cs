using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;




//https://zhuanlan.zhihu.com/p/98663058
//trackHandle


/// <summary>
/// 一个单元的资源管理结构
/// </summary>
public class AssetMgr : IDisposable
{
    /// <summary>
    /// 收集这个单元使用过的所有 LoadAsset AsyncOperationHandle
    /// </summary>
    private List<AsyncOperationHandle> _loadAssetHandleCollect;

    /// <summary>
    /// 收集这个单元使用过的所有 Instantiate AsyncOperationHandle
    /// </summary>
    private List<AsyncOperationHandle> _instantiateHandleCollect;

    public AssetMgr()
    {
        _loadAssetHandleCollect = new List<AsyncOperationHandle>();
        _instantiateHandleCollect = new List<AsyncOperationHandle>();
    }

    #region 基础API LoadAsset Instantiate

    public Task<T> LoadAssetAsync<T>(string key)
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        _loadAssetHandleCollect.Add(handle);
        return handle.Task;
    }

    public void LoadAsset<T>(string key, Action<T> successCallback, Action failCallback = null)
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        _loadAssetHandleCollect.Add(handle);

        handle.Completed += (t) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                successCallback?.Invoke(t.Result);
            }
            else
            {
                failCallback?.Invoke();
            }
        };
    }



    public Task<IList<T>> LoadAssetsAsync<T>(List<string> keys)
    {
        Action<T> callback = null;
        var handle = Addressables.LoadAssetsAsync(keys, callback, Addressables.MergeMode.Union);
       //var handle = Addressables.LoadAssetsAsync(keys, callback, true);
        _loadAssetHandleCollect.Add(handle);
        return handle.Task;
    }

    public void LoadAssets<T>(string key, Action<IList<T>> successCallback, Action failCallback = null)
    {
        Action<T> callback = null;
        var handle = Addressables.LoadAssetsAsync(key, callback, true);
        _loadAssetHandleCollect.Add(handle);

        handle.Completed += (t) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                successCallback?.Invoke(t.Result);
            }
            else
            {
                failCallback?.Invoke();
            }
        };
    }



    public Task<GameObject> InstantiateAsync(string key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
    {
        var handle = Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, trackHandle);
        _instantiateHandleCollect.Add(handle);
        return handle.Task;
    }

    public void Instantiate(string key, Action<GameObject> successCallback, Action failCallback = null, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
    {
        var handle = Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, trackHandle);
        _instantiateHandleCollect.Add(handle);

        handle.Completed += (t) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                successCallback?.Invoke(t.Result);
            }
            else
            {
                failCallback?.Invoke();
            }
        };
    }

    #endregion

    #region 拓展
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Sprite"></typeparam>
    /// <param name="path">图集的Addressables Name(Path)</param>
    /// <param name="spriteName">精灵的名称</param>
    /// <returns></returns>
    public Task<Sprite> LoadSpriteAsync(string path, string spriteName)
    {
        var key = string.Format("{0}[{1}]", path, spriteName);
        return LoadAssetAsync<Sprite>(key);
    }
    #endregion


    public void Release()
    {
        foreach (var item in _loadAssetHandleCollect)
        {
            Addressables.Release(item);
        }

        foreach (var item in _instantiateHandleCollect)
        {
            Addressables.ReleaseInstance(item);
        }
        _loadAssetHandleCollect.Clear();
        _instantiateHandleCollect.Clear();
    }

    //[System.Diagnostics.Conditional("DEBUG_ENABLE")]
    public void DebugHandleCollect()
    {
#if UNITY_EDITOR
        string DebugHandleList(List<AsyncOperationHandle> handList)
        {
            var debugNameList = new List<string>();
            foreach (var item in handList)
            {
                debugNameList.Add(item.DebugName);
            }

            return string.Join("\n", debugNameList);
        }

        var result = string.Empty;
        if (_loadAssetHandleCollect.Count > 0)
        {
            var loadAsset = DebugHandleList(_loadAssetHandleCollect);
            result = string.Format("loadAsset:\n{0}", loadAsset);
        }
        if (_instantiateHandleCollect.Count > 0)
        {
            var instantiate = DebugHandleList(_instantiateHandleCollect);
            result += string.Format("\ninstantiate:\n{0}", instantiate);
        }
        if (!string.IsNullOrEmpty(result))
        {
            Debug.LogError(result);
        }
#endif
    }


    ~AssetMgr()
    {
        Dispose(false);
#if UNITY_EDITOR
        //Debug.LogError("以下资源未及时回收：");
        DebugHandleCollect();
#endif
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }
        _loadAssetHandleCollect = null;
        _instantiateHandleCollect = null;
        GC.Collect();
        GC.SuppressFinalize(this);
    }
}


/*
 * 
 * https://blog.csdn.net/zhenghongzhi6/article/details/103334939
 * 
 * 
 采用第二种重载加载时，其实会去先查询每一个地址/标签对应的资源，然后再根据MergeMode进行最终结果的计算。

举个栗子：

比如传入的参数是new List<object>{"cube", "red"}，根据cube查询出来的资源有A、B、D，根据red查询出来的资源有C、D、E。

那么MergeMode是Node或UseFirst时，会取第一个key查询到的资源：A、B、D；

MergeMode是Union时，会取所有key查询到的资源的并集：A、B、C、D、E；

MergeMode是Intersection时，会取所有key查询到的资源的交集：D。
 */
