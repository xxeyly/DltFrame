using System;
using System.Collections.Generic;
using DltFramework;

public class ClientToken
{
    [AddRequestCode(RequestCode.Token, RequestType.Client)]
    public void OnToken(string data)
    {
        FileOperationComponent.SaveTextToLoad(DataFrameComponent.Path_DeviceStorage(), "Token.txt", data);
        ClientSocketFrameComponent.Instance.Token = Convert.ToInt32(data);
        //获得所有客户端IClientToken
        List<IClientToken> clientTokenList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IClientToken>();
        foreach (IClientToken clientToken in clientTokenList)
        {
            //分发Token请求
            clientToken.ClientToken(data);
        }

       
    }
}