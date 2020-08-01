
using UnityEngine;

class RuntimeInitialize
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        //Debug.Log("Before first Scene loaded");

        //做一些配置上的设置
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod()
    {
        //Debug.Log("After first Scene loaded");
    }
    //游戏加载后，将调用标记为 [RuntimeInitializeOnLoadMethod] 的 方法。这是在调用 Awake 方法后进行的。
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        //Debug.Log("RuntimeMethodLoad: After first Scene loaded");
    }
}