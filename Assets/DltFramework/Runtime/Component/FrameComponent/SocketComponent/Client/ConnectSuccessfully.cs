using UnityEngine;

public class ConnectSuccessfully
{
    [AddRequestCode(RequestCode.None)]
    public void OnConnectSuccessfully(string data)
    {
        Debug.Log(data);
        HeartbeatDetection heartbeatDetection = new HeartbeatDetection();
        heartbeatDetection.StartHeartbeatDetection();
    }
}