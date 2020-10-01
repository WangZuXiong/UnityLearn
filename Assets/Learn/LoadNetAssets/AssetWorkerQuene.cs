using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace foo
{

    public interface IAssetWorkerQuene
    {
        int MaxCount { get; set; }

        void Enqueue(IAssetWorkerItem item);

        void Dequeue(IAssetWorkerItem item);

        void WaitDownload(IAssetWorkerItem item);
    }

    public class AssetWorkerQuene : MonoBehaviour, IAssetWorkerQuene
    {
        public List<IAssetWorkerItem> WaittingList { get; } = new List<IAssetWorkerItem>();
        public List<IAssetWorkerItem> RunningList { get; } = new List<IAssetWorkerItem>();
        public List<IAssetWorkerItem> RemoveingList { get; } = new List<IAssetWorkerItem>();
        private float _assetResTickCount = 0.0f;
        private int m_MaxQueueCount = 5;
        private static AssetWorkerQuene _instance;
        public static AssetWorkerQuene Instance
        {
            get
            {
                if (null == _instance)
                {
                    new GameObject("[AssetWorkerQuene]", typeof(AssetWorkerQuene));
                }
                return _instance;
            }
        }

        public void Init()
        {

        }

        public int MaxCount
        {
            get
            {
                return m_MaxQueueCount;
            }

            set
            {
                m_MaxQueueCount = value;
                m_MaxQueueCount = Mathf.Clamp(m_MaxQueueCount, 2, 20);
            }
        }

        void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags |= HideFlags.DontSave;
        }

        void LateUpdate()
        {
            float deltalTime = Time.unscaledDeltaTime;
            _assetResTickCount += deltalTime;
            if (_assetResTickCount > 3.0f) //每3秒检查一次引用为0的时间缓存检查
            {
                AssetResManager.Instance.Tick();
                _assetResTickCount = 0;
            }
        }

        void Update()
        {
            CheckRun();
            Run();
        }
        public void Enqueue(IAssetWorkerItem item)
        {
            if (null == item)
            {
                return;
            }
            if (RunningList.Contains(item) || WaittingList.Contains(item))
            {
                return;
            }
            WaittingList.Add(item);
            CheckRun();

        }

        public void Dequeue(IAssetWorkerItem item)
        {
            if (null == item) return;
            WaittingList.Remove(item);
            RunningList.Remove(item);
        }


        void CheckRun()
        {
            if (AssetResManager.Instance.UnloadUnusedAssetCalling)
            {
                return;
            }

            while (RunningList.Count < MaxCount)
            {
                if (WaittingList.Count == 0)
                {
                    return;
                }

                IAssetWorkerItem item = WaittingList[0];
                WaittingList.RemoveAt(0);
                RunningList.Add(item);
            }
        }

        void Run()
        {
            if (0 == RunningList.Count)
            {
                return;
            }

            int runCount = RunningList.Count;
            for (int i = 0; i < runCount; i++)
            {
                IAssetWorkerItem item = RunningList[i];
                if (item.Run())
                {
                    RefCountUnityObj temp = item as RefCountUnityObj;
                    RemoveingList.Add(temp);
                    if (null != temp)
                    {

                    }
                }
            }

            int removeCount = RemoveingList.Count;
            for (int i = 0; i < removeCount; ++i)
            {
                RunningList.Remove(RemoveingList[i]);
            }
            RemoveingList.Clear();

        }


        public void WaitDownload(IAssetWorkerItem item)
        {

        }
    }

}
