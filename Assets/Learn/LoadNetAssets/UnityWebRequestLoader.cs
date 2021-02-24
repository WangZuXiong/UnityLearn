using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestLoader
{
    private readonly int TIME_OUT = 5;
    private DownloadHandlerTexture handle = new DownloadHandlerTexture();
    private Coroutine _enumerator;

    public Coroutine GetDownTextureCoroutine(string path, Action<Texture2D> finishCallback, bool cache, Action errCallback = null)
    {
        if (_enumerator != null)
        {
            handle = new DownloadHandlerTexture();
        }
        Coroutine cor = MonoObject.Instance.StartCoroutine(DownTextureCoroutine(path, finishCallback, cache, errCallback));
        _enumerator = cor;
        return _enumerator;
    }

    private IEnumerator DownTextureCoroutine(string path, Action<Texture2D> finishCallback, bool cache, Action errCallback = null)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(path);
        webRequest.downloadHandler = handle;
        webRequest.timeout = TIME_OUT;
        webRequest.disposeDownloadHandlerOnDispose = true;
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            if (errCallback != null)
            {
                errCallback.Invoke();
            }
        }
        else
        {
            if (finishCallback != null)
            {
#if UNITY_EDITOR
                handle.texture.name = path;
#endif
                finishCallback.Invoke(handle.texture);
            }
        }
        webRequest.Dispose();
    }

    public void StopCoroutine()
    {
        if (_enumerator != null)
        {
            MonoObject.Instance.StopCoroutine(_enumerator);
        }
    }
}
