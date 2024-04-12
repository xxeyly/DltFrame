using System;
using System.Collections.Generic;
using LitJson;

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
    public List<ClientSocket> clientSockets = new List<ClientSocket>();

    /// <summary>
    /// 玩家准备
    /// </summary>
    private Dictionary<ClientSocket, bool> playerReady = new Dictionary<ClientSocket, bool>();


    /// <summary>
    /// 玩家加入房间
    /// </summary>
    /// <param name="clientSocket"></param>
    public void ClientAddRoom(ClientSocket clientSocket)
    {
        if (!clientSockets.Contains(clientSocket))
        {
            clientSockets.Add(clientSocket);
        }

        if (!playerReady.ContainsKey(clientSocket))
        {
            playerReady.Add(clientSocket, false);
        }

        ServerRoomData.roomPlayerCount = clientSockets.Count;
        //自己进入房间的时候会获得所有玩家状态
        //向其他玩家发送进入房间
        ServerRoomPlayerReadyState serverRoomPlayerReadyState = new ServerRoomPlayerReadyState();
        serverRoomPlayerReadyState.ready = false;
        serverRoomPlayerReadyState.roomId = ServerRoomData.roomId;
        serverRoomPlayerReadyState.token = clientSocket.token.ToString();
        foreach (ClientSocket socket in clientSockets)
        {
            if (socket.token != clientSocket.token)
            {
                socket.TcpSend(RequestCode.Room_OtherPlayerEnterRoom, LitJson.JsonMapper.ToJson(serverRoomPlayerReadyState));
            }
        }
    }

    /// <summary>
    /// 退出房间
    /// </summary>
    /// <param name="clientSocket"></param>
    public void ClientExitRoom(ClientSocket clientSocket)
    {
        clientSockets.Remove(clientSocket);
        playerReady.Remove(clientSocket);
        ServerRoomData.roomPlayerCount = clientSockets.Count;
        //向其他玩家发送退出房间
        ServerRoomPlayerReadyState serverRoomPlayerReadyState = new ServerRoomPlayerReadyState();
        serverRoomPlayerReadyState.ready = false;
        serverRoomPlayerReadyState.roomId = ServerRoomData.roomId;
        serverRoomPlayerReadyState.token = clientSocket.token.ToString();
        Console.WriteLine(ServerRoomData.roomId + ":" + ServerRoomData.roomPlayerCount);
        foreach (ClientSocket socket in clientSockets)
        {
            if (socket.token != clientSocket.token)
            {
                socket.TcpSend(RequestCode.Room_OtherPlayerExitRoom, LitJson.JsonMapper.ToJson(serverRoomPlayerReadyState));
            }
        }

        //房间没人了
        if (clientSockets.Count == 0)
        {
            ServerRoomManager.RemoveRoom(ServerRoomData);
        }
        
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
    public void ClientPlayerReady(ClientSocket clientSocket, bool ready)
    {
        playerReady[clientSocket] = ready;
        //广播给其他玩家
        foreach (ClientSocket socket in clientSockets)
        {
            ServerRoomPlayerReadyState serverRoomPlayerReadyState = new ServerRoomPlayerReadyState()
            {
                roomId = ServerRoomData.roomId,
                token = clientSocket.token.ToString(),
                ready = ready
            };
            //给自己发
            if (socket.token == clientSocket.token)
            {
                socket.TcpSend(RequestCode.Room_Ready, JsonMapper.ToJson(serverRoomPlayerReadyState));
            }
            //给其他玩家发
            else
            {
                socket.TcpSend(RequestCode.Room_OtherPlayerReady, JsonMapper.ToJson(serverRoomPlayerReadyState));
            }
        }
    }

    /// <summary>
    /// 获得玩家状态
    /// </summary>
    /// <returns></returns>
    public List<ServerRoomPlayerReadyState> GetPlayerReady()
    {
        List<ServerRoomPlayerReadyState> serverRoomPlayerReadyStates = new List<ServerRoomPlayerReadyState>();
        //防止玩家显示位置不一致
        foreach (ClientSocket clientSocket in clientSockets)
        {
            serverRoomPlayerReadyStates.Add(new ServerRoomPlayerReadyState()
            {
                roomId = ServerRoomData.roomId,
                token = clientSocket.token.ToString(),
                ready = playerReady[clientSocket]
            });
        }

        return serverRoomPlayerReadyStates;
    }
}