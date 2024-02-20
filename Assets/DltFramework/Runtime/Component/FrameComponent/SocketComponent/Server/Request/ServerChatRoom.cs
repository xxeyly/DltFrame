using System;

public class ServerChatRoom
{
    [AddRequestCode(RequestCode.ChatRoom, RequestType.Server)]
    public void OnHeartbeat(string data, ClientSocket clientSocket)
    {
        foreach (ClientSocket socket in ClientSocketManager.clientSocketList)
        {
            if (socket != clientSocket)
            {
                socket.Send(RequestCode.ChatRoom, clientSocket.socket.RemoteEndPoint + ":" + data);
            }
            else
            {
                socket.Send(RequestCode.ChatRoom, "自己" + ":" + data);
            }
        }
    }
}