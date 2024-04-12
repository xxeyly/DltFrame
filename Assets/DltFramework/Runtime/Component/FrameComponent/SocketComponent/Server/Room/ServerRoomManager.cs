using System;
using System.Collections.Generic;

public static class ServerRoomManager
{
    //服务器房间
    public static List<ServerRoom> serverRooms = new List<ServerRoom>();

    /// <summary>
    /// 生成房间Id
    /// </summary>
    /// <returns></returns>
    public static int GenerateRoomId()
    {
        int roomId = 0;

        roomId = new Random().Next(0, Int32.MaxValue);
        if (IsExistRoom(roomId))
        {
            GenerateRoomId();
        }

        return roomId;
    }

    /// <summary>
    /// 获取房间
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public static bool IsExistRoom(int roomId)
    {
        foreach (ServerRoom serverRoom in serverRooms)
        {
            if (serverRoom.ServerRoomData.roomId == roomId)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  添加房间
    /// </summary>
    public static void CreateServerRoom(ServerRoomData serverRoomData)
    {
        ServerRoom serverRoom = new ServerRoom();
        serverRoom.ServerRoomData = serverRoomData;
        Console.WriteLine("房间:" + serverRoomData.roomId + "创建");
        serverRooms.Add(serverRoom);
    }

    /// <summary>
    /// 移除房间
    /// </summary>
    /// <param name="serverRoomData"></param>
    public static void RemoveRoom(ServerRoomData serverRoomData)
    {
        foreach (ServerRoom serverRoom in serverRooms)
        {
            if (serverRoom.ServerRoomData.roomId == serverRoomData.roomId)
            {
                Console.WriteLine("房间:" + serverRoomData.roomId + "移除");
                serverRooms.Remove(serverRoom);
                return;
            }
        }
    }

    /// <summary>
    /// 获取房间
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public static ServerRoom GetServerRoom(int roomId)
    {
        foreach (ServerRoom serverRoom in serverRooms)
        {
            if (serverRoom.ServerRoomData.roomId == roomId)
            {
                return serverRoom;
            }
        }

        return null;
    }

    /// <summary>
    /// 获得房间数据
    /// </summary>
    /// <returns></returns>
    public static List<ServerRoomData> GetServerRoomData()
    {
        List<ServerRoomData> serverRoomDatas = new List<ServerRoomData>();
        foreach (ServerRoom serverRoom in serverRooms)
        {
            serverRoomDatas.Add(serverRoom.ServerRoomData);
        }

        return serverRoomDatas;
    }
    /// <summary>
    /// 玩家退出房间
    /// </summary>
    /// <param name="clientToken"></param>
    public static void ExitRoom(int clientToken)
    {
        foreach (ServerRoom serverRoom in serverRooms)
        {
            foreach (ClientSocket clientSocket in serverRoom.clientSockets)
            {
                //当前玩家在这个房间
                if (clientSocket.token == clientToken)
                {
                    serverRoom.ClientExitRoom(clientSocket);
                    return;
                }
            }
        }
    }
}