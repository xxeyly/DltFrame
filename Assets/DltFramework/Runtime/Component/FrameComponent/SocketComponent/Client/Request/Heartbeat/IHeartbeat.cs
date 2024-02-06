public interface IHeartbeat
{
    //心跳异常
    void HeartbeatAbnormal(int remainderCount);

    //心跳恢复正常
    void HeartbeatRestoreNormal();

    //心跳停止
    void HeartbeatStop();
    
    void HeartbeatNormal();
}