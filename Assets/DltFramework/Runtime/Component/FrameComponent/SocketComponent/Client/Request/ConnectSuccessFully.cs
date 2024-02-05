using UnityEngine;

public class ConnectSuccessFully
{
    [AddRequestCode(RequestCode.None, RequestType.Client)]
    public void OnConnectSuccessFully(string data)
    {
        HeartbeatDetection heartbeatDetection = new HeartbeatDetection();
        heartbeatDetection.StartHeartbeatDetection();
    }
}