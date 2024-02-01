using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

public class ServerSocketFrameComponent
{
    private Socket _serverSocket;
    private string ip = "127.0.0.1";
    private int port = 828;
    private Message _msg = new Message();
    private Dictionary<RequestCode, List<MethodInfoData>> _requestCodes = new Dictionary<RequestCode, List<MethodInfoData>>();

    public void StartServer()
    {
        ReflectionRequestCode();
        Console.WriteLine(_requestCodes.Count);
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
    }

    private void AcceptCallBack(IAsyncResult ar)
    {
        ClientSocket clientSocket = new ClientSocket(this, _serverSocket.EndAccept(ar));
        Console.WriteLine(clientSocket.socket.LocalEndPoint + ":加入系统...");
        clientSocket.socket.Send(_msg.PackData(RequestCode.None, "连接成功"));
        _serverSocket.BeginAccept(AcceptCallBack, _serverSocket);
    }

    /// <summary>
    /// 执行反射逻辑
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    public void ExecuteReflection(RequestCode requestCode, string data)
    {
        if (_requestCodes.ContainsKey(requestCode))
        {
            foreach (MethodInfoData methodInfoData in _requestCodes[requestCode])
            {
                methodInfoData.methodInfo.Invoke(Activator.CreateInstance(methodInfoData.type), new object[] { data });
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