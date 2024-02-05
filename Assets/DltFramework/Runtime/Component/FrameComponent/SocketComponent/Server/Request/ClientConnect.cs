using System;

public class ClientConnect 
{
    [AddRequestCode(RequestCode.None,RequestType.Server)]
    public void OnConnectSuccess(string data)
    {
        Console.WriteLine(data);
    }
}
