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

    //获得房间
    public const int GetRoom = 100;


    //创建房间
    public const int CreateRoom = 102;

    //进入房间
    public const int EnterRoom = 101;

    //进入房间失败,房间已满
    public const int EnterRoomFailedRoomFull = 103;

    //进入房间失败,密码错误
    public const int EnterRoomFailedRoomPasswordError = 104;

    //进入房间成功
    public const int EnterRoomSuccessFully = 105;

    //进入房间,获得房间玩家
    public const int EnterRoomGetRoomPlayer = 106;

    //创建房间成功
    public const int CreateRoomSuccessFully = 7;

    //帧同步
    public const int FrameSync = 10000;

    //聊天室
    public const int ChatRoom = 100001;

    //其他角色退出
    public const int OtherPlayerExit = 100013;
}