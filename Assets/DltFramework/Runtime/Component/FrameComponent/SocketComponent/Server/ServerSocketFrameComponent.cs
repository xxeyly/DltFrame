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
    private string ip = "192.168.3.3";
    private int port = 828;
    private int udpPort = 829;

    //反射请求代码
    private Dictionary<int, List<MethodInfoData>> _requestCodes = new Dictionary<int, List<MethodInfoData>>();

    //Udp
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
        FrameData frameData = ProtobufTool.DeserializeFromByteArray<FrameData>(receivedBytes);
        // Console.WriteLine("客户端:" + frameData.ClientToken + ":" + frameData.FrameIndex + ":" + frameData.DataType);

        UdpExecuteReflection(frameData, remoteIpEndPoint);
        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    private void UdpExecuteReflection(FrameData frameData, IPEndPoint ipEndPoint)
    {
        Console.WriteLine("客户端:" + frameData.FrameIndex);
        Console.WriteLine("服务器:" + ServerMapManager.GetServerMap(ClientSocketManager.GetClientSocket(frameData.ClientToken)).mapFrameIndex);
        ClientSocket clientSocket = ClientSocketManager.GetClientSocket(frameData.ClientToken);
        clientSocket.SetUdpClient(ipEndPoint);
        clientSocket.FrameIndex = frameData.FrameIndex;

        ServerMap serverMap = ServerMapManager.GetServerMap(clientSocket);
        if (frameData.FrameIndex != ServerMapManager.GetServerMap(ClientSocketManager.GetClientSocket(frameData.ClientToken)).mapFrameIndex)
        {
            Console.WriteLine("超时:丢弃帧数据");
            //向客户端发送最新帧数据
        }
        else
        {
            Console.WriteLine("帧记录:");
            serverMap.AddFrameData(clientSocket, frameData);
        }
    }

    /// <summary>
    /// 执行反射逻辑
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    /// <param name="clientSocket"></param>
    public void ExecuteReflection(int requestCode, byte[] data, ClientSocket clientSocket)
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