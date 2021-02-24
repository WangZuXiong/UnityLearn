using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using foo;

public class AssetResManager : MonoBehaviour
{
    private readonly Dictionary<string, AssetResObject> _assetResDic = new Dictionary<string, AssetResObject>();
    public bool UnloadUnusedAssetCalling { get; private set; } = false;
    /// <summary>
    /// 等待卸载列表
    /// </summary>
    private readonly HashSet<RefCountUnityObj> _waittingDeleteList = new HashSet<RefCountUnityObj>();
    /// <summary>
    /// 从未使用过的资源
    /// </summary>
    private readonly HashSet<RefCountUnityObj> _neverUsedList = new HashSet<RefCountUnityObj>();
    private readonly List<RefCountUnityObj> _addingDeleteList = new List<RefCountUnityObj>();
    private readonly List<AssetResObject> _canUnloadAssetResObj = new List<AssetResObject>();
    /// <summary>
    /// 卸载时不做延迟缓存
    /// </summary>
    public static bool unloadNotDelayCache = false;
    /// <summary>
    /// 执行资源卸载的最小资源
    /// </summary>
    private static readonly int _doUnloadAssetsMinCount = 2;

    public Dictionary<string, AssetResObject> AssetTextureDic
    {
        get { return AssetTextureDic; }
    }

    private static AssetResManager _instance;
    public static AssetResManager Instance
    {
        get
        {
            if (null == _instance)
            {
                new GameObject("[AssetResManager]", typeof(AssetResManager));
                AssetWorkerQuene.Instance.Init();
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.hideFlags |= HideFlags.DontSave;
    }

    /// <summary>
    /// 增加引用计数
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public int AddReferenceCount(string assetPath)
    {
        if (_assetResDic.TryGetValue(assetPath, out AssetResObject resObj))
        {
            resObj.Acquire();
            return resObj.RefCount;
        }

        return 0;
    }

    /// <summary>
    /// 减少引用计数
    /// </summary>
    /// <param name="path"></param>
    /// <param name="releaseWaitTime"></param>
    public void DecReferenceCount(string path, int releaseWaitTime = 0)
    {
        if (_assetResDic.TryGetValue(path, out AssetResObject resObj))
        {
            resObj.Release(releaseWaitTime);
        }
    }

    public void ReleaseAllPreLoad()
    {
        var iter = _assetResDic.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.SetPreLoad = false;
        }

        Tick();
    }

    public void SetResIsPreLoad(string path)
    {
        if (_assetResDic.TryGetValue(path, out AssetResObject resObject))
        {
            resObject.SetPreLoad = true;
        }
    }

    //检查资源卸载的Tick
    public void Tick()
    {
        UnloadUnusedAssets(unloadNotDelayCache);
    }

    private void UnloadUnusedAssets(bool immed)
    {
        _canUnloadAssetResObj.Clear();
        //无等待卸载列表则直接返回
        if (0 == _waittingDeleteList.Count && _addingDeleteList.Count == 0)
        {
            return;
        }
        float nowTime = Time.realtimeSinceStartup;
        List<AssetResObject> removeList = new List<AssetResObject>();
        _addingDeleteList.ForEach((item) =>
        {
            _waittingDeleteList.Add(item);
        });
        _addingDeleteList.Clear();
        foreach (AssetResObject item in _waittingDeleteList)
        {
            if (item.RefCount > 0)
            {
                removeList.Add(item);
            }
            if (immed)
            {
                if (item.CheckAndDeleteImmediate())
                {
                    _canUnloadAssetResObj.Add(item);
                }
            }
            else
            {
                if (item.CheckAndDelete(nowTime, immed))
                {
                    _canUnloadAssetResObj.Add(item);
                }
            }
        }
        int count = _canUnloadAssetResObj.Count;
        if (count > _doUnloadAssetsMinCount - 1)
        {
            for (int i = 0; i < count; i++)
            {
                DeleteObject(_canUnloadAssetResObj[i]);
                removeList.Add(_canUnloadAssetResObj[i]);
                _canUnloadAssetResObj[i].Reset();
                _canUnloadAssetResObj[i].Asset = null;
            }

            StartCoroutine(StartUnloadUnusedAssets());
        }

        removeList.ForEach((item) =>
        {
            _waittingDeleteList.Remove(item);
        });
    }

    IEnumerator StartUnloadUnusedAssets()
    {
        UnloadUnusedAssetCalling = true;
        AsyncOperation op = Resources.UnloadUnusedAssets();
        yield return op;
        UnloadUnusedAssetCalling = false;
    }

    //从列表中删除
    private void DeleteObject(RefCountUnityObj obj)
    {
        if (obj is AssetResObject aro)
        {
            if (_assetResDic.TryGetValue(aro.Url, out AssetResObject aro1) && aro1 == aro)
            {
                _assetResDic.Remove(aro.Url);
            }

        }
    }

    //加入从未使用过的资源列表
    public void AddToNeverUsedList(RefCountUnityObj o)
    {
        _neverUsedList.Add(o);
    }

    //从从未使用过的资源列表中移除
    public void RemoveFromNeverUsedList(RefCountUnityObj o)
    {
        _neverUsedList.Remove(o);
    }

    public void AddToWaittingDeleteList(RefCountUnityObj o)
    {
        _addingDeleteList.Add(o);
    }

    public void LoadAsync(string url, string savePath, string fileName, AssetLoadCallback finishCallback, bool cache, object setter, bool texCanNull = false, AssetLoadCallback errCallback = null, bool isPreLoad = false)
    {
        if (_assetResDic.TryGetValue(url, out AssetResObject resObj))
        {
            resObj.SetPreLoad = false;
        }
        else
        {
            resObj = new AssetResObject(url)
            {
                SetPreLoad = isPreLoad
            };
            _assetResDic[url] = resObj;

        }
        resObj.LoadAsync(url, savePath, fileName, (path, asset, data) =>
        {
            if (finishCallback != null)
            {
                finishCallback.Invoke(path, asset, data);
            }
        }, cache, setter, texCanNull, (path, asset, data) =>
        {
            if (errCallback != null)
            {
                errCallback.Invoke(path, asset, data);
            }
            DeleteObject(resObj);
        });
    }
}
