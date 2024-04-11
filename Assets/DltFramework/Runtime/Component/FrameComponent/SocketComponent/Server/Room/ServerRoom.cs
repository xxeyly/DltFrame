using System;
using System.Collections.Generic;

public class ServerRoom
{
    public ServerRoomData ServerRoomData = new ServerRoomData();


    /// <summary>
    /// 房间初始化帧索引
    /// </summary>
    public int roomInitFrameIndex;

    /// <summary>
    /// 房间内玩家
    /// </summary>
    private List<ClientSocket> clientSockets = new List<ClientSocket>();

    /// <summary>
    /// 玩家准备
    /// </summary>
    private Dictionary<ClientSocket, bool> playerReady = new Dictionary<ClientSocket, bool>();


    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="clientSocket"></param>
    public void AddRoom(ClientSocket clientSocket)
    {
        clientSockets.Add(clientSocket);
    }

    /// <summary>
    /// 退出房间
    /// </summary>
    /// <param name="clientSocket"></param>
    public void ExitRoom(ClientSocket clientSocket)
    {
        clientSockets.Remove(clientSocket);
    }

    /// <summary>
    /// 房间是否满了
    /// </summary>
    /// <returns></returns>
    public bool IsFull()
    {
        return clientSockets.Count >= ServerRoomData.roomPlayerMaxCount;
    }

    /// <summary>
    /// 玩家准备
    /// </summary>
    /// <param name="clientSocket"></param>
    /// <param name="ready"></param>
    public void PlayerReady(ClientSocket clientSocket, bool ready)
    {
        playerReady[clientSocket] = ready;
    }
    
    public Dictionary<int,bool> GetPlayerReady()
    {
        Dictionary<int,bool> playerReadyData = new Dictionary<int, bool>();
        foreach (var item in playerReady)
        {
            playerReadyData.Add(item.Key.token,item.Value);
        }

        return playerReadyData;
    }
}