public enum RequestCode
{
    None = 0,

    //心跳
    HeartbeatPacket = 1,

    //主动断开连接
    Disconnect = 2,

    //聊天室
    ChatRoom,

    //角色初始化
    PlayerInit,

    //角色移动
    PlayerMove,

    //角色退出
    PlayerExit,

    //其他角色初始化
    OtherPlayerInit,

    //其他角色移动
    OtherPlayerMove,

    //其他角色退出
    OtherPlayerExit
}