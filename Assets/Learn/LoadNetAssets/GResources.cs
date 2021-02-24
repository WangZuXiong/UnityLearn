using UnityEngine;

namespace foo
{
    /// <summary>
    /// 异步加载U3D资源回调
    /// </summary>
    /// <param name="path"></param>
    /// <param name="asset"></param>
    /// <param name="userData"></param>
    public delegate void AssetLoadCallback(string path, Object asset, object userData);

    public class GResources
    {

        /// <summary>
        /// 增加资源引用计数
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns></returns>
        public static int AddReferenceCount(string path)
        {
            return AssetResManager.Instance.AddReferenceCount(path);
        }

        /// <summary>
        /// 减少资源引用计数
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="releaseWaitTime">释放等待时间</param>
        public static void DecReferenceCount(string path, int releaseWaitTime = 0)
        {
            AssetResManager.Instance.DecReferenceCount(path, releaseWaitTime);
        }

        public static void DestroyGameObject(GameObject go)
        {
            if (null == go)
            {
                return;
            }

            //UnityEngine.Profiling.Profiler.BeginSample("ManualRelease");
            AssetRefComponent.Release(go);
            //UnityEngine.Profiling.Profiler.EndSample();
            GameObject.Destroy(go);

        }


        /// <summary>
        /// 异步加载一个U3D资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="callBack">回调</param>
        /// <param name="userData">传参数据</param>
        public static void LoadAsync(string url, string savePath, string fileName, AssetLoadCallback finishCallback, object setter, bool cache, bool texCanNull = false, AssetLoadCallback errCallback = null, bool isPreLoad = false)
        {
            AssetResManager.Instance.LoadAsync(url, savePath, fileName, finishCallback, cache, setter, texCanNull, errCallback, isPreLoad);
        }
    }
}
