using System;
using System.Net.Sockets;

public class ClientSocket
{
    private Message _msg;

    //服务器
    private ServerSocketFrameComponent _server;

    //客户端Socket
    public Socket socket;

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
        //如果是连接状态
        if (socket.Connected)
        {
            socket.BeginReceive(_msg.Data, _msg.StartIndex, _msg.RemainSize, SocketFlags.None,
                ReceiveCallback, null);
        }
    }

    /// <summary>
    /// 异步接受
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (socket == null || socket.Connected == false)
            {
                return;
            }

            int count = socket.EndReceive(ar);
            //没有数据
            if (count == 0)
            {
                //关闭连接
                CloseConnection();
            }

            //读取消息
            _msg.ReadMessage(count, _server.ExecuteReflection);
            //递归接受
            StartReceiveCallback();
        }
        catch (Exception e)
        {
            CloseConnection();
            Console.WriteLine(e);
        }
    }


    private void CloseConnection()
    {
    }
}