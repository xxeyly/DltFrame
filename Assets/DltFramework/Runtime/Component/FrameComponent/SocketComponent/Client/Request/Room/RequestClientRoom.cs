using System.Collections;
using System.Collections.Generic;
using DltFramework;
using UnityEngine;

public class RequestClientRoom
{
    [AddRequestCode(RequestCode.GetRoom, RequestType.Client)]
    public void OnGetRoom(string roomData)
    {
        List<IRequestClientRoomGetRoom> requestClientRoomGetRoomList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IRequestClientRoomGetRoom>();
        List<ServerRoomData> serverRoomDataList = new List<ServerRoomData>();
        if (roomData == "[]")
        {
            //没有房间
        }
        else
        {
            serverRoomDataList = JsonUtility.FromJson<List<ServerRoomData>>(roomData);
        }

        foreach (IRequestClientRoomGetRoom requestClientRoomGetRoom in requestClientRoomGetRoomList)
        {
            requestClientRoomGetRoom.OnGetRoom(serverRoomDataList);
        }
    }

    [AddRequestCode(RequestCode.CreateRoomSuccessFully, RequestType.Client)]
    public void OnCreateRoomSuccessFully(string data)
    {
        ServerRoomData serverRoomData = JsonUtility.FromJson<ServerRoomData>(data);
        ListenerFrameComponent.Instance.serverRoomList.AddServerRoomListItem(serverRoomData);
        ViewFrameComponent.Instance.HideView(typeof(ClientCreateRoom));
        
        ListenerFrameComponent.Instance.serverRoomList.JoinRoom(serverRoomData);
    }

    [AddRequestCode(RequestCode.EnterRoomFailedRoomFull, RequestType.Client)]
    public void OnEnterRoomFailedRoomFull(string data)
    {
        Debug.Log("房间已满");
    }

    [AddRequestCode(RequestCode.EnterRoomFailedRoomPasswordError, RequestType.Client)]
    public void OnEnterRoomFailedRoomPasswordError(string data)
    {
        Debug.Log("密码错误");
    }

    [AddRequestCode(RequestCode.EnterRoomSuccessFully, RequestType.Client)]
    public void OnEnterRoomSuccessFully(string data)
    {
        Debug.Log("进入房间成功");
        ViewFrameComponent.Instance.HideView(typeof(ClientJoinRoom));
        ViewFrameComponent.Instance.HideView(typeof(ServerRoomList));
        ViewFrameComponent.Instance.ShowView(typeof(ClientRoomReady));
    }

    [AddRequestCode(RequestCode.EnterRoomGetRoomPlayer, RequestType.Client)]
    public void OnEnterRoomGetRoomPlayer(string data)
    {
        Dictionary<int, bool> roomPlayerList = JsonUtility.FromJson<Dictionary<int, bool>>(data);
        ListenerFrameComponent.Instance.clientRoomReady.SetRoomPlayer(roomPlayerList);
    }
}