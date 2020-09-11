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


    public static void GetRequest(string uri, Action<string> OnSuccessCallback, Action OnFailCallback)
    {
        SimpleCoroutineManager.Instance.StartCoroutine(_webRequestImpl.GetRequest(uri, OnSuccessCallback, OnFailCallback));
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

    public IEnumerator GetRequest(string uri, Action<string> OnSuccessCallback, Action OnFailCallback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            //webRequest.SetRequestHeader("Content-Type", "application/json");
            //webRequest.SetRequestHeader("Accept", "application/json");
            //webRequest.certificateHandler = new BypassCertificate();

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            //string[] pages = uri.Split('/');
            //int page = pages.Length - 1;

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


    //public class BypassCertificate : CertificateHandler
    //{
    //    protected override bool ValidateCertificate(byte[] certificateData)
    //    {
    //        return true;
    //    }
    //}
}
