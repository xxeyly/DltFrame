using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using DltFramework;
using UnityEngine;

public class ConnectSuccessFully
{
    [AddRequestCode(RequestCode.ConnectSuccessFully, RequestType.Client)]
    public async void OnConnectSuccessFully(byte[] data)
    {
        //获得所有客户端IConnectSuccessFully
        List<IConnectSuccessFully> connectSuccessFullyList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IConnectSuccessFully>();
        foreach (IConnectSuccessFully connectSuccessFully in connectSuccessFullyList)
        {
            //分发连接成功请求
            connectSuccessFully.ConnectSuccessFully();
        }

        //发送Token
        if (File.Exists(DataFrameComponent.Path_DeviceStorage() + "/Token.txt"))
        {
            ClientSocketFrameComponent.Instance.Send(RequestCode.Token, FileOperationComponent.GetTextToLoad(DataFrameComponent.Path_DeviceStorage() + "/Token.txt"));
        }
        else
        {
            ClientSocketFrameComponent.Instance.Send(RequestCode.Token, "0");
        }
    }

}