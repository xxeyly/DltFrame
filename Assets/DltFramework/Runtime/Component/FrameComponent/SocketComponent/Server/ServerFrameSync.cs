using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LitJson;

public class ServerFrameSyncData
{
    public ClientSocket clientSocket;
    public List<byte[]> data;
}

public class ServerFrameSync
{
    //帧数据记录
    public static long startTime;
    public static long currentTime;

    public static long oldTime;

    //开始帧记录
    public static bool startFrameRecord = false;

    public static void CreateFrameSync()
    {
        TimeSpan TimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        startTime = (long)TimeSpan.TotalMilliseconds;
        Console.WriteLine("服务器开始时间:" + startTime);
        oldTime = startTime;
        new Thread(OnFrameSync).Start();
    }

    private static void OnFrameSync()
    {
        while (true)
        {
            // Thread.Sleep(60);
            TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            currentTime = (long)mTimeSpan.TotalMilliseconds;
            // Console.WriteLine(currentTime);
            // Console.WriteLine(oldTime);
            if (currentTime - oldTime >= ClientFrameSync.frameInterval)
            {
                int timeOffset = (int)(currentTime - oldTime) - ClientFrameSync.frameInterval;
                oldTime = currentTime;
                //有时可能会多出来1-2,减去偏差,下次不用计算了
                oldTime -= timeOffset;
                ClientFrameSync.serverFrameIndex += 1;
                // Console.WriteLine(FrameRecord.serverFrameIndex + ":" + currentTime);
                if (!FrameRecord.ContainsFrameIndex(ClientFrameSync.serverFrameIndex))
                {
                    //没有任何客户端连接,服务器自创建
                    RecordFrameSyncData(ClientFrameSync.serverFrameIndex);
                }
                //发送帧到服务器端
                foreach (ClientSocket clientSocket in ClientSocketManager.clientSocketList)
                {
                    if (clientSocket != null && clientSocket.udpClient != null && clientSocket.remoteIpEndPoint != null)
                    {
                        Console.WriteLine("客户端帧:" + clientSocket.FrameIndex + "服务器帧:" + ClientFrameSync.serverFrameIndex);
                        //+1 客户端有这么一帧了,不用再发了
                        //-1 服务器这帧刚更新,是没有数据的,不用发
                        for (int i = clientSocket.FrameIndex + 1; i < ClientFrameSync.serverFrameIndex; i++)
                        {
                            Console.WriteLine("帧发送:" + i + ":" + JsonMapper.ToJson(FrameRecord.frameRecord[i - 1]));
                            clientSocket.UdpSend(i, JsonMapper.ToJson(FrameRecord.frameRecord[i - 1]));
                        }

                        Console.WriteLine("---------------------------------------------------------");
                    }
                }
            }
        }
    }


    //记录帧操作
    public static void RecordFrameSyncData(int clientFrameIndex, FrameRecordData frameRecordData = null)
    {
        FrameRecord.AddFrameRecordData(clientFrameIndex, frameRecordData);
    }
}