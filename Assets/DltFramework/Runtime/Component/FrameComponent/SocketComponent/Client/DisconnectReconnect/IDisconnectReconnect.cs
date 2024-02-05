public interface IDisconnectReconnect
{
    //断线重连
    void DisconnectReconnect(int remainderCount);

    //断线重连成功
    void OnDisconnectReconnectSuccess();

    //断线重连失败
    void OnDisconnectReconnectFail();
}