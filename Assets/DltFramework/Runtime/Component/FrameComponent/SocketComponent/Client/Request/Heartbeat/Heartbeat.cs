using System;
using System.Collections.Generic;
using UnityEngine;

public class Heartbeat
{
    //断线重连默认次数
    private static int disconnectReconnectDefaultCount = 30;

    //当前断线重连次数
    private static int disconnectReconnectCount;

    //心跳异常提示时间
    private static int heartBeatAbnormalTime = 5;
    private static List<IHeartbeat> Iheartbeats;

    public static void StartHeartbeatDetection()
    {
        disconnectReconnectCount = disconnectReconnectDefaultCount;
        UniTaskFrameComponent.Instance.AddSceneTask("心跳检测", 1, 0, null, null, OnHeartbeatDetection);
    }

    public static void StopHeartbeatDetection()
    {
        Debug.Log("关闭心跳检测");
        UniTaskFrameComponent.Instance.RemoveSceneTask("心跳检测");
    }

    private static void OnHeartbeatDetection()
    {
        Debug.Log("发送心跳包到服务器");
        //发送心跳包
        ClientSocketFrameComponent.Instance.Send(RequestCode.HeartbeatPacket, "0");
    }

    public static void StartDisconnectReconnect()
    {
        Iheartbeats = AotGlobal.GetAllObjectsInScene<IHeartbeat>();
        UniTaskFrameComponent.Instance.AddSceneTask("断线重连", 1, 0, null, null, OnDisconnectReconnect);
    }

    public static void OnDisconnectReconnect()
    {
        if (disconnectReconnectCount <= 0)
        {
            foreach (IHeartbeat iheartbeat in Iheartbeats)
            {
                //心跳停止
                iheartbeat.HeartbeatStop();
            }

            //关闭心跳检测
            StopHeartbeatDetection();
        }
        else
        {
            disconnectReconnectCount--;
            if (disconnectReconnectCount < disconnectReconnectDefaultCount - heartBeatAbnormalTime)
            {
                foreach (IHeartbeat iheartbeat in Iheartbeats)
                {
                    //心跳异常
                    iheartbeat.HeartbeatAbnormal(disconnectReconnectCount);
                }
                
            }
            else
            {
                foreach (IHeartbeat iheartbeat in Iheartbeats)
                {
                    //心跳正常
                    iheartbeat.HeartbeatNormal();
                }
            }
        }
    }

    [AddRequestCode(RequestCode.HeartbeatPacket, RequestType.Client)]
    public void OnHeartbeat(string data)
    {
        if (disconnectReconnectCount < disconnectReconnectDefaultCount - heartBeatAbnormalTime)
        {
            Debug.Log("心跳恢复");
        }

        disconnectReconnectCount = disconnectReconnectDefaultCount;
    }
}