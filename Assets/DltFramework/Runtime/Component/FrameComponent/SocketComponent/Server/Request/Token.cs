using System;

public class Token
{
    [AddRequestCode(RequestCode.Token, RequestType.Server)]
    public void OnToken(string data, ClientSocket clientSocket)
    {
        Console.WriteLine(data);
        //该设备还没有Token
        if (data == "0")
        {
            //Token为0,表示该设备还没有Token,需要生成Token
            clientSocket.token = ClientSocketManager.GetClientToken();
        }
        else
        {
            clientSocket.token = int.Parse(data);
        }

        clientSocket.TcpSend(RequestCode.Token, clientSocket.token.ToString());
    }
}