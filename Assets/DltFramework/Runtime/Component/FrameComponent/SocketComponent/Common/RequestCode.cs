

public partial class RequestCode
{
    public const int None = 0;
    public const int ConnectSuccessFully = 1;

    //连接码
    public const int Token = 2;

    //心跳
    public const int HeartbeatPacket = 3;

    //主动断开连接
    public const int Disconnect = 4;

    //进入
    public const int EnterGame = 5;

    //帧同步
    public const int FrameSync = 6;

    //聊天室
    public const int ChatRoom = 7;

    //其他角色退出
    public const int OtherPlayerExit = 13;
}