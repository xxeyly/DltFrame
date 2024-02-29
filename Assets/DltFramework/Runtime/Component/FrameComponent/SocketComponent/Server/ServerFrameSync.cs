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
            if (currentTime - oldTime >= 60)
            {
                int timeOffset = (int)(currentTime - oldTime) - 60;
                oldTime = currentTime;
                //有时可能会多出来1-2,减去偏差,下次不用计算了
                oldTime -= timeOffset;
                // Console.WriteLine(FrameRecord.frameIndex + ":" + currentTime);

                if (!FrameRecord.ContainsFrameIndex(FrameRecord.frameIndex))
                {
                    //没有任何客户端连接,服务器自创建
                    RecordFrameSyncData(FrameRecord.frameIndex, new FrameRecordData());
                }

                //发送帧到服务器端
                foreach (ClientSocket clientSocket in ClientSocketManager.clientSocketList)
                {
                    if (clientSocket != null && clientSocket.udpClient != null && clientSocket.remoteIpEndPoint != null)
                    {
                        for (int i = clientSocket.FrameIndex; i <= FrameRecord.frameIndex; i++)
                        {
                            clientSocket.UdpSend(i, JsonMapper.ToJson(FrameRecord.frameRecord[i]));
                        }
                    }
                }

                FrameRecord.frameIndex += 1;
            }
        }
    }


    //记录帧操作
    public static void RecordFrameSyncData(int clientFrameIndex, FrameRecordData frameRecordData)
    {
        FrameRecord.AddFrameRecordData(clientFrameIndex, frameRecordData);
    }
}