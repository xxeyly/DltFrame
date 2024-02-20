using System;
using System.Collections.Generic;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Heartbeat
{
    //断线重连默认次数
    private static int disconnectReconnectDefaultCount = 30;

    //当前断线重连次数
    private static int disconnectReconnectCount;

    //心跳异常提示时间
    private static int heartBeatAbnormalTime = 5;

    //心跳接口列表
    private static List<IHeartbeat> Iheartbeats;

    /*//心跳恢复
    private static bool isHeartbeatRestore;*/

    public static void StartHeartbeatDetection()
    {
        // isHeartbeatRestore = true;
        //如果是断线重连,则关闭断线重连
        StopDisconnectReconnect();
        //关掉心跳检测
        StopHeartbeatDetection();
        Iheartbeats = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IHeartbeat>();
        UniTaskFrameComponent.Instance.AddTask("心跳检测", heartBeatAbnormalTime, 0, null, null, OnHeartbeatDetection);
    }

    private static void OnHeartbeatDetection()
    {
        //心跳回访
        StartHeartbeatRequest();
        //向服务器发送网络请求时,会阻塞该线程,所以需要开一个心跳回访来计时
        ClientSocketFrameComponent.Instance.Send(RequestCode.HeartbeatPacket, "0");
    }

    public static void StopHeartbeatDetection()
    {
        UniTaskFrameComponent.Instance.RemoveTask("心跳检测");
    }

    [LabelText("开启心跳回访")]
    private static void StartHeartbeatRequest()
    {
        //一个心跳回合后,此定时器还未关闭,说明与服务器交互出了问题
        // Debug.Log("打开心跳回访");
        UniTaskFrameComponent.Instance.AddTask("心跳回访", heartBeatAbnormalTime, 1, null, null, StartDisconnectReconnect);
    }
    [LabelText("打开断线重连")]
    public static void StartDisconnectReconnect()
    {
        Debug.Log("打开断线重连");
        //重置断线重连次数
        disconnectReconnectCount = disconnectReconnectDefaultCount;
        UniTaskFrameComponent.Instance.AddTask("断线重连", 1, 0, null, null, OnDisconnectReconnect);
    }


    [LabelText("关闭心跳回访")]
    private static void StopHeartbeatRequest()
    {
        // Debug.Log("关闭心跳回访");
        UniTaskFrameComponent.Instance.RemoveTask("心跳回访");
    }

   
    [LabelText("关闭断线重连")]
    private static void StopDisconnectReconnect()
    {
        UniTaskFrameComponent.Instance.RemoveTask("断线重连");
    }

    [LabelText("断线重连")]
    private static void OnDisconnectReconnect()
    {
        disconnectReconnectCount--;
        if (disconnectReconnectCount <= 0)
        {
            //心跳停止
            foreach (IHeartbeat iheartbeat in Iheartbeats)
            {
                iheartbeat.HeartbeatStop();
            }

            //关闭心跳检测
            StopDisconnectReconnect();
        }
        else
        {
            //心跳异常
            foreach (IHeartbeat iheartbeat in Iheartbeats)
            {
                //心跳异常
                iheartbeat.HeartbeatAbnormal(disconnectReconnectCount);
            }

            Debug.Log("心跳异常.重新连接服务器");
            StopHeartbeatDetection();
        }
    }


    [AddRequestCode(RequestCode.HeartbeatPacket, RequestType.Client)]
    public void OnHeartbeat(string data)
    {
        if (Iheartbeats == null)
        {
            return;
        }

        // isHeartbeatRestore = true;
        foreach (IHeartbeat iheartbeat in Iheartbeats)
        {
            if (iheartbeat != null)
            {
                //心跳正常
                iheartbeat.HeartbeatNormal();
            }
        }

        StopHeartbeatRequest();
    }
}