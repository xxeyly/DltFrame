using System.Collections;
using System.Collections.Generic;
using DltFramework;
using HotFix;
using UnityEngine;

public class RequestClientRoom
{
    [AddRequestCode(RequestCode.Room_GetRoom, RequestType.Client)]
    public void Room_GetRoom(string roomData)
    {
        List<IRoom_GetRoom> requestClientRoomGetRoomList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_GetRoom>();
        List<ServerRoomData> serverRoomDataList = new List<ServerRoomData>();
        if (roomData == "[]")
        {
            //没有房间
        }
        else
        {
            serverRoomDataList = JsonUtil.FromJson<List<ServerRoomData>>(roomData);
        }

        foreach (IRoom_GetRoom requestClientRoomGetRoom in requestClientRoomGetRoomList)
        {
            requestClientRoomGetRoom.Room_GetRoom(serverRoomDataList);
        }
    }

    [AddRequestCode(RequestCode.Room_CreateRoomSuccessFully, RequestType.Client)]
    public void Room_CreateRoomSuccessFully(string data)
    {
        ServerRoomData serverRoomData = JsonUtil.FromJson<ServerRoomData>(data);
        List<IRoom_CreateRoomSuccessFully> requestClientRoomCreateRoomSuccessFullyList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_CreateRoomSuccessFully>();
        foreach (IRoom_CreateRoomSuccessFully requestClientRoomCreateRoomSuccessFully in requestClientRoomCreateRoomSuccessFullyList)
        {
            requestClientRoomCreateRoomSuccessFully.Room_CreateRoomSuccessFully(serverRoomData);
        }
    }

    [AddRequestCode(RequestCode.Room_EnterRoomFailedRoomNotExistent, RequestType.Client)]
    public void Room_EnterRoomFailedRoomNotExistent(string data)
    {
        List<IRoom_EnterRoomFailedRoomNotExistent> Room_EnterRoomFailedRoomNotExistent = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_EnterRoomFailedRoomNotExistent>();
        foreach (IRoom_EnterRoomFailedRoomNotExistent roomEnterRoomFailedRoomNotExistent in Room_EnterRoomFailedRoomNotExistent)
        {
            roomEnterRoomFailedRoomNotExistent.Room_EnterRoomFailedRoomNotExistent();
        }
    }

    [AddRequestCode(RequestCode.Room_EnterRoomFailedRoomFull, RequestType.Client)]
    public void Room_EnterRoomFailedRoomFull(string data)
    {
        List<IRoom_EnterRoomFailedRoomFull> requestClientRoomRoomFullList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_EnterRoomFailedRoomFull>();
        foreach (IRoom_EnterRoomFailedRoomFull requestClientRoomRoomFull in requestClientRoomRoomFullList)
        {
            requestClientRoomRoomFull.OnEnterRoomFailedRoomFull();
        }
    }

    [AddRequestCode(RequestCode.Room_EnterRoomFailedRoomPasswordError, RequestType.Client)]
    public void Room_EnterRoomFailedRoomPasswordError(string data)
    {
        List<IRoom_EnterRoomFailedRoomPasswordError> requestClientRoomEnterRoomPasswordErrorList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_EnterRoomFailedRoomPasswordError>();
        foreach (IRoom_EnterRoomFailedRoomPasswordError requestClientRoomEnterRoomPasswordError in requestClientRoomEnterRoomPasswordErrorList)
        {
            requestClientRoomEnterRoomPasswordError.Room_EnterRoomFailedRoomPasswordError();
        }

        Debug.Log("密码错误");
    }

    [AddRequestCode(RequestCode.Room_EnterRoomSuccessFully, RequestType.Client)]
    public void Room_EnterRoomSuccessFully(string data)
    {
        List<IRoom_EnterRoomSuccessFully> room_EnterRoomSuccessFully = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_EnterRoomSuccessFully>();
        ServerRoomData serverRoomData = JsonUtil.FromJson<ServerRoomData>(data);
        foreach (IRoom_EnterRoomSuccessFully roomEnterRoomSuccessFully in room_EnterRoomSuccessFully)
        {
            roomEnterRoomSuccessFully.Room_EnterRoomSuccessFully(serverRoomData);
        }
    }

    [AddRequestCode(RequestCode.Room_GetRoomPlayer, RequestType.Client)]
    public void Room_GetRoomPlayer(string data)
    {
        List<ServerRoomPlayerReadyState> serverRoomPlayerReadyStates = JsonUtil.FromJson<List<ServerRoomPlayerReadyState>>(data);
        ListenerFrameComponent.Instance.clientRoomReady.SetRoomPlayer(serverRoomPlayerReadyStates);
    }

    [AddRequestCode(RequestCode.Room_OtherPlayerEnterRoom, RequestType.Client)]
    public void Room_OtherPlayerEnterRoom(string data)
    {
        ServerRoomPlayerReadyState serverRoomPlayerReadyState = JsonUtil.FromJson<ServerRoomPlayerReadyState>(data);
        List<IRoom_OtherPlayerEnterRoom> IRoom_OtherPlayerEnterRoom = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_OtherPlayerEnterRoom>();
        foreach (IRoom_OtherPlayerEnterRoom roomOtherPlayerEnterRoom in IRoom_OtherPlayerEnterRoom)
        {
            roomOtherPlayerEnterRoom.Room_OtherPlayerEnterRoom(serverRoomPlayerReadyState);
        }
    }

    [AddRequestCode(RequestCode.Room_Ready, RequestType.Client)]
    public void Room_Ready(string data)
    {
        ServerRoomPlayerReadyState serverRoomPlayerReadyState = JsonUtil.FromJson<ServerRoomPlayerReadyState>(data);
        List<IRoom_Ready> IRoom_Ready = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_Ready>();
        foreach (IRoom_Ready roomReady in IRoom_Ready)
        {
            roomReady.Room_Ready(serverRoomPlayerReadyState);
        }
    }

    [AddRequestCode(RequestCode.Room_OtherPlayerReady, RequestType.Client)]
    public void Room_OtherPlayerReady(string data)
    {
        ServerRoomPlayerReadyState serverRoomPlayerReadyState = JsonUtil.FromJson<ServerRoomPlayerReadyState>(data);
        List<IRoom_OtherPlayerReady> IRoom_OtherPlayerReady = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_OtherPlayerReady>();
        foreach (IRoom_OtherPlayerReady roomOtherPlayerReady in IRoom_OtherPlayerReady)
        {
            roomOtherPlayerReady.Room_OtherPlayerReady(serverRoomPlayerReadyState);
        }
    }

    [AddRequestCode(RequestCode.Room_OtherPlayerExitRoom, RequestType.Client)]
    public void Room_OtherPlayerExitRoom(string data)
    {
        ServerRoomPlayerReadyState serverRoomPlayerReadyState = JsonUtil.FromJson<ServerRoomPlayerReadyState>(data);
        List<IRoom_OtherPlayerExitRoom> IRoom_OtherPlayerExitRoom = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_OtherPlayerExitRoom>();
        foreach (IRoom_OtherPlayerExitRoom roomOtherPlayerExitRoom in IRoom_OtherPlayerExitRoom)
        {
            roomOtherPlayerExitRoom.Room_OtherPlayerExitRoom(serverRoomPlayerReadyState);
        }
    }

    [AddRequestCode(RequestCode.Room_ExitRoomSuccessFully, RequestType.Client)]
    public void Room_ExitRoomSuccessFully(string data)
    {
        List<IRoom_ExitRoomSuccessFully> IRoom_ExitRoomSuccessFully = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRoom_ExitRoomSuccessFully>();
        foreach (IRoom_ExitRoomSuccessFully roomExitRoomSuccessFully in IRoom_ExitRoomSuccessFully)
        {
            roomExitRoomSuccessFully.Room_ExitRoomSuccessFully();
        }
    }
}