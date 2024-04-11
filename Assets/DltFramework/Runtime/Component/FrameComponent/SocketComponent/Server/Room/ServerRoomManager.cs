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
    public static void AddServerRoom(ServerRoomData serverRoomData)
    {
        ServerRoom serverRoom = new ServerRoom();
        serverRoom.ServerRoomData = serverRoomData;
        serverRooms.Add(serverRoom);
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
}