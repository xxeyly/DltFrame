using System;
using System.Collections.Generic;
using LitJson;

public class ServerPlayerMove
{
    public static Dictionary<ClientSocket, PlayerData> playerDataDic = new Dictionary<ClientSocket, PlayerData>();

    public static void OnPlayerInit(ClientSocket clientSocket)
    {
        playerDataDic.Add(clientSocket, new PlayerData() { id = clientSocket.clientSocketId });
        clientSocket.Send(RequestCode.PlayerInit, clientSocket.clientSocketId.ToString());
        foreach (ClientSocket otherSocket in ClientSocketManager.clientSocketList)
        {
            if (otherSocket != clientSocket)
            {
                //其他玩家
                otherSocket.Send(RequestCode.OtherPlayerInit, clientSocket.clientSocketId.ToString());
                //自身
                clientSocket.Send(RequestCode.OtherPlayerInit, otherSocket.clientSocketId.ToString());
                PlayerData playerData = playerDataDic[otherSocket];
                clientSocket.Send(RequestCode.OtherPlayerMove, JsonMapper.ToJson(playerData));
            }
        }
    }

    [AddRequestCode(RequestCode.PlayerMove, RequestType.Server)]
    public void OnPlayerMove(string data, ClientSocket clientSocket)
    {
        PlayerData playerData = JsonMapper.ToObject<PlayerData>(data);
        playerData.id = clientSocket.clientSocketId;
        playerDataDic[clientSocket] = playerData;
        clientSocket.Send(RequestCode.PlayerMove, JsonMapper.ToJson(playerData));

        foreach (ClientSocket socket in ClientSocketManager.clientSocketList)
        {
            if (socket != clientSocket)
            {
                socket.Send(RequestCode.OtherPlayerMove, JsonMapper.ToJson(playerData));
            }
        }
    }

    public static void OnPlayerExit(ClientSocket clientSocket)
    {
        foreach (ClientSocket socket in ClientSocketManager.clientSocketList)
        {
            if (socket != clientSocket)
            {
                socket.Send(RequestCode.OtherPlayerExit, clientSocket.clientSocketId.ToString());
            }
        }

        playerDataDic.Remove(clientSocket);
    }
}