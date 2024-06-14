using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Cysharp.Threading.Tasks;
using DltFramework;
using Google.Protobuf;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


public class ClientSocketFrameComponent : FrameComponent, IHeartbeat
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
    [LabelText("Token")] public int Token;

    [HorizontalGroup("Room")] [LabelText("房间ID")]
    public int roomId;

    //TCP
    [LabelText("反射数据")] private static Dictionary<int, List<MethodInfoData>> _requestCodes = new Dictionary<int, List<MethodInfoData>>();
    private static Queue<RequestData> tcpRequestData = new Queue<RequestData>();

    //UDP
    private static Queue<FrameDataGroup> udpRequestData = new Queue<FrameDataGroup>();

    public delegate void OnUpdate();

    public OnUpdate onUpdate;

    public override async void FrameInitComponent()
    {
        Instance = this;
        ReflectionRequestCode();
        FrameLogic.Init();
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
                        int requestCode = ((AddRequestCodeAttribute)customAttribute).RequestCode;
                        RequestType requestType = ((AddRequestCodeAttribute)customAttribute).RequestType;
                        //参数长度
                        int parameterTypeLength = methodInfo.GetParameters().Length;
                        //参数类型个数
                        if (parameterTypeLength != 1)
                        {
                            continue;
                        }

                        //参数类型
                        if (methodInfo.GetParameters()[0].ParameterType != typeof(byte[]))
                        {
                            continue;
                        }

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


    [LabelText("开启连接")]
    public async UniTask StartConnect()
    {
        //开启快照
        /*Snapshot.StartSnapshot();
        ClientFrameSync.ClientFrameSyncInit();*/
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
            Debug.Log("连接失败..." + e);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }

    #region UDP消息解析

    private void UdpReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = _udpClient.EndReceive(ar, ref ipEndPoint);
        FrameDataGroupList frameDataGroupList = ProtobufTool.DeserializeFromByteArray<FrameDataGroupList>(data);
        UdpExecuteReflection(frameDataGroupList);
        _udpClient.BeginReceive(UdpReceiveCallback, null);
    }

    //帧同步异步接收,不能在主线程中执行
    private void UdpExecuteReflection(FrameDataGroupList frameDataGroupList)
    {
        //对接收的数据进行排查.判断是否符合接收规则
        int serverFrameMinIndex = frameDataGroupList.FrameDataGroup[0].FrameIndex;
        foreach (FrameDataGroup frameDataGroup in frameDataGroupList.FrameDataGroup)
        {
            if (frameDataGroup.FrameIndex < serverFrameMinIndex)
            {
                serverFrameMinIndex = frameDataGroup.FrameIndex;
            }
        }

        //正确的下一帧
        if (serverFrameMinIndex - ClientMapManager.GetClientMap(roomId).clientFrameIndex != 1)
        {
            return;
        }


        // Debug.Log("客户端接收帧数量:" + frameDataGroupList.FrameDataGroup.Count);
        foreach (FrameDataGroup frameDataGroup in frameDataGroupList.FrameDataGroup)
        {
            udpRequestData.Enqueue(frameDataGroup);
        }
    }


    //帧同步解析
    public void UdpExecuteReflection(FrameDataGroup frameDataGroup)
    {
        Debug.Log("客户端接收帧:" + frameDataGroup.FrameIndex);
        ClientMap clientMap = ClientMapManager.GetClientMap(roomId);
        //更新客户端帧
        clientMap.clientFrameIndex = frameDataGroup.FrameIndex;
        //回放模式,加入回放列表
        if (RecordReplays.isReplay)
        {
            RecordReplays.AddFrameData(new List<FrameData>(frameDataGroup.FrameData));
        }
        else
        {
            //直接执行
            //客户端更新当前帧
            foreach (FrameData frameData in frameDataGroup.FrameData)
            {
                FrameLogic.ExecuteReflection(frameData.DataType, frameData.Data.ToByteArray());
            }
        }
    }

    #endregion

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
            Debug.Log("连接失败..." + e);
        }
    }

    private void Update()
    {
        #region TCP UDP 消息主线程解析

        if (tcpRequestData.Count > 0)
        {
            RequestData requestData = tcpRequestData.Dequeue();
            ExecuteReflection(requestData.requestCode, requestData.data);
        }

        if (udpRequestData.Count > 0)
        {
            FrameDataGroup frameDataGroup = udpRequestData.Dequeue();
            UdpExecuteReflection(frameDataGroup);
        }

        #endregion

        onUpdate?.Invoke();

        // ClientFrameSync.Update();
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

    #region TCP消息解析

    /// <summary>
    /// 接收消息
    /// </summary>
    private void TcpReceive()
    {
        _clientSocket.BeginReceive(_msg.Data, _msg.StoredSize, _msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
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

        int count = _clientSocket.EndReceive(ar);
        if (count > 0)
        {
            _msg.ReadMessage(count, AddToExecuteReflection);
        }
        /*try
        {
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }*/

        TcpReceive();
    }

    //执行反射逻辑
    private void AddToExecuteReflection(int requestCode, byte[] data)
    {
        tcpRequestData.Enqueue(new RequestData(requestCode, data));
    }

    //执行反射逻辑
    private void ExecuteReflection(int requestCode, byte[] data)
    {
        if (_requestCodes.ContainsKey(requestCode))
        {
            foreach (MethodInfoData methodInfoData in _requestCodes[requestCode])
            {
                try
                {
                    methodInfoData.methodInfo.Invoke(methodInfoData.obj, new object[] { data });
                }
                catch (Exception e)
                {
                    Debug.LogError("请求码:" + requestCode + "异常" + methodInfoData.obj.ToString() + methodInfoData.methodInfo.Name + ":" + e);
                }
            }
        }
        else
        {
            Debug.LogError("未找到对应的RequestCode:" + requestCode);
        }
    }

    #endregion

    #region TCP消息发送

    //发送消息
    public void Send(int requestCode, string data)
    {
        byte[] bytes = _msg.PackData(requestCode, data);
        _clientSocket.Send(bytes);
    }

    public void Send(int requestCode, int data)
    {
        Send(requestCode, data.ToString());
    }


    #endregion

    #region UDP消息发送

    public void UdpSend(byte[] bytes)
    {
        _udpClient.Send(bytes, bytes.Length);
    }

    #endregion


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