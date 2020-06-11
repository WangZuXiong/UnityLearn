using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/*
Socket基于TCP协议，连接过程一定进行三次握手，数据安全达到
Socket定义服务端：确定ip地址跟端口号
Socket定义客户端：可以连接服务端
*/

/// <summary>
/// 创建服务端和客户端进行连接
/// </summary>
public class ServerSocket1
{
    private const int PORT = 7777;
    private const string IP_STRING = "192.168.3.137";

    public void Listen()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(IP_STRING), PORT);
        socket.Bind(iPEndPoint);
        socket.Listen(0);
        Debug.LogError("等待连接. . .");
    }

}


/// <summary>
/// 服务端和客户端进行一对一通信 服务端只能接收 客户端只能发送
/// </summary>
public class ServerSocket2
{
    private Socket _socket;
    private Socket _user;
    private const int PORT = 7777;
    private const string IP_STRING = "192.168.3.137";

    public ServerSocket2()
    {
        //套接字
        //参数一：地址族 确定地址类型 IPV4
        //参数二：传输数据类型 字节流Stream
        //参数三：通信协议 TCP
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //服务端：给套接字绑定ip和端口号
        //端点 ip+端口号
        IPEndPoint point = new IPEndPoint(IPAddress.Parse(IP_STRING), PORT);
        //套接字绑定端点 形成服务端点
        _socket.Bind(point);
        _socket.Listen(0);
        _user = _socket.Accept();//接受连接客户端套接字
    }

    public void ReceiveMessage()
    {
        while (true)
        {
            byte[] bytes = new byte[1024];
            //接受当前客户端的数据
            int count = _user.Receive(bytes);
            //len实际接受的字节数
            string value = Encoding.Default.GetString(bytes, 0, count);
            Debug.Log(value);
        }
    }
}