public class UdpInit
{
    [AddRequestCode(RequestCode.UdpInit, RequestType.Server)]
    public void OnUdpInit(string data, ClientSocket clientSocket)
    {
        clientSocket.UdpSend(RequestCode.UdpInit, "0");
    }
}