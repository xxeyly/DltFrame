using DltFramework;
using UnityEngine;

public class HeartbeatDemo : SceneComponentInit, IHeartbeat
{
    public void HeartbeatAbnormal(int remainderCount)
    {
        Debug.Log("心跳异常：" + remainderCount + "");
    }

    public void HeartbeatRestoreNormal()
    {
        Debug.Log("心跳恢复正常");
    }

    public void HeartbeatStop()
    {
        Debug.Log("心跳停止");
    }

    public void HeartbeatNormal()
    {
        Debug.Log("心跳正常");
    }

    public override void StartComponent()
    {
    }

    public override void InitComponent()
    {
    }
}