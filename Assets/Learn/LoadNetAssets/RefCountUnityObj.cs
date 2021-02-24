using UnityEngine;


namespace foo
{
    public enum LoadState
    {
        LOADSTATE_LOADED = 0,
        LOADSTATE_LOADING = 1,
        LOADSTATE_UNLOADED = 2,
    }


    public interface IAssetWorkerItem
    {
        // 工作执行函数，注意，这个是等待其它线程执行结果的函数,执行完成返回true,未完成返回false
        bool Run();
        // 等待执行的优先级
        int Priority { get; set; }
    }

    public class RefCountUnityObj : IAssetWorkerItem
    {
        public static int RELEASE_WAIT_DEFAULT = 0; //默认等待持续时间
        private int _releaseWaitTime = RELEASE_WAIT_DEFAULT;
        public bool IsSync { get; set; } = false;
        public string Url { get; }
        public LoadState CurrentLoadState { get; set; } = LoadState.LOADSTATE_UNLOADED;
        public bool LoadedEnd { get; set; } = false;
        public int RefCount { get; private set; } = 0;
        /// <summary>
        /// 加载完成时间
        /// </summary>
        public float LoadedTime { get; protected set; } = -1.0f;
        public float LastReleaseTime { get; set; } = -1.0f;
        public int Priority { get; set; } = 0;


        public RefCountUnityObj(string url)
        {
            Url = url;
            LoadedEnd = false;
            _releaseWaitTime = RELEASE_WAIT_DEFAULT;
        }

        public virtual void Reset()
        {
            LoadedTime = -1.0f;
            _releaseWaitTime = RELEASE_WAIT_DEFAULT;
            LastReleaseTime = -1.0f;
            RefCount = 0;
            CurrentLoadState = LoadState.LOADSTATE_UNLOADED;
            LoadedEnd = false;
        }


        protected void SetLoaded()
        {
            CurrentLoadState = LoadState.LOADSTATE_LOADED;
            LoadedTime = Time.realtimeSinceStartup;
        }

        //增加引用计数
        public virtual int Acquire()
        {
            RefCount += 1;
            LastReleaseTime = -1.0f;
            return RefCount;
        }



        //减少引用计数
        public virtual void Release(int waitTime = 0)
        {
            if (RefCount <= 0)
            {
                //todo release 
                AssetResManager.Instance.AddToWaittingDeleteList(this);

            }
            else
            {
                RefCount -= 1;
                if (RefCount == 0)
                {
                    LastReleaseTime = Time.realtimeSinceStartup;
                    //todo release
                    AssetResManager.Instance.AddToWaittingDeleteList(this);

                }

                //只有特殊标志才更新释放时间
                if (waitTime < 0 || waitTime > _releaseWaitTime)
                {
                    _releaseWaitTime = waitTime;
                }
            }
        }



        /// <summary>
        /// 检查并且卸载资源
        /// </summary>
        /// <param name="now">当前时间</param>
        /// <param name="immed"> 是否立即卸载资源不用考虑时间缓存</param>
        /// <returns></returns>
        public virtual bool CheckAndDelete(float now, bool immed)
        {
            if (RefCount > 0)
            {
                return false;
            }

            if (LastReleaseTime < 0f) //加载了从未使用过
            {
                return false;
            }

            if (immed)
            {
                return true;
            }
            return (now - LastReleaseTime) > _releaseWaitTime;
        }

        // 递归检查删除资源，默认立即模式，且无需检测释放等待时间
        public virtual bool CheckAndDeleteImmediate()
        {
            if (RefCount > 0)
            {
                return false;
            }


            if (LastReleaseTime < 0.0f)  // 加载了还没用过,或接下来又要使用(见AssetResourceObject.Wanna)
            {
                return false;
            }

            return true;
        }

        // 异步加载
        public virtual void LoadAsync(string url, string savePath, string fileName, AssetLoadCallback erroBack, bool cache, object userData, bool texCanNull = false, AssetLoadCallback errCallback = null)
        {

        }


        public virtual bool Run()
        {
            return false;
        }
    }

}
