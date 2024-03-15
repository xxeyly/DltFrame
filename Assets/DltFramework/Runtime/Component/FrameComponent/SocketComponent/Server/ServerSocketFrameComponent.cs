using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using LitJson;

public class ServerSocketFrameComponent
{
    private Socket _serverSocket;
    private string ip = "192.168.7.32";
    private int port = 828;
    private int udpPort = 829;
    private Dictionary<int, List<MethodInfoData>> _requestCodes = new Dictionary<int, List<MethodInfoData>>();
    private UdpClient _udpClient;

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
        // HeartBeat.CreateHeartBeat();
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
        clientSocket.TcpSend(RequestCode.ConnectSuccessFully, "1");
        // HeartBeat.AddClientSocket(clientSocket);
        _serverSocket.BeginAccept(AcceptCallBack, null);
        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    private void UdpReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] receivedBytes = _udpClient.EndReceive(ar, ref remoteIpEndPoint);
        // Console.WriteLine(remoteIpEndPoint);
        Message.UdpReadMessage(receivedBytes, remoteIpEndPoint, UdpExecuteReflection);
        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    private void UdpExecuteReflection(int clientFrameIndex, string content, IPEndPoint ipEndPoint)
    {
        Console.WriteLine("客户端:" + clientFrameIndex + ":" + content);
        Console.WriteLine("服务器:" + ClientFrameSync.serverFrameIndex);
        // Console.WriteLine(content);
        // Console.WriteLine(ipEndPoint);

        FrameRecordData frameRecordData = JsonMapper.ToObject<FrameRecordData>(content);
        ClientSocket clientSocket = ClientSocketManager.GetClientSocket(frameRecordData.id);
        //客户端发来的会比服务器的多一帧,表示客户端已经同步过上一帧了
        clientSocket.SetUdpClient(ipEndPoint);
        clientSocket.FrameIndex = clientFrameIndex;
        //客户端和服务器的帧是时间是一样的
        //服务器第1帧的时候向客户端发送数据
        //客户端第1帧的时候也接收到了服务器第1帧的数据(最理想的状态)
        //客户端第2帧向服务器发送数据,数据帧是第一帧的帧序列
        //服务器接收的时候是第2帧
        //所以要消除这个1帧的差异

        
        if (clientFrameIndex + 1 < ClientFrameSync.serverFrameIndex)
        {
            Console.WriteLine("超时:丢弃帧数据");
            //向客户端发送最新帧数据
        }
        else
        {
            Console.WriteLine("帧记录:" + (clientFrameIndex + 1) + ":" + content);
            //记录下一帧数据
            ServerFrameSync.RecordFrameSyncData(clientFrameIndex + 1, frameRecordData);
            // Console.WriteLine(frameRecordData.id);
            //记录地址
        }
    }

    /// <summary>
    /// 执行反射逻辑
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    /// <param name="clientSocket"></param>
    public void ExecuteReflection(int requestCode, string data, ClientSocket clientSocket)
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
                        int requestCode = ((AddRequestCodeAttribute)customAttribute).RequestCode;
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