using System;
using System.Collections.Generic;
using Google.Protobuf;

public class ServerMap
{
    private bool frameSyncInit = false;

    public ServerMapData ServerMapData = new ServerMapData();

    /// <summary>
    /// 地图内帧索引
    /// </summary>
    public int mapFrameIndex = 0;

    /// <summary>
    /// 偏移帧
    /// </summary>
    public int offsetFrameIndex = 0;

    //帧记录开始时间
    public long startTime;

    /// <summary>
    /// 地图内玩家
    /// </summary>
    public List<ClientSocket> clientSockets = new List<ClientSocket>();

    //所有客户端帧记录
    public List<FrameDataGroup> frameDataGroups = new List<FrameDataGroup>();

    public void MapInit()
    {
        ServerFrameSync.frameSync += OnFrameSync;
    }

    private void OnFrameSync(int frameIndex)
    {
        if (!frameSyncInit)
        {
            frameSyncInit = true;
            offsetFrameIndex = frameIndex;
            startTime = ServerFrameSync.currentTime;
        }

        //发送帧数据到客户端
        mapFrameIndex = frameIndex - offsetFrameIndex;
        // Console.WriteLine(ServerFrameSync.currentTime + ":" + mapFrameIndex);
        FrameDataGroup frameDataGroup = new FrameDataGroup();
        frameDataGroup.FrameIndex = mapFrameIndex;
        frameDataGroups.Add(frameDataGroup);
        foreach (ClientSocket clientSocket in clientSockets)
        {
            FrameData frameData = new FrameData();
            frameData.FrameIndex = mapFrameIndex;
            frameData.ClientToken = clientSocket.token;
            frameData.DataType = "Empty";
            frameData.Data = ByteString.CopyFrom(new byte[] { });
            frameDataGroup.FrameData.Add(frameData);
        }

        SendFrameData();
    }

    /// <summary>
    /// 玩家加入地图
    /// </summary>
    /// <param name="clientSocket"></param>
    public void ClientAddMap(ClientSocket clientSocket)
    {
        if (!clientSockets.Contains(clientSocket))
        {
            clientSockets.Add(clientSocket);
        }
    }

    /// <summary>
    /// 退出地图
    /// </summary>
    /// <param name="clientSocket"></param>
    public void ClientExitMap(ClientSocket clientSocket)
    {
        if (clientSockets.Contains(clientSocket))
        {
            clientSockets.Remove(clientSocket);
        }
    }

    /// <summary>
    /// 记录帧数据
    /// </summary>
    public void AddFrameData(ClientSocket clientSocket, FrameData frameRecordData)
    {
        FrameDataGroup frameDataGroup = GetFrameDataGroup(mapFrameIndex);

        if (frameRecordData != null)
        {
            //移除旧的帧数据
            foreach (FrameData frameData in frameDataGroup.FrameData)
            {
                if (frameData.ClientToken == clientSocket.token)
                {
                    frameDataGroup.FrameData.Remove(frameData);
                }
            }

            //添加新的帧数据
            frameDataGroup.FrameData.Add(frameRecordData);
        }

        clientSocket.FrameIndex = frameRecordData.FrameIndex;
    }

    /// <summary>
    /// 获得当前帧数据组
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <returns></returns>
    private FrameDataGroup GetFrameDataGroup(int frameIndex)
    {
        for (int i = 0; i < frameDataGroups.Count; i++)
        {
            if (frameDataGroups[i].FrameIndex == frameIndex)
            {
                return frameDataGroups[i];
            }
        }

        return null;
    }

    private void SendFrameData()
    {
        foreach (ClientSocket clientSocket in clientSockets)
        {
            if (!clientSocket.UdpIsReady())
            {
                return;
            }

            FrameDataGroupList frameDataGroupList = new FrameDataGroupList();
            List<FrameDataGroup> tempAddFrameDataGroup = new List<FrameDataGroup>();
            Console.WriteLine("客户端当前帧:" + clientSocket.FrameIndex);
            Console.WriteLine("服务器当前帧:" + mapFrameIndex);
            Console.Write("发送帧:[");
            //客户端当前帧不发送
            for (int i = clientSocket.FrameIndex; i < mapFrameIndex; i++)
            {
                tempAddFrameDataGroup.Add(frameDataGroups[i]);
                Console.Write(i);
                if (i != mapFrameIndex - 1)
                {
                    Console.Write(",");
                }
            }

            Console.Write("]");
            Console.WriteLine();
            frameDataGroupList.FrameDataGroup.AddRange(tempAddFrameDataGroup);
            // Console.WriteLine("发送帧数" + frameDataGroupList.FrameDataGroup.Count);
            clientSocket.UdpSend(ProtobufTool.SerializeToByteArray(frameDataGroupList));
        }
    }
}