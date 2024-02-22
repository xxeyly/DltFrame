using System;
using DltFramework;
using UnityEngine;

public class ConnectSuccessFully
{
    [AddRequestCode(RequestCode.ConnectCode, RequestType.Client)]
    public void OnConnectSuccessFully(string data)
    {
        Debug.Log("连接成功...");
        //开启帧同步
        ClientFrameSync.StartFrameSync(ClientSocketFrameComponent.Instance.frameInterval);
        //保存连接码
        ClientSocketFrameComponent.Instance.connectCode = Convert.ToInt32(data);
        ClientSocketFrameComponent.Instance.SendUdp(RequestCode.UdpInit, ClientSocketFrameComponent.Instance.connectCode, "0");
    }
}