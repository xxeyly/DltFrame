using System;
using System.Collections.Generic;
using LitJson;

public class ServerPlayerMove
{
    public static Dictionary<ClientSocket, PlayerData> playerDataDic = new Dictionary<ClientSocket, PlayerData>();

    //玩家加入服务器
    [AddRequestCode(RequestCode.PlayerInit, RequestType.Server)]
    public static void OnPlayerInit(string data, ClientSocket clientSocket)
    {
        playerDataDic.Add(clientSocket, new PlayerData() { id = clientSocket.clientSocketId });
        clientSocket.TcpSend(RequestCode.PlayerInit, clientSocket.clientSocketId.ToString());
        foreach (ClientSocket otherSocket in ClientSocketManager.clientSocketList)
        {
            if (otherSocket != clientSocket)
            {
                //其他玩家的位置数据
                PlayerData playerData = playerDataDic[otherSocket];
                //自身获得其他玩家的初始化
                clientSocket.TcpSend(RequestCode.OtherPlayerInit, otherSocket.clientSocketId.ToString());
                //自身获得其他玩家的位置
                clientSocket.UdpSend(RequestCode.OtherPlayerMove, JsonMapper.ToJson(playerData));
                //其他玩家获得当前玩家的初始化
                otherSocket.TcpSend(RequestCode.OtherPlayerInit, clientSocket.clientSocketId.ToString());
            }
        }
    }

    //玩家移动
    [AddRequestCode(RequestCode.PlayerMove, RequestType.Server)]
    public void OnPlayerMove(string data, ClientSocket clientSocket)
    {
        PlayerData playerData = JsonMapper.ToObject<PlayerData>(data);
        playerData.id = clientSocket.clientSocketId;
        playerDataDic[clientSocket] = playerData;
        // Console.WriteLine(clientSocket.clientSocketId);
        clientSocket.UdpSend(RequestCode.PlayerMove, JsonMapper.ToJson(playerData));

        foreach (ClientSocket socket in ClientSocketManager.clientSocketList)
        {
            if (socket != clientSocket)
            {
                // Console.WriteLine(socket.clientSocketId + "其他玩家更新");
                socket.UdpSend(RequestCode.OtherPlayerMove, JsonMapper.ToJson(playerData));
            }
        }
    }

    public static void OnPlayerExit(ClientSocket clientSocket)
    {
        foreach (ClientSocket socket in ClientSocketManager.clientSocketList)
        {
            if (socket != clientSocket)
            {
                socket.TcpSend(RequestCode.OtherPlayerExit, clientSocket.clientSocketId.ToString());
            }
        }

        playerDataDic.Remove(clientSocket);
    }
}