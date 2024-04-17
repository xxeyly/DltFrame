using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ServerRoomData
{
    /// <summary>
    /// 房间Id
    /// </summary>
    public int roomId;

    /// <summary>
    /// 房间名称
    /// </summary>
    public string roomName;

    /// <summary>
    /// 房间玩家数量
    /// </summary>
    public int roomPlayerCount;

    /// <summary>
    /// 房间玩家上限数量
    /// </summary>
    public int roomPlayerMaxCount;

    /// <summary>
    /// 房间密码
    /// </summary>
    public string roomPassword;

    /// <summary>
    /// 房主
    /// </summary>
    public int RoomOwnerToken;
}