using UnityEngine;

public class ConnectSuccessFully
{
    [AddRequestCode(RequestCode.None, RequestType.Client)]
    public void OnConnectSuccessFully(string data)
    {
        //发送心跳包到服务器
        Heartbeat.StartHeartbeatDetection();
        //断线重连检测
        Heartbeat.StartDisconnectReconnect();
    }
}