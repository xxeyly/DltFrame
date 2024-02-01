using System;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class ServerSocketFrameComponent
{
    private Socket _serverSocket;
    private string ip = "127.0.0.1";
    private int port = 828;
    private Message _msg = new Message();

    public void StartServer()
    {
        //开启服务器
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Ip地址绑定
        var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        //服务器与Ip地址绑定
        _serverSocket.Bind(ipEndPoint);
        //设定监听数量
        _serverSocket.Listen(0);
        Debug.Log("服务器开启成功...");
        //异步加载用户
        _serverSocket.BeginAccept(AcceptCallBack, _serverSocket);
    }

    private void AcceptCallBack(IAsyncResult ar)
    {
        //客户端Socket
        Socket clientSocket = _serverSocket.EndAccept(ar);
        //用户绑定服务器和客户端的Socket
        Debug.Log(clientSocket.LocalEndPoint + ":加入系统...");
        _serverSocket.BeginAccept(AcceptCallBack, _serverSocket);
        byte[] bytes = _msg.PackData(RequestCode.None, "123222");

        clientSocket.Send(bytes);
    }

    public void Close()
    {
        _serverSocket.Close();
    }
}