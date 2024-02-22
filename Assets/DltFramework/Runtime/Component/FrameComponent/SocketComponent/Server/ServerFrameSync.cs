using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class ServerFrameSyncData
{
    public ClientSocket clientSocket;
    public List<byte[]> data;
}

public class ServerFrameSync
{
    private static List<ServerFrameSyncData> frameSyncDataList = new List<ServerFrameSyncData>();

    public static void CreateFrameSync()
    {
        new Thread(OnFrameSync).Start();
    }

    private static void OnFrameSync()
    {
        while (true)
        {
            Thread.Sleep(20);
            // Console.WriteLine("服务器帧同步");
            for (int i = 0; i < frameSyncDataList.Count; i++)
            {
                for (int j = 0; j < frameSyncDataList[i].data.Count; j++)
                {
                    frameSyncDataList[i].clientSocket.UdpSend(frameSyncDataList[i].data[j]);
                }
            }

            frameSyncDataList.Clear();
        }
    }

    public static void AddFrameSync(ClientSocket clientSocket, byte[] data)
    {
        ServerFrameSyncData serverFrameSyncData = IsContainsClientSocket(clientSocket);
        if (serverFrameSyncData == null)
        {
            frameSyncDataList.Add(new ServerFrameSyncData() { clientSocket = clientSocket, data = new List<byte[]>() { data } });
        }
        else
        {
            serverFrameSyncData.data.Add(data);
        }
    }

    private static ServerFrameSyncData IsContainsClientSocket(ClientSocket clientSocket)
    {
        for (int i = 0; i < frameSyncDataList.Count; i++)
        {
            if (frameSyncDataList[i].clientSocket == clientSocket)
            {
                return frameSyncDataList[i];
            }
        }

        return null;
    }
}