using UnityEngine;

public class DisconnectReconnect : IHeartbeat
{
    public void HeartbeatAbnormal(int remainderCount)
    {
        Debug.Log("重新发起请求");
        ClientSocketFrameComponent.Instance.ReConnect();
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