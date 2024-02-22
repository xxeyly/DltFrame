using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

public class ServerSocketFrameComponent
{
    private Socket _serverSocket;
    private string ip = "192.168.7.32";
    private int port = 828;
    private int udpPort = 829;
    private Dictionary<RequestCode, List<MethodInfoData>> _requestCodes = new Dictionary<RequestCode, List<MethodInfoData>>();
    private UdpClient _udpClient;
    private IPEndPoint remoteIpEndPoint;

    private bool isInit = false;

    public void StartServer()
    {
        //解析服务器代码
        ReflectionRequestCode();

        //开启服务器
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Ip地址绑定
        IPEndPoint tcpIpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        //服务器与Ip地址绑定
        _serverSocket.Bind(tcpIpEndPoint);
        //设定监听数量
        _serverSocket.Listen(0);
        _serverSocket.BeginAccept(AcceptCallBack, _serverSocket);
        Console.WriteLine("Tcp服务器开启成功...");
        //异步加载用户
        _udpClient = new UdpClient(udpPort);
        Console.WriteLine("Udp服务器开启成功...");
        //心跳包
        HeartBeat.CreateHeartBeat();
        //帧同步
        ServerFrameSync.CreateFrameSync();
    }

    private void AcceptCallBack(IAsyncResult ar)
    {
        ClientSocket clientSocket = new ClientSocket();
        clientSocket.SetServer(this);
        clientSocket.SetTcpClient(_serverSocket.EndAccept(ar));
        clientSocket.SetUdpClient(_udpClient);
        //添加客户端Socket
        ClientSocketManager.AddClientSocket(clientSocket);
        //心跳包
        Console.WriteLine(clientSocket.socket.RemoteEndPoint + ":加入系统...");
        clientSocket.TcpSend(RequestCode.ConnectCode, clientSocket.clientSocketId.ToString());
        HeartBeat.AddClientSocket(clientSocket);
        _serverSocket.BeginAccept(AcceptCallBack, null);
        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    private void UdpReceiveCallback(IAsyncResult ar)
    {
        remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] receivedBytes = _udpClient.EndReceive(ar, ref remoteIpEndPoint);
        Message.UdpReadMessage(receivedBytes, remoteIpEndPoint, UdpExecuteReflection);
        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    private void UdpExecuteReflection(RequestCode requestCode, int connectCode, string data, IPEndPoint remoteIpEndPoint)
    {
        // Console.WriteLine(requestCode + " " + data + " " + remoteIpEndPoint);
        ClientSocket clientSocket = ClientSocketManager.GetClientSocket(connectCode);
        clientSocket.SetUdpClient(remoteIpEndPoint);
        clientSocket.ExecuteReflection(requestCode, data);
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
                methodInfoData.methodInfo.Invoke(methodInfoData.obj, new object[] { data, clientSocket, });
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
                        RequestType requestType = ((AddRequestCodeAttribute)customAttribute).RequestType;
                        if (requestType == RequestType.Server)
                        {
                            if (!_requestCodes.ContainsKey(requestCode))
                            {
                                _requestCodes.Add(requestCode, new List<MethodInfoData>());
                            }

                            _requestCodes[requestCode].Add(new MethodInfoData()
                            {
                                obj = Activator.CreateInstance(type), methodInfo = methodInfo
                            });
                        }
                    }
                }
            }
        }
    }
}