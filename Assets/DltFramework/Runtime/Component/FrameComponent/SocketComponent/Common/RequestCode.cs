public enum RequestCode
{
    None = 0,
    ConnectSuccessFully,

    //连接码
    Token,

    //心跳
    HeartbeatPacket,

    //主动断开连接
    Disconnect,

    //进入
    EnterGame,

    //帧同步
    FrameSync,

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