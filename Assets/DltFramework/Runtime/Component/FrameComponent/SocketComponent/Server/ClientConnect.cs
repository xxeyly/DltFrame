using System;

public class ClientConnect 
{
    [AddRequestCode(RequestCode.Heartbeat)]
    public void OnConnectSuccess(string data)
    {
        Console.WriteLine(data);
    }
}
