using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public delegate void OnLoadSceneAsync(float progress);

public delegate void OnUnloadSceneCompleted();


public static class LoadSceneManager
{

    public static LoadSceneImpl _loadSceneImpl = new LoadSceneImpl();

    public static void LoadSceneAsync(string sceneName, OnLoadSceneAsync onLoadSceneAsync)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_loadSceneImpl.LoadSceneAsync(sceneName, onLoadSceneAsync));
    }

    public static void UnloadSceneAsync(string sceneName, OnUnloadSceneCompleted onUnloadSceneCompleted)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_loadSceneImpl.UnloadSceneAsync(sceneName, onUnloadSceneCompleted));
    }
}

public class LoadSceneImpl
{
    public IEnumerator LoadSceneAsync(string sceneName, OnLoadSceneAsync onLoadSceneAsync)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        void action(AsyncOperation t)
        {
            onLoadSceneAsync?.Invoke(t.progress);
        }

        asyncOperation.completed += action;

        //asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            onLoadSceneAsync?.Invoke(asyncOperation.progress);
            //if (asyncOperation.progress >= 0.9f)
            //{
            //    asyncOperation.allowSceneActivation = true;
            //}

            yield return null;
        }
    }

    public IEnumerator UnloadSceneAsync(string sceneName, OnUnloadSceneCompleted onUnloadSceneCompleted)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
        //Action<AsyncOperation> action = (t) => { onUnloadSceneCompleted?.Invoke(); };

        void action(AsyncOperation t)
        {
            onUnloadSceneCompleted?.Invoke();
        }

        asyncOperation.completed += action;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}