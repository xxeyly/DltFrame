using System;

namespace SocketServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ServerSocketFrameComponent serverSocketFrameComponent = new ServerSocketFrameComponent();
            serverSocketFrameComponent.StartServer();
            Console.Read();
        }
    }
}