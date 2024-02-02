using UnityEngine;

public class ConnectSuccessfully
{
    [AddRequestCode(RequestCode.None)]
    public void OnConnectSuccessfully(string data)
    {
        HeartbeatDetection heartbeatDetection = new HeartbeatDetection();
        heartbeatDetection.StartHeartbeatDetection();
    }
}