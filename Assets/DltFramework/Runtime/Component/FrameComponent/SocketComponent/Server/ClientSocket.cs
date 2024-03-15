using System;
using System.Net;
using System.Net.Sockets;

public class ClientSocket
{
    public int token;
    private Message _msg;

    //服务器
    public ServerSocketFrameComponent server;

    //客户端Socket
    public Socket socket;
    public UdpClient udpClient;

    //心跳维持
    public bool isHeartBeat = true;

    public IPEndPoint remoteIpEndPoint;

    public int FrameIndex;

    public ClientSocket()
    {
        _msg = new Message();
    }

    public void SetServer(ServerSocketFrameComponent server)
    {
        this.server = server;
    }


    public void SetTcpClient(Socket socket)
    {
        this.socket = socket;
        StartReceiveCallback();
    }

    public void SetUdpClient(UdpClient udpClient)
    {
        this.udpClient = udpClient;
    }

    public void SetUdpClient(IPEndPoint remoteIpEndPoint)
    {
        // Console.WriteLine("客户端地址:" + remoteIpEndPoint);
        this.remoteIpEndPoint = remoteIpEndPoint;
    }

    /// <summary>
    /// 开始异步接受
    /// </summary>
    private void StartReceiveCallback()
    {
        try
        {
            //如果是连接状态
            if (socket.Connected)
            {
                socket.BeginReceive(_msg.Data, _msg.StartIndex, _msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
            }
        }
        catch (Exception e)
        {
            CloseConnection();
        }
    }

    /// <summary>
    /// 异步接收
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (socket == null || socket.Connected == false || ar == null)
            {
                return;
            }

            int count = socket.EndReceive(ar);
            if (count > 0)
            {
                //读取消息
                _msg.ReadMessage(count, ExecuteReflection);
            }

            //递归接受
            StartReceiveCallback();
        }
        catch (Exception e)
        {
            Console.WriteLine("客户端异常:" + e);
        }
    }

    public void ExecuteReflection(int requestCode, string data)
    {
        server.ExecuteReflection(requestCode, data, this);
    }


    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    public void TcpSend(int requestCode, string data)
    {
        byte[] bytes = _msg.PackData(requestCode, data);
        socket.Send(bytes);
    }

    public void UdpSend(int frameIndex, string data)
    {
        byte[] bytes = Message.UdpPackData(frameIndex, data);
        UdpSend(bytes);
    }

    public void UdpSend(byte[] bytes)
    {
        try
        {
            udpClient.Send(bytes, bytes.Length, remoteIpEndPoint);
            // Console.WriteLine("发送帧数据到:" + remoteIpEndPoint + "数据:" + Encoding.UTF8.GetString(bytes));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void CloseConnection()
    {
        ServerPlayerMove.OnPlayerExit(this);
        Console.WriteLine(socket.RemoteEndPoint + "断开连接...");
        //用户列表移除自身
        ClientSocketManager.RemoveClientSocket(this);
        //心跳列表移除自身
        HeartBeat.RemoveClientSocket(this);
        socket.Close();
    }
}