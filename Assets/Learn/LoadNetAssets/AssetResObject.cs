using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace foo
{
    public sealed class AssetResObject : RefCountUnityObj
    {
        private int _loadAssetTimes;
        private UnityWebRequestLoader _loader = null;
        public AssetResObject(string url) : base(url) { }
        private readonly Dictionary<AssetLoadCallback, object> loadSuccessCallbackList = new Dictionary<AssetLoadCallback, object>();
        private readonly Dictionary<AssetLoadCallback, object> loadFailCallbackList = new Dictionary<AssetLoadCallback, object>();

        public UnityEngine.Object Asset { get; set; } = null;

        public bool SetPreLoad { get; set; } = false;

        public object UserData { get; private set; }

        public string SavePath { get; private set; }

        public string FileName { get; private set; }

        public bool Cache { get; private set; } = false;

        public bool TextureCanNull { get; private set; } = false;

        private void AddCallback(AssetLoadCallback success, AssetLoadCallback error, object userData)
        {
            if (null != success)
            {
                loadSuccessCallbackList.Add(success, userData);
            }

            if (null != error)
            {
                loadFailCallbackList.Add(error, userData);
            }
        }

        private void InvokeSuccessCallback()
        {
            foreach (var kv in loadSuccessCallbackList)
            {
                try
                {
                    kv.Key.Invoke(Url, Asset, kv.Value);
                    CurrentLoadState = LoadState.LOADSTATE_LOADED;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            loadSuccessCallbackList.Clear();
        }

        private void InvokeErrorCallback()
        {
            foreach (var kv in loadFailCallbackList)
            {
                try
                {
                    kv.Key.Invoke(Url, Asset, kv.Value);
                    CurrentLoadState = LoadState.LOADSTATE_UNLOADED;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            loadFailCallbackList.Clear();
        }


        public override void LoadAsync(string url, string savePath, string fileName, AssetLoadCallback successCallback, bool cache, object userData, bool texCanNull = false, AssetLoadCallback errorCallback = null)
        {
            SavePath = savePath;
            FileName = fileName;
            Cache = cache;
            UserData = userData;
            IsSync = false;
            TextureCanNull = texCanNull;
            if (CurrentLoadState == LoadState.LOADSTATE_LOADED)
            {
                successCallback?.Invoke(url, Asset, userData);
                return;
            }
            AddCallback(successCallback, errorCallback, userData);

            AssetWorkerQuene.Instance.Enqueue(this);
        }



        public override bool CheckAndDelete(float now, bool immed)
        {
            if (!base.CheckAndDelete(now, immed) || SetPreLoad == true)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 期望获得资源，默认行为是对加载后从未使用的资源更新加载时间为当前时间
        /// 子类可以在此基础上修改此行为
        /// </summary>
        public void Wanna()
        {
            if (CurrentLoadState == LoadState.LOADSTATE_LOADED && 0 == RefCount)
            {
                LastReleaseTime = -1.0f;  // 重置最后释放时间
                LoadedTime = Time.realtimeSinceStartup;  // 重置加载时间
                AssetResManager.Instance.AddToNeverUsedList(this);
            }
        }

        public override bool Run()
        {
            if (CurrentLoadState == LoadState.LOADSTATE_UNLOADED)
            {
                if (IsSync)
                {

                }
                //暂时全部用异步
                else
                {
                    MonoObject.Instance.StartCoroutine(DownTextureAsset(Url, SavePath, FileName, () =>
                    {
                        LoadedEnd = true;
                        InvokeSuccessCallback();

                    }, UserData, Cache, TextureCanNull, () =>
                    {
                        LoadedEnd = true;
                        InvokeErrorCallback();
                    }));
                }
            }

            return LoadedEnd;
        }

        /// <summary>
        /// 网络加载图片资源
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        /// <param name="fileName"></param>
        /// <param name="finishCallback"></param>
        /// <param name="userData"></param>
        /// <param name="cache"></param>
        /// <param name="texCanNull"></param>
        /// <param name="errCallback"></param>
        /// <returns></returns>
        private IEnumerator DownTextureAsset(string url, string savePath, string fileName, Action finishCallback, object userData, bool cache, bool texCanNull = false, Action errCallback = null)
        {
            CurrentLoadState = LoadState.LOADSTATE_LOADING;
            Action callBack = finishCallback;
            string filePath = new StringBuilder().Append(savePath).Append("/").Append(fileName).Append(".info").ToString();
            ++_loadAssetTimes;
            bool isFileExit = File.Exists(filePath);
            if (_loader != null)
            {
                _loader.StopCoroutine();
            }
            else
            {
                _loader = new UnityWebRequestLoader();
            }

            if (isFileExit)
            {
                string path = new StringBuilder().Append("file://").Append(savePath).Append("/").Append(fileName).Append(".info").ToString();
                Coroutine cor = _loader.GetDownTextureCoroutine(path, (texture2D =>
                 {
                     if (callBack != null)
                     {
                         Asset = texture2D;
                         finishCallback.Invoke();
                         SetLoaded();
                     }
                     Wanna();
                     _loadAssetTimes = 0;

                 }), cache, (() =>
                 {
                     if (_loadAssetTimes < 2) //重试3次
                     {
                         Debug.Log("Retry::" + fileName);
                         MonoObject.Instance.StartCoroutine(DownTextureAsset(url, savePath, fileName, finishCallback, userData, cache, texCanNull, errCallback));
                     }
                     else
                     {
                         _loadAssetTimes = 0;
                         if (texCanNull)
                         {
                             Asset = null;
                             callBack.Invoke();
                         }
                         else
                         {
                             errCallback.Execute();

                         }
                     }

                 }));
                yield return cor;

            }
            else
            {
                Coroutine cor = _loader.GetDownTextureCoroutine(url, texture2D =>
                {

                    if (cache && texture2D != null)
                    {
                        byte[] data = texture2D.EncodeToPNG();


                        Loom.Instance.RunAsync(() =>
                         {
                             var path = Path.Combine(savePath, fileName) + ".info";
                             FileInfo file = new FileInfo(path);
                             if (file.Directory != null && file.Directory.Exists == false)
                             {
                                 file.Directory.Create();
                             }
                             File.WriteAllBytes(path, data);
                         });
                        _loadAssetTimes = 0;
                    }
                    Asset = texture2D;
                    if (callBack != null)
                    {
                        callBack.Invoke();
                    }

                }, cache, () =>
                 {
                     if (_loadAssetTimes < 2) //重试3次
                     {
                         Debug.Log("Retry::" + fileName);
                         MonoObject.Instance.StartCoroutine(DownTextureAsset(url, savePath, fileName, callBack, userData, cache, texCanNull, errCallback));
                     }
                     else
                     {
                         _loadAssetTimes = 0;
                         if (texCanNull)
                         {
                             Asset = null;
                             callBack.Invoke();
                         }
                         else
                         {
                             errCallback.Execute();
                         }
                     }
                 });
                yield return cor;
            }
        }
    }
}

