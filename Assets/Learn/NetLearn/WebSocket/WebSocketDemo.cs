using System;
using UnityEngine;
using WebSocketSharp;
using System.Text;
public class WebSocketDemo : MonoBehaviour
{
    private WebSocket _webSocket;
    private const string URL = "ws://123.207.167.163:9010/ajaxchattest";

    private void Awake()
    {
        Init();
        Connect();

        string message = "123";
        var bytes = Encoding.UTF8.GetBytes(message);
        SendMessage(bytes);
    }

    private void Init()
    {
        _webSocket = new WebSocket(URL);
        _webSocket.OnMessage += OnMessage;
        _webSocket.OnOpen += OnOpen;
        _webSocket.OnClose += OnClose;
        _webSocket.OnError += OnError;
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        throw new Exception(e.Message);
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnOpen(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 接受到消息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMessage(object sender, MessageEventArgs e)
    {
        //do something

        Debug.LogError("OnMessage");
        var message = Encoding.UTF8.GetString(e.RawData);
        Debug.LogError(message);
    }

    /// <summary>
    /// 连接
    /// </summary>
    private void Connect()
    {
        _webSocket.Connect();
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    private void Close()
    {
        _webSocket.Close();
    }

    /// <summary>
    /// 发消息
    /// </summary>
    /// <param name="message"></param>
    private void SendMessage(byte[] message)
    {
        _webSocket.Send(message);
    }
}
