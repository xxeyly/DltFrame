using System;
using System.Collections.Generic;
using LitJson;

public class ServerPlayerMove
{
    public static void OnPlayerExit(ClientSocket clientSocket)
    {
        foreach (ClientSocket socket in ClientSocketManager.clientSocketList)
        {
            if (socket != clientSocket)
            {
                socket.TcpSend(RequestCode.OtherPlayerExit, clientSocket.token.ToString());
            }
        }

        Console.WriteLine(clientSocket.token + "退出服务器");
    }
}