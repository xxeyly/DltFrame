using System;
using UnityEngine;

public class HeartbeatDetection
{
    public void StartHeartbeatDetection()
    {
        UniTaskFrameComponent.Instance.AddSceneTask("心跳检测", 1, 0, null, null, OnHeartbeatDetection);
    }

    private void OnHeartbeatDetection()
    {
        //发送心跳包
        ClientSocketFrameComponent.Instance.Send(RequestCode.HeartbeatPacket, "0");
    }

    [AddRequestCode(RequestCode.HeartbeatPacket)]
    public void OnHeartbeat(string data)
    {
        Debug.Log("接收到服务器的心跳包:" + data);
    }
}