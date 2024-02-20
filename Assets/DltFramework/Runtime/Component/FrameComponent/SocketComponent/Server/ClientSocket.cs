using System;
using System.Net.Sockets;

public class ClientSocket
{
    public int clientSocketId;
    private Message _msg;

    //服务器
    private ServerSocketFrameComponent _server;

    //客户端Socket
    public Socket socket;

    //心跳维持
    public bool isHeartBeat = true;


    public ClientSocket(ServerSocketFrameComponent server, Socket socket)
    {
        _server = server;
        this.socket = socket;
        _msg = new Message();
        StartReceiveCallback();
    }

    /// <summary>
    /// 开始异步接受
    /// </summary>
    void StartReceiveCallback()
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

    public void ExecuteReflection(RequestCode requestCode, string data)
    {
        _server.ExecuteReflection(requestCode, data, this);
    }

    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    public void Send(RequestCode requestCode, string data)
    {
        // Console.WriteLine(requestCode + " " + data);
        byte[] bytes = _msg.PackData(requestCode, data);
        ServerFrameSync.AddFrameSync(this, bytes);
    }

    public void Send(byte[] bytes)
    {
        try
        {
            socket.Send(bytes);
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