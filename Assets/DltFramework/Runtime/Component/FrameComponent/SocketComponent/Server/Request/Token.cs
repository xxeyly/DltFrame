using System;
using System.Text;

public class Token
{
    [AddRequestCode(RequestCode.Token, RequestType.Server)]
    public void OnToken(byte[] data, ClientSocket clientSocket)
    {
        string content = Encoding.UTF8.GetString(data);
        Console.WriteLine("Token:" + content);
        //该设备还没有Token
        if (content == "0")
        {
            //Token为0,表示该设备还没有Token,需要生成Token
            clientSocket.token = ClientSocketManager.GetClientToken();
        }
        else
        {
            
            clientSocket.token = int.Parse(content);
        }

        clientSocket.TcpSend(RequestCode.Token, clientSocket.token.ToString());
    }
}