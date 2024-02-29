using System;
using System.Collections.Generic;
using System.Net;

public class ClientSocketManager
{
    private static Random random = new Random();

    //客户端SocketId列表
    public static List<int> clientSocketIdList = new List<int>();

    //客户端Socket列表
    public static List<ClientSocket> clientSocketList = new List<ClientSocket>();
    

    public static void AddClientSocket(ClientSocket clientSocket)
    {
        clientSocketList.Add(clientSocket);
    }

    public static void RemoveClientSocket(ClientSocket clientSocket)
    {
        clientSocketIdList.Remove(clientSocket.token);
        clientSocketList.Remove(clientSocket);
    }

    //获得指定客户端
    public static ClientSocket GetClientSocket(int connectCode)
    {
        foreach (ClientSocket clientSocket in clientSocketList)
        {
            if (clientSocket.token == connectCode)
            {
                return clientSocket;
            }
        }


        return null;
    }


    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <returns></returns>
    public static int GetClientToken()
    {
        //生成随机数
        int id = random.Next();
        if (clientSocketIdList.Contains(id))
        {
            GetClientToken();
        }
        else
        {
            clientSocketIdList.Add(id);
        }

        return id;
    }
}