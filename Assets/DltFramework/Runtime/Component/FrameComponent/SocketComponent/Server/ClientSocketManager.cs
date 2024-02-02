using System;
using System.Collections.Generic;

public class ClientSocketManager
{
    private Random random = new Random();

    //客户端SocketId列表
    public List<int> clientSocketIdList = new List<int>();

    //客户端Socket列表
    public static List<ClientSocket> clientSocketList = new List<ClientSocket>();

    public void AddClientSocket(ClientSocket clientSocket)
    {
        clientSocket.clientSocketId = GetClientSocketId();
        clientSocketList.Add(clientSocket);
    }

    public void RemoveClientSocket(ClientSocket clientSocket)
    {
        clientSocketIdList.Remove(clientSocket.clientSocketId);
        clientSocketList.Remove(clientSocket);
    }

    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <returns></returns>
    private int GetClientSocketId()
    {
        //生成随机数
        int id = random.Next();
        if (clientSocketIdList.Contains(id))
        {
            GetClientSocketId();
        }
        else
        {
            clientSocketIdList.Add(id);
        }

        return id;
    }
}