using DltFramework;
using UnityEngine;

public class DisconnectReconnectDemo : SceneComponentInit, IDisconnectReconnect
{
    public void DisconnectReconnect(int remainderCount)
    {
        Debug.Log("剩余次数：" + remainderCount + "");
    }

    public void OnDisconnectReconnectSuccess()
    {
        Debug.Log("断线重连成功");
    }

    public void OnDisconnectReconnectFail()
    {
        Debug.Log("断线重连失败");
    }

    public override void StartComponent()
    {
    }

    public override void InitComponent()
    {
        
    }
   
}