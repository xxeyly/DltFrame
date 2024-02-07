using UnityEngine;

public class ConnectSuccessFully
{
    [AddRequestCode(RequestCode.None, RequestType.Client)]
    public void OnConnectSuccessFully(string data)
    {
        Debug.Log("连接成功...");
        //发送心跳包到服务器
        Heartbeat.StartHeartbeatDetection();
    }
}