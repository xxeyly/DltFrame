using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;


public class ClientSocketFrameComponent : FrameComponent,IHeartbeat
{
    public static ClientSocketFrameComponent Instance;
    [LabelText("自动开启连接")] public bool autoConnect = true;
    [LabelText("是否连接")] public bool isConnect;
    private Socket _clientSocket;
    private Message _msg = new Message();
    [LabelText("IP地址")] [SerializeField] private string ip = "127.0.0.1";
    [LabelText("端口")] [SerializeField] private int port = 828;
    [LabelText("反射数据")] private Dictionary<RequestCode, List<MethodInfoData>> _requestCodes = new Dictionary<RequestCode, List<MethodInfoData>>();
    private static Queue<RequestData> _requestData = new Queue<RequestData>();

    public override void FrameInitComponent()
    {
        Instance = this;
        ReflectionRequestCode();
        if (autoConnect)
        {
            StartConnect();
        }
    }

    [LabelText("开启连接")]
    public void StartConnect()
    {
        Debug.Log("开启连接");
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            _clientSocket.Connect(ip, port);
            Receive();
        }
        catch (Exception e)
        {
            Debug.Log("连接失败...");
        }
    }

    [LabelText("重新连接")]
    public void ReConnect()
    {
        _clientSocket.Close();
        StartConnect();
    }

    private void Update()
    {
        if (_requestData.Count > 0)
        {
            RequestData requestData = _requestData.Dequeue();
            ExecuteReflection(requestData.requestCode, requestData.data);
        }
    }

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

    public override void FrameSceneInitComponent()
    {
    }

    public override void FrameSceneEndComponent()
    {
    }

    public override void FrameEndComponent()
    {
        if (_clientSocket.Connected)
        {
            Send(RequestCode.Disconnect, "主动断开连接");
            _clientSocket.Close();
        }
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    private void Receive()
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

        Receive();
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