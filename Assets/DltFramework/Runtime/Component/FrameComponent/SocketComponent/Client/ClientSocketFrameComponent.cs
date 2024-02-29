using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Cysharp.Threading.Tasks;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;


public class ClientSocketFrameComponent : FrameComponent, IHeartbeat, IFrameSync
{
    public static ClientSocketFrameComponent Instance;
    [LabelText("自动开启连接")] public bool autoConnect = true;
    [LabelText("是否连接")] public bool isConnect;
    private Socket _clientSocket;
    private UdpClient _udpClient;
    private Message _msg = new Message();
    [LabelText("IP地址")] [SerializeField] private string ip = "127.0.0.1";
    [LabelText("端口")] [SerializeField] private int port = 828;
    [LabelText("端口")] [SerializeField] private int udpPort = 829;
    [LabelText("连接码")] public int Token = 0;

    [LabelText("帧发送间隔,单位毫秒")] [SerializeField]
    public int frameInterval = 60;

    [LabelText("反射数据")] private static Dictionary<RequestCode, List<MethodInfoData>> _requestCodes = new Dictionary<RequestCode, List<MethodInfoData>>();
    private static Queue<RequestData> _requestData = new Queue<RequestData>();

    public override async void FrameInitComponent()
    {
        Instance = this;
        ReflectionRequestCode();
        if (autoConnect)
        {
            await StartConnect();
        }
    }

    #region 反射请求数据

    /// <summary>
    /// 反射请求数据
    /// </summary>
    private void ReflectionRequestCode()
    {
        Assembly _assembly = Assembly.Load("Assembly-CSharp");
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
                        if (requestType == RequestType.Client)
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

    #endregion


    public void FrameSync()
    {
    }

    [LabelText("开启连接")]
    public async UniTask StartConnect()
    {
        //开启快照
        Snapshot.StartSnapshot();
        Debug.Log("开启连接");
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await _clientSocket.ConnectAsync(ip, port);
            _udpClient = new UdpClient();
            _udpClient.Connect(ip, udpPort);
            _udpClient.BeginReceive(UdpReceiveCallback, null);
            TcpReceive();
        }
        catch (Exception e)
        {
            Debug.Log("连接失败..." + e.ToString());
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            // await StartConnect();
        }
    }

    private void UdpReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = _udpClient.EndReceive(ar, ref ipEndPoint);
        Message.UdpReadMessage(data, UdpExecuteReflection);

        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    //帧同步解析
    private void UdpExecuteReflection(int frameIndex, string data)
    {
        Debug.Log(frameIndex + ":" + data);
        ClientFrameSync.ExecuteReflection(frameIndex, data);
    }

    [LabelText("重新连接")]
    public void ReConnect()
    {
        _clientSocket.Close();
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            _clientSocket.Connect(ip, port);
            TcpReceive();
        }
        catch (Exception e)
        {
            Debug.Log("连接失败...");
        }
    }

    private void Update()
    {
        if (_requestData.Count > 0)
        {
            RequestData requestData = _requestData.Dequeue();
            ExecuteReflection(requestData.requestCode, requestData.data);
        }

        ClientFrameSync.Update();
    }


    public override void FrameSceneInitComponent()
    {
    }

    public override void FrameSceneEndComponent()
    {
    }

    public override void FrameEndComponent()
    {
        if (_clientSocket != null && _clientSocket.Connected)
        {
            Send(RequestCode.Disconnect, "主动断开连接");
            _clientSocket.Close();
        }
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    private void TcpReceive()
    {
        _clientSocket.BeginReceive(_msg.Data, _msg.StartIndex, _msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }


    /// <summary>
    /// 异步接受数据回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallback(IAsyncResult ar)
    {
        if (_clientSocket == null || _clientSocket.Connected == false || ar == null)
        {
            return;
        }

        try
        {
            int count = _clientSocket.EndReceive(ar);
            if (count > 0)
            {
                _msg.ReadMessage(count, AddToExecuteReflection);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        TcpReceive();
    }

    //执行反射逻辑
    private void AddToExecuteReflection(RequestCode requestCode, string data)
    {
        _requestData.Enqueue(new RequestData(requestCode, data));
    }

    //执行反射逻辑
    private void ExecuteReflection(RequestCode requestCode, string data)
    {
        if (_requestCodes.ContainsKey(requestCode))
        {
            foreach (MethodInfoData methodInfoData in _requestCodes[requestCode])
            {
                methodInfoData.methodInfo.Invoke(methodInfoData.obj, new object[] { data });
            }
        }
        else
        {
            Debug.LogError("未找到对应的RequestCode:" + requestCode);
        }
    }


    //发送消息
    public void Send(RequestCode requestCode, string data)
    {
        byte[] bytes = _msg.PackData(requestCode, data);
        _clientSocket.Send(bytes);
    }

    public void UdpSend(FrameRecordData frameRecordData, bool IsForecast = true)
    {
        ClientFrameSync.AddFrameRecordData(frameRecordData, IsForecast);
    }

    public void UdpStartSend(FrameRecordData frameRecordData, bool IsForecast = true)
    {
        //记录当前操作
        FrameRecord.ClientRecordFrameSyncData(frameRecordData, IsForecast);
        //客户端发送的要比服务器快一帧
        byte[] bytes = _msg.UdpPackData(FrameRecord.frameIndex + 1, JsonUtil.ToJson(frameRecordData));
        _udpClient.Send(bytes, bytes.Length);
    }

    public void HeartbeatAbnormal(int remainderCount)
    {
        ReConnect();
    }

    public void HeartbeatRestoreNormal()
    {
    }

    public void HeartbeatStop()
    {
    }

    public void HeartbeatNormal()
    {
    }
}