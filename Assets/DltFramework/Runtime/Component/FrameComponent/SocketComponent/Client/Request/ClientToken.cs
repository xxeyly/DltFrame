using System;
using DltFramework;

public class ClientToken
{
    [AddRequestCode(RequestCode.Token, RequestType.Client)]
    public void OnToken(string data)
    {
        FileOperationComponent.SaveTextToLoad(DataFrameComponent.Path_DeviceStorage(), "Token.txt", data);
        ClientSocketFrameComponent.Instance.Token = Convert.ToInt32(data);
        ViewFrameComponent.Instance.HideView(typeof(ConnectServer));
        ViewFrameComponent.Instance.ShowView(typeof(EnterGame));
    }
}