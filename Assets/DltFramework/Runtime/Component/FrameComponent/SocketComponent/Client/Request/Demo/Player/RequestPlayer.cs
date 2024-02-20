using System;
using DltFramework;
using UnityEngine;

public class RequestPlayer
{
    [AddRequestCode(RequestCode.PlayerInit, RequestType.Client)]
    public void OnPlayerInit(string data)
    {
        ListenerFrameComponent.Instance.playerSceneComponent.InitPlayer();
    }

    [AddRequestCode(RequestCode.PlayerMove, RequestType.Client)]
    public void OnPlayerMove(string data)
    {
        PlayerData playerData = JsonUtil.FromJson<PlayerData>(data);
        ListenerFrameComponent.Instance.playerSceneComponent.PlayerMove(playerData);
    }


    [AddRequestCode(RequestCode.OtherPlayerInit, RequestType.Client)]
    public void OnOtherPlayerInit(string data)
    {
        ListenerFrameComponent.Instance.playerSceneComponent.InitOtherPlayer(Convert.ToInt32(data));
    }

    [AddRequestCode(RequestCode.OtherPlayerMove, RequestType.Client)]
    public void OnOtherPlayerMove(string data)
    {
        ListenerFrameComponent.Instance.playerSceneComponent.OtherPlayerMove(JsonUtil.FromJson<PlayerData>(data));
    }

    [AddRequestCode(RequestCode.OtherPlayerExit, RequestType.Client)]
    public void OnOtherPlayerExit(string data)
    {
        ListenerFrameComponent.Instance.playerSceneComponent.OtherPlayerExit(Convert.ToInt32(data));
    }
}