using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class RequestServerRoom
{
    [AddRequestCode(RequestCode.GetRoom, RequestType.Server)]
    public void OnGetRoom(string data, ClientSocket clientSocket)
    {
        List<ServerRoomData> serverRoomDataList = ServerRoomManager.GetServerRoomData();
        string serverRoomDataListJson = JsonMapper.ToJson(serverRoomDataList);
        clientSocket.TcpSend(RequestCode.GetRoom, serverRoomDataListJson);
    }

    [AddRequestCode(RequestCode.CreateRoom, RequestType.Server)]
    public void OnCreateRoom(string data, ClientSocket clientSocket)
    {
        ServerRoomData serverRoomData = JsonMapper.ToObject<ServerRoomData>(data);
        serverRoomData.roomId = ServerRoomManager.GenerateRoomId();
        Console.WriteLine(serverRoomData.roomName);
        Console.WriteLine(serverRoomData.roomId);
        Console.WriteLine(serverRoomData.roomPassword);
        Console.WriteLine(serverRoomData.roomPlayerMaxCount);
        ServerRoomManager.AddServerRoom(serverRoomData);
        clientSocket.TcpSend(RequestCode.CreateRoomSuccessFully, JsonMapper.ToJson(serverRoomData));
    }

    [AddRequestCode(RequestCode.EnterRoom, RequestType.Server)]
    public void OnEnterRoom(string roomData, ClientSocket clientSocket)
    {
        ServerRoomData clientServerRoomData = JsonMapper.ToObject<ServerRoomData>(roomData);
        ServerRoom serverRoom = ServerRoomManager.GetServerRoom(clientServerRoomData.roomId);
        if (serverRoom.IsFull())
        {
            clientSocket.TcpSend(RequestCode.EnterRoomFailedRoomFull, "房间已满");
        }
        else if (serverRoom.ServerRoomData.roomPassword != clientServerRoomData.roomPassword)
        {
            clientSocket.TcpSend(RequestCode.EnterRoomFailedRoomPasswordError, "密码错误");
        }
        else
        {
            clientSocket.TcpSend(RequestCode.EnterRoomSuccessFully, "1");

            serverRoom.AddRoom(clientSocket);
        }

        /*//获得所有帧数据
        //第一次使用Tcp传输,后面使用Udp传输
        //获得所有帧数据
        FrameInitData frameInitData = new FrameInitData();
        frameInitData.frameIndex = ServerFrameSync.serverFrameIndex;
        frameInitData.startTime = ServerFrameSync.startTime;
        frameInitData.currentTime = ServerFrameSync.currentTime;
        //精简了帧数据,无操作的帧数据不传输,需要客户端自己计算
        Console.WriteLine("帧数据数量:" + FrameRecord.frameRecord.Count);
        //有效帧数据
        List<FrameRecordDataGroup> effectiveFrameRecord = new List<FrameRecordDataGroup>();
        for (int i = 0; i < FrameRecord.frameRecord.Count; i++)
        {
            bool isEffective = false;
            foreach (FrameRecordData frameRecordData in FrameRecord.frameRecord[i].frameRecordData)
            {
                if (!FrameRecordData.IsEmptyFrame(frameRecordData))
                {
                    isEffective = true;
                }
            }

            if (isEffective)
            {
                effectiveFrameRecord.Add(FrameRecord.frameRecord[i]);
            }
        }

        frameInitData.frameRecord = FrameRecord.frameRecord;*/
    }

    [AddRequestCode(RequestCode.EnterRoomGetRoomPlayer, RequestType.Server)]
    public void OnEnterRoomGetRoomPlayer(string data, ClientSocket clientSocket)
    {
        ServerRoom serverRoom = ServerRoomManager.GetServerRoom(int.Parse(data));
        Dictionary<int, bool> playerReady = serverRoom.GetPlayerReady();
        clientSocket.TcpSend(RequestCode.EnterRoomGetRoomPlayer, JsonMapper.ToJson(playerReady));
    }
}