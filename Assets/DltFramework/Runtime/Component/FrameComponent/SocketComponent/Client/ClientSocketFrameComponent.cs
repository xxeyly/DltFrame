using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class ClientSocketFrameComponent : FrameComponent
{
    private Socket _clientSocket;
    private ServerSocketFrameComponent serverSocketFrameComponent;
    private Message _msg = new Message();
    [LabelText("IP地址")] [SerializeField] private string ip = "";
    [LabelText("端口")] [SerializeField] private int port = 0;
    [LabelText("反射数据")] private Dictionary<RequestCode, List<MethodInfoData>> _requestCodes = new Dictionary<RequestCode, List<MethodInfoData>>();


    public override void FrameInitComponent()
    {
        ReflectionRequestCode();
        serverSocketFrameComponent = new ServerSocketFrameComponent();
        serverSocketFrameComponent.StartServer();
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _clientSocket.Connect(ip, port);

        Receive();
    }

    /// <summary>
    /// 反射请求数据
    /// </summary>
    private void ReflectionRequestCode()
    {
        Assembly _assembly = Assembly.Load("Assembly-CSharp");
        foreach (Type type in _assembly.GetTypes())
        {
            GenerateListenerComponent.GenerateClassData tempGenerateClassData = new GenerateListenerComponent.GenerateClassData();
            tempGenerateClassData.className = type.Name;
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

    public override void FrameSceneInitComponent()
    {
    }

    public override void FrameSceneEndComponent()
    {
    }

    public override void FrameEndComponent()
    {
        serverSocketFrameComponent.Close();
        _clientSocket.Close();
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
        if (_clientSocket == null || _clientSocket.Connected == false) return;
        int count = _clientSocket.EndReceive(ar);
        if (count > 0)
        {
            _msg.ReadMessage(count, BindingNetworkEvents);
        }

        Receive();
    }

    /// <summary>
    /// 根据事件代码绑定逻辑
    /// 线程中,先放到缓存列表
    /// </summary>
    /// <param name="requestCode">事件码</param>
    /// <param name="data">返回数据</param>
    private void BindingNetworkEvents(RequestCode requestCode, string data)
    {
        foreach (MethodInfoData methodInfoData in _requestCodes[requestCode])
        {
            methodInfoData.methodInfo.Invoke(Activator.CreateInstance(methodInfoData.type), new object[] { data });
        }
    }
}