using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisconnectReconnect
{
    //断线重连默认次数
    private int disconnectReconnectDefaultCount = 15;

    //当前断线重连次数
    private int disconnectReconnectCount = 15;

    //剩余多少时间开始断线重连
    private int remainderTime = 5;

    private List<IDisconnectReconnect> IIDisconnectReconnects;

    //是否断线重连
    private bool isDisconnectReconnect = false;

    public void StartDisconnectReconnect()
    {
        IIDisconnectReconnects = AotGlobal.GetAllObjectsInScene<IDisconnectReconnect>();
        UniTaskFrameComponent.Instance.AddSceneTask("断线重连", 1, 0, null, null, OnDisconnectReconnect);
    }

    public void OnDisconnectReconnect()
    {
        if (disconnectReconnectCount <= 0)
        {
            foreach (IDisconnectReconnect iDisconnectReconnect in IIDisconnectReconnects)
            {
                iDisconnectReconnect.OnDisconnectReconnectFail();
            }
        }
        else
        {
            disconnectReconnectCount--;
            if (disconnectReconnectCount <= remainderTime)
            {
                isDisconnectReconnect = true;
                foreach (IDisconnectReconnect iDisconnectReconnect in IIDisconnectReconnects)
                {
                    iDisconnectReconnect.DisconnectReconnect(disconnectReconnectCount);
                }
            }
        }
    }

    [AddRequestCode(RequestCode.HeartbeatPacket, RequestType.Client)]
    public void OnHeartbeat(string data)
    {
        //如果是已经短线了
        if (isDisconnectReconnect)
        {
            foreach (IDisconnectReconnect iDisconnectReconnect in IIDisconnectReconnects)
            {
                iDisconnectReconnect.OnDisconnectReconnectSuccess();
            }
        }

        disconnectReconnectCount = disconnectReconnectDefaultCount;
    }
}