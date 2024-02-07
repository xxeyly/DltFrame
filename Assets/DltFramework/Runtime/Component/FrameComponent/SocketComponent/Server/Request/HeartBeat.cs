using System;
using System.Collections.Generic;


public class HeartBeat
{
    public static List<ClientSocket> heartBeatDataList = new List<ClientSocket>();

    //旧的心跳时间
    private long oldHeartBeatTime;

    //心跳间隔
    private int heartBeatInterval = 5;

    public void AddClientSocket(ClientSocket clientSocket)
    {
        heartBeatDataList.Add(clientSocket);
    }

    public void RemoveClientSocket(ClientSocket clientSocket)
    {
        heartBeatDataList.Remove(clientSocket);
    }

    //创建心跳包
    public void CreateHeartBeat()
    {
        //创建心跳包,等于当前时间
        oldHeartBeatTime = GetTimeStamp();
        //创建心跳包
        while (true)
        {
            if (GetTimeStamp() - oldHeartBeatTime >= heartBeatInterval)
            {
                oldHeartBeatTime = GetTimeStamp();
                for (int i = 0; i < heartBeatDataList.Count; i++)
                {
                    if (heartBeatDataList[i].isHeartBeat == false)
                    {
                        heartBeatDataList[i].CloseConnection();
                    }
                    else
                    {
                        heartBeatDataList[i].isHeartBeat = false;
                        heartBeatDataList[i].Send(RequestCode.HeartbeatPacket, "1");
                    }
                }
            }
        }
    }

    private long GetTimeStamp()
    {
        return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
    }

    [AddRequestCode(RequestCode.HeartbeatPacket, RequestType.Server)]
    public void OnHeartbeat(string data, ClientSocket clientSocket)
    {
        if (IsContainsClientSocket(clientSocket))
        {
            HeartBeatRecovery(clientSocket);
        }
    }

    private bool IsContainsClientSocket(ClientSocket clientSocket)
    {
        return heartBeatDataList.Contains(clientSocket);
    }

    //心跳恢复
    private void HeartBeatRecovery(ClientSocket clientSocket)
    {
        clientSocket.isHeartBeat = true;
    }
}