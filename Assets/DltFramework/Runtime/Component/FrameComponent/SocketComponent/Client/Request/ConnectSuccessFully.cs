using System;
using System.IO;
using Cysharp.Threading.Tasks;
using DltFramework;
using UnityEngine;

public class ConnectSuccessFully
{
    [AddRequestCode(RequestCode.ConnectSuccessFully, RequestType.Client)]
    public async void OnConnectSuccessFully(string data)
    {
        Debug.Log("连接成功...");
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