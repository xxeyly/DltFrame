using DltFramework;

public class UdpSuccessFully
{
    [AddRequestCode(RequestCode.UdpInit, RequestType.Client)]
    public void OnUdpInit(string data)
    {
        ViewFrameComponent.Instance.HideView(typeof(ConnectServer));
        ViewFrameComponent.Instance.ShowView(typeof(EnterGame));
        //发送心跳包到服务器
        Heartbeat.StartHeartbeatDetection();
    }
}