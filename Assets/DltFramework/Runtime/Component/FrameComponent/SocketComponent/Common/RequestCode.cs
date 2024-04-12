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
    public const int Room_GetRoom = 100;

    //创建房间
    public const int Room_CreateRoom = 102;

    //进入房间
    public const int Room_EnterRoom = 101;

    //进入房间失败,房间已满
    public const int Room_EnterRoomFailedRoomFull = 103;

    //进入房间失败,房间不存在
    public const int Room_EnterRoomFailedRoomNotExistent = 104;

    //进入房间失败,密码错误
    public const int Room_EnterRoomFailedRoomPasswordError = 105;

    //进入房间成功
    public const int Room_EnterRoomSuccessFully = 106;

    //进入房间,获得房间玩家
    public const int Room_GetRoomPlayer = 107;

    //房间玩家准备
    public const int Room_Ready = 108;

    //房间玩家退出
    public const int Room_ExitRoom = 109;

    //房间玩家退出成功
    public const int Room_ExitRoomSuccessFully = 110;

    //房间其他玩家进入
    public const int Room_OtherPlayerEnterRoom = 111;

    //房间其他玩家准备
    public const int Room_OtherPlayerReady = 112;

    //房间其他玩家退出
    public const int Room_OtherPlayerExitRoom = 113;

    //创建房间成功
    public const int Room_CreateRoomSuccessFully = 7;

    //帧同步
    public const int FrameSync = 10000;

    //聊天室
    public const int ChatRoom = 100001;

    //其他角色退出
    public const int OtherPlayerExit = 100013;
}