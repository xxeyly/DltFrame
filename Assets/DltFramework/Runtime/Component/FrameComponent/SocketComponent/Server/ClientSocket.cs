using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

    public int FrameIndex = -1;

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
                socket.BeginReceive(_msg.Data, _msg.StoredSize, _msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
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

            int dataCount = socket.EndReceive(ar);
            if (dataCount > 0)
            {
                //读取消息
                _msg.ReadMessage(dataCount, ExecuteReflection);
            }

            //递归接受
            StartReceiveCallback();
        }
        catch (Exception e)
        {
            Console.WriteLine("客户端异常:" + e);
        }
    }

    public void ExecuteReflection(int requestCode, byte[] data)
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
        TcpSend(requestCode, Encoding.UTF8.GetBytes(data));
    }

    public void TcpSend(int requestCode, byte[] data)
    {
        byte[] bytes = _msg.PackData(requestCode, data);
        socket.Send(bytes);
    }

    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    public void TcpSend(int requestCode, int data)
    {
        TcpSend(requestCode, data.ToString());
    }

    public bool UdpIsReady()
    {
        return remoteIpEndPoint != null;
    }

    public void UdpSend(byte[] bytes)
    {
        if (remoteIpEndPoint != null)
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
    }

    public void CloseConnection()
    {
        Console.WriteLine(socket.RemoteEndPoint + "断开连接...");
        //玩家退出房间
        ServerRoomManager.ExitRoom(token);
        //用户列表移除自身
        ClientSocketManager.RemoveClientSocket(this);
        //心跳列表移除自身
        HeartBeat.RemoveClientSocket(this);
        socket.Close();
    }
}