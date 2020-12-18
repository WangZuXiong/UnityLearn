using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestManager
{

    private static readonly WebRequestImpl _webRequestImpl = new WebRequestImpl();

    public static void Upload(string url, string data, Action OnSuccessCallback, Action OnFailCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_webRequestImpl.Upload(url, data, OnSuccessCallback, OnFailCallback));
    }


    public static void Get(string uri, Action<string> OnSuccessCallback, Action OnFailCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_webRequestImpl.Get(uri, OnSuccessCallback, OnFailCallback));
    }

    public static void Post(string uri, string postData, Action<string> OnSuccessCallback, Action OnFailCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_webRequestImpl.Post(uri, postData, OnSuccessCallback, OnFailCallback));
    }
}


public class WebRequestImpl
{
    public IEnumerator Upload(string url, string data, Action OnSuccessCallback, Action OnFailCallback)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(url, data))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                OnFailCallback?.Invoke();
            }
            else
            {
                Debug.Log("Upload complete!");
                OnSuccessCallback?.Invoke();
            }
        }
    }

    public IEnumerator Get(string uri, Action<string> OnSuccessCallback, Action OnFailCallback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
                Debug.Log(uri);

                OnFailCallback?.Invoke();
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                OnSuccessCallback?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }


    public IEnumerator Post(string uri, string postData, Action<string> OnSuccessCallback, Action OnFailCallback)
    {
        //using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postData))
        //{
        //    webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        //    yield return webRequest.SendWebRequest();

        //    if (webRequest.isNetworkError)
        //    {
        //        Debug.Log("Error: " + webRequest.error);
        //        OnFailCallback?.Invoke();
        //    }
        //    else
        //    {
        //        Debug.Log("Received: " + webRequest.downloadHandler.text);
        //        OnSuccessCallback?.Invoke(webRequest.downloadHandler.text);
        //    }
        //}

        //https://docs.unity.cn/cn/current/ScriptReference/Networking.UnityWebRequest.Post.html
        byte[] databyte = System.Text.Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest webRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(databyte);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            //webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
                OnFailCallback?.Invoke();
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                OnSuccessCallback?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
}
