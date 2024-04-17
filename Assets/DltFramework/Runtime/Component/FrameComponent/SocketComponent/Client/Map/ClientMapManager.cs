using System.Collections.Generic;

public class ClientMapManager
{
    public static int frameInterval = 60;
    private static Dictionary<int, ClientMap> _clientMaps = new Dictionary<int, ClientMap>();

    /// <summary>
    /// 创建服务器地图
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    public static ClientMap CreateServerMap(int mapId)
    {
        ClientMap clientMap = new ClientMap();
        _clientMaps.Add(mapId, clientMap);
        clientMap.MapInit();
        return clientMap;
    }

    /// <summary>
    /// 获取服务器地图
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    public static ClientMap GetClientMap(int mapId)
    {
        return _clientMaps[mapId];
    }
}