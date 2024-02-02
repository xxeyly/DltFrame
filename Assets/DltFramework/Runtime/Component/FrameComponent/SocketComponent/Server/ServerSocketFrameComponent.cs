using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

public class ServerSocketFrameComponent
{
    private Socket _serverSocket;
    private string ip = "192.168.3.3";
    private int port = 828;
    public HeartBeat heartBeat = new HeartBeat();
    private ClientSocketManager clientSocketManager;
    private Dictionary<RequestCode, List<MethodInfoData>> _requestCodes = new Dictionary<RequestCode, List<MethodInfoData>>();

    private bool isInit = false;

    public void StartServer()
    {
        //解析服务器代码
        ReflectionRequestCode();

        //开启服务器
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Ip地址绑定
        var ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        //服务器与Ip地址绑定
        _serverSocket.Bind(ipEndPoint);
        //设定监听数量
        _serverSocket.Listen(0);
        Console.WriteLine("服务器开启成功...");
        //异步加载用户
        _serverSocket.BeginAccept(AcceptCallBack, _serverSocket);
        //客户端管理
        clientSocketManager = new ClientSocketManager();
        //心跳包
        heartBeat = new HeartBeat();
        heartBeat.CreateHeartBeat();
    }

    private void AcceptCallBack(IAsyncResult ar)
    {
        ClientSocket clientSocket = new ClientSocket(this, clientSocketManager, heartBeat, _serverSocket.EndAccept(ar));
        clientSocketManager.AddClientSocket(clientSocket);
        heartBeat.AddClientSocket(clientSocket);
        Console.WriteLine(clientSocket.socket.RemoteEndPoint + ":加入系统...");
        clientSocket.Send(RequestCode.None, "服务器登录成功");
        _serverSocket.BeginAccept(AcceptCallBack, _serverSocket);
    }

    /// <summary>
    /// 执行反射逻辑
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    /// <param name="clientSocket"></param>
    public void ExecuteReflection(RequestCode requestCode, string data, ClientSocket clientSocket)
    {
        if (_requestCodes.ContainsKey(requestCode))
        {
            foreach (MethodInfoData methodInfoData in _requestCodes[requestCode])
            {
                methodInfoData.methodInfo.Invoke(Activator.CreateInstance(methodInfoData.type), new object[] { data, clientSocket, });
            }
        }
        else
        {
            Console.WriteLine(requestCode + "没有对应的反射方法");
        }
    }

    public void Close()
    {
        _serverSocket.Close();
    }

    /// <summary>
    /// 反射请求数据
    /// </summary>
    private void ReflectionRequestCode()
    {
        Assembly _assembly = Assembly.GetExecutingAssembly();
        foreach (Type type in _assembly.GetTypes())
        {
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                foreach (Attribute customAttribute in methodInfo.GetCustomAttributes())
                {
                    if (customAttribute is AddRequestCodeAttribute)
                    {
                        RequestCode requestCode = ((AddRequestCodeAttribute)customAttribute).RequestCode;
                        if (!_requestCodes.ContainsKey(requestCode))
                        {
                            _requestCodes.Add(requestCode, new List<MethodInfoData>());
                        }

                        _requestCodes[requestCode].Add(new MethodInfoData()
                        {
                            type = type, methodInfo = methodInfo
                        });
                    }
                }
            }
        }
    }
}