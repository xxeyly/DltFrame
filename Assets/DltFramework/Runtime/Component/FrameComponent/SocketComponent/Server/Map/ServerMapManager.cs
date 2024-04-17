using System;
using System.Collections.Generic;

public class ServerMapManager
{
    //服务器地图
    public static List<ServerMap> serverMaps = new List<ServerMap>();

    /// <summary>
    /// 获取地图
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    public static bool IsExistMap(int mapId)
    {
        foreach (ServerMap serverMap in serverMaps)
        {
            if (serverMap.ServerMapData.mapId == mapId)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///  添加地图
    /// </summary>
    public static void CreateServerMap(ServerRoomData serverRoomData)
    {
        ServerMap serverMap = new ServerMap();
        serverMap.ServerMapData.mapId = serverRoomData.roomId;
        serverMap.ServerMapData.mapPlayerCount = serverRoomData.roomPlayerCount;
        serverMap.ServerMapData.mapPlayerMaxCount = serverRoomData.roomPlayerMaxCount;
        serverMap.MapInit();
        Console.WriteLine("地图:" + serverMap.ServerMapData.mapId + "创建");
        serverMaps.Add(serverMap);
    }

    /// <summary>
    /// 移除地图
    /// </summary>
    /// <param name="mapId"></param>
    public static void RemoveMap(int mapId)
    {
        foreach (ServerMap serverMap in serverMaps)
        {
            if (serverMap.ServerMapData.mapId == mapId)
            {
                Console.WriteLine("地图:" + serverMap.ServerMapData.mapId + "移除");
                serverMaps.Remove(serverMap);
                return;
            }
        }
    }

    /// <summary>
    /// 获取地图
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    public static ServerMap GetServerMap(int mapId)
    {
        foreach (ServerMap serverMap in serverMaps)
        {
            if (serverMap.ServerMapData.mapId == mapId)
            {
                return serverMap;
            }
        }

        return null;
    }

    /// <summary>
    /// 玩家退出房间
    /// </summary>
    /// <param name="clientToken"></param>
    public static void ExitRoom(int clientToken)
    {
        foreach (ServerMap serverMap in serverMaps)
        {
            foreach (ClientSocket clientSocket in serverMap.clientSockets)
            {
                //当前玩家在这个房间
                if (clientSocket.token == clientToken)
                {
                    serverMap.ClientExitMap(clientSocket);
                    return;
                }
            }
        }
    }
}