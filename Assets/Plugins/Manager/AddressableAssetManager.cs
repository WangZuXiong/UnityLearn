using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


[Obsolete("废弃了 使用AssetMgr代替")]
public static class AddressableAssetManager
{
    private struct AssetHandle
    {
        public object Handle;
        public Type AssetType;

        public AssetHandle(object handle, Type assetType)
        {
            Handle = handle;
            AssetType = assetType;
        }
    }


    private static readonly Dictionary<object, AssetHandle> _assetHandleDict = new Dictionary<object, AssetHandle>();

    private static readonly Dictionary<object, List<AsyncOperationHandle>> _gameObjectHandleDict = new Dictionary<object, List<AsyncOperationHandle>>();




    //private string GetUniqueKey(string rawKey)
    //{
    //    return rawKey.ToString()+Time.s
    //}


    #region Async Load Asset

    public async static Task<T> LoadAssetAsync<T>(object key)
    {
        if (_assetHandleDict.TryGetValue(key, out AssetHandle value))
        {
            var handle = (AsyncOperationHandle<T>)value.Handle;
            return handle.Result;
        }
        else
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            _assetHandleDict.Add(key, new AssetHandle(handle, typeof(T)));
            var task = await handle.Task;
            return task;
        }
    }

    public static void LoadAssetAsync<T>(object key, Action<T> successCallback, Action failCallback)
    {
        if (_assetHandleDict.TryGetValue(key, out AssetHandle value))
        {
            var handle = (AsyncOperationHandle<T>)value.Handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                successCallback?.Invoke(handle.Result);
            }
            else
            {
                failCallback?.Invoke();
            }
        }
        else
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            handle.Completed += (t) =>
            {
                _assetHandleDict.Add(key, new AssetHandle(handle, typeof(T)));
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
    }

    public async static Task<IList<T>> LoadAssetsAsync<T>(object key)
    {
        if (_assetHandleDict.TryGetValue(key, out AssetHandle value))
        {
            var handle = (AsyncOperationHandle<IList<T>>)value.Handle;
            return handle.Result;
        }
        else
        {
            Action<T> callback = null;

            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync(key, callback, true);
            return await handle.Task;
        }
    }
    #endregion

    #region Async Instantiate

    public async static Task<GameObject> InstantiateAsync(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
    {
        var handle = Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, trackHandle);
        //if (_gameObjectHandleDict.TryGetValue(key, out List<AsyncOperationHandle> list))
        //{
        //    list.Add(handle);
        //}
        //else
        //{
        //    var newList = new List<AsyncOperationHandle>() { handle };
        //    _gameObjectHandleDict.Add(key, newList);
        //}
        return await handle.Task;
    }

    public static void InstantiateAsync(object key, Action<GameObject> successCallback, Action failCallback)
    {
        var handle = Addressables.InstantiateAsync(key);
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

    #region Release

    public static void ReleaseObject<TObject>(TObject obj)
    {
        foreach (var item in _assetHandleDict)
        {
            if (item.Value.Equals(obj))
            {
                ReleaseByKey(item.Key);
            }
        }
    }

    public static void ReleaseAll()
    {
        foreach (var item in _assetHandleDict)
        {
            ReleaseByKey(item.Key);
        }
    }

    public static void ReleaseByKey(object key)
    {
        if (_assetHandleDict.TryGetValue(key, out AssetHandle value))
        {
            if (value.AssetType == typeof(GameObject))
            {
                var handle = (AsyncOperationHandle<GameObject>)value.Handle;
                Addressables.Release(handle);
                _assetHandleDict.Remove(key);
            }
            else if (value.AssetType == typeof(Texture2D))
            {
                var handle = (AsyncOperationHandle<Texture2D>)value.Handle;
                Addressables.Release(handle);
                _assetHandleDict.Remove(key);
            }
            else if (value.AssetType == typeof(Sprite))
            {
                var handle = (AsyncOperationHandle<Sprite>)value.Handle;
                Addressables.Release(handle);
                _assetHandleDict.Remove(key);
            }
            else if (value.AssetType == typeof(Texture2D[]))
            {
                var handle = (AsyncOperationHandle<Texture2D[]>)value.Handle;
                Addressables.Release(handle);
                _assetHandleDict.Remove(key);
            }
            else if (value.AssetType == typeof(List<Texture2D>))
            {
                var handle = (AsyncOperationHandle<List<Texture2D>>)value.Handle;
                Addressables.Release(handle);
                _assetHandleDict.Remove(key);
            }
        }
    }

    public static bool ReleaseInstance(GameObject instance)
    {
        return Addressables.ReleaseInstance(instance);
    }

    #endregion
}
