using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AwaitTaskLearn : MonoBehaviour
{

    string _pngUrl = "http://192.168.1.243:8082/basketball/my_team_logo/dh6.png";
    string _jsonUrl = "http://192.168.1.243:8082/basketball/theme_activity/configure/ThemeActivityConfig_1.json";
    HttpClient _httpClient;

    private void Awake()
    {
        _httpClient = new HttpClient();


        var t1 = Task<int>.Run(() => { return 1; });
        var t2 = Task.Run(() => { return 1; });


    }

    /// <summary>
    /// 异步空函数
    /// </summary>
    public async void AsyncLoadStr()
    {
        string contents = await _httpClient.GetStringAsync(_jsonUrl);
        Debug.Log(contents);
    }

    /// <summary>
    /// 异步函数 且带了返回值
    /// </summary>
    /// <returns></returns>
    public async Task<int> AsyncLoadStrAndReturnStr()
    {
        string contents = await _httpClient.GetStringAsync(_jsonUrl);
        Debug.Log(contents);
        return contents.Length;
    }


    private IEnumerator enumerator()
    {
        yield return new WaitForUpdate();
    }
}



public class MyClass : IAsyncResult
{
    public object AsyncState => throw new NotImplementedException();

    public WaitHandle AsyncWaitHandle => throw new NotImplementedException();

    public bool CompletedSynchronously => throw new NotImplementedException();

    public bool IsCompleted => throw new NotImplementedException();
}