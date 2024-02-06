using System;
using System.Collections.Generic;

public class HeartBeatData
{
    public ClientSocket clientSocket;
    public float time;
}

public class HeartBeat
{
    public static List<HeartBeatData> heartBeatDataList = new List<HeartBeatData>();

    //旧的心跳时间
    private long oldHeartBeatTime;

    //心跳间隔
    private int heartBeatInterval = 1;

    //心跳时间上限
    private int heartBeatTimeLimit = 30;

    //心跳异常提示时间
    private int heartBeatAbnormalTime = 5;

    public void AddClientSocket(ClientSocket clientSocket)
    {
        heartBeatDataList.Add(new HeartBeatData()
        {
            clientSocket = clientSocket,
            time = heartBeatTimeLimit
        });
    }

    public void RemoveClientSocket(ClientSocket clientSocket)
    {
        for (int i = 0; i < heartBeatDataList.Count; i++)
        {
            if (heartBeatDataList[i].clientSocket == clientSocket)
            {
                heartBeatDataList.RemoveAt(i);
                break;
            }
        }
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
                    heartBeatDataList[i].time -= 1;
                    if (heartBeatDataList[i].time < heartBeatTimeLimit - heartBeatAbnormalTime)
                    {
                        Console.WriteLine(heartBeatDataList[i].clientSocket.socket.RemoteEndPoint + "心跳状态异常:" + heartBeatDataList[i].time);
                    }

                    if (heartBeatDataList[i].time <= 0)
                    {
                        heartBeatDataList[i].clientSocket.CloseConnection();
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
        foreach (HeartBeatData heartBeatData in heartBeatDataList)
        {
            if (heartBeatData.clientSocket.clientSocketId == clientSocket.clientSocketId)
            {
                return true;
            }
        }

        return false;
    }

    //心跳恢复
    private void HeartBeatRecovery(ClientSocket clientSocket)
    {
        foreach (HeartBeatData heartBeatData in heartBeatDataList)
        {
            if (heartBeatData.clientSocket == clientSocket)
            {
                if (heartBeatData.time < heartBeatTimeLimit - heartBeatAbnormalTime)
                {
                    Console.WriteLine("心跳恢复:" + clientSocket.socket.RemoteEndPoint);
                }

                Console.WriteLine("心跳正常");
                heartBeatData.time = heartBeatTimeLimit;
                clientSocket.Send(RequestCode.HeartbeatPacket, "1");
                break;
            }
        }
    }
}