using UnityEngine;

class RuntimeInitialize
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        Debug.Log("Before first Scene loaded");

        //做一些配置上的设置
        PlayAudioManager.Init(1, 1);


        //DownloadAssetManager.DownloadAssetBundleAsync(new AssetBundleConfig(), (t) =>
        //{
        //    var languageJson = t.LoadAsset<TextAsset>("LanguageConfig").text;
        //    LocalizationManager.InitLanguageDict(languageJson);
        //}, () => { });





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

    //允许在 Unity 加载时初始化编辑器类方法，无需用户操作。
    //[UnityEditor.InitializeOnLoadMethod]
    //static void OnProjectLoadedInEditor()
    //{
    //    Debug.Log("Project loaded in Unity Editor");
    //}
}