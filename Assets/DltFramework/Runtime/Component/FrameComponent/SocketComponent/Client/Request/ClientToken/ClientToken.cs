using System;
using System.Collections.Generic;
using DltFramework;
using UnityEngine;

public class ClientToken
{
    [AddRequestCode(RequestCode.Token, RequestType.Client)]
    public void OnToken(byte[] data)
    {
        string content = System.Text.Encoding.UTF8.GetString(data);
        FileOperationComponent.SaveTextToLoad(DataFrameComponent.Path_DeviceStorage(), "Token.txt", content);
        ClientSocketFrameComponent.Instance.Token = Convert.ToInt32(content);
        //获得所有客户端IClientToken
        List<IClientToken> clientTokenList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IClientToken>();
        foreach (IClientToken clientToken in clientTokenList)
        {
            //分发Token请求
            clientToken.ClientToken(content);
        }
    }
}