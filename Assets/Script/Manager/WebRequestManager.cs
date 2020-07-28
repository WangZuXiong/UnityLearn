using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : MonoBehaviour
{
    private readonly string BASE_URL = "";

    public void Request<T, U>(string cmd, T input, Action<U> callBack)
        where T : BaseInputVO
        where U : BaseOutputVO
    {
        string url = BASE_URL + cmd;

        StartCoroutine(RequestEnumerator(url, input, callBack));
    }


    private IEnumerator RequestEnumerator<T, U>(string url, T input, Action<U> callBack)
          where T : BaseInputVO
          where U : BaseOutputVO
    {

        //请求头
        Dictionary<string, string> headers = new Dictionary<string, string>();
        byte[] postData = new byte[10];

        //Protobuf转化为string
        //string转化为byte数组
        //加密

        WWW www = new WWW(url, postData, headers);
        //UnityWebRequest unityWebRequest = new UnityWebRequest(,)
        yield return www;

        //校验
        //判断
        string text = www.text;
        //将text转化为U
        //callBackAction(u);
        www.Dispose();
    }

}


public class BaseInputVO
{

}

public class BaseOutputVO
{

}

public class CMD
{
    public static readonly string LOGIN = "";
}