using System;

public class ClientConnect 
{
    [AddRequestCode(RequestCode.None)]
    public void OnConnectSuccess(string data)
    {
        Console.WriteLine(data);
    }
}
