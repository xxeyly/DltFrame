using System;

public class Disconnect
{
    [AddRequestCode(RequestCode.Disconnect, RequestType.Server)]
    public void OnHeartbeat(string data, ClientSocket clientSocket)
    {
        Console.WriteLine(clientSocket.socket.RemoteEndPoint + "主动断开连接");
        clientSocket.CloseConnection();
    }
}