public enum RequestCode
{
    None = 0,

    //连接码
    ConnectCode = 1,

    //心跳
    HeartbeatPacket = 2,

    //主动断开连接
    Disconnect = 3,

    //UDP初始化
    UdpInit,

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
    OtherPlayerExit,
}