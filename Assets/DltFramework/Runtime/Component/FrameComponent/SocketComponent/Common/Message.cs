using System;
using System.Linq;
using System.Net;
using System.Text;

public class Message
{
    private byte[] data = new byte[1024];
    private int storedSize = 0; //我们存取了多少个字节的数据在数组里面

    public byte[] Data
    {
        get { return data; }
    }

    public int StoredSize
    {
        get { return storedSize; }
    }

    public int RemainSize
    {
        get { return data.Length - storedSize; }
    }

    //Client
    public static void UdpReadMessage(byte[] data, Action<int, string> processDataCallback)
    {
        int topData = BitConverter.ToInt32(data, 0);
        string s = Encoding.UTF8.GetString(data, 4, data.Length - 4);
        processDataCallback(topData, s);
    }

    //Server
    public static void UdpReadMessage(byte[] data, IPEndPoint ipEndPoint, Action<int, string, IPEndPoint> processDataCallback)
    {
        int topData = BitConverter.ToInt32(data, 0);
        string s = Encoding.UTF8.GetString(data, 4, data.Length - 4);
        processDataCallback(topData, s, ipEndPoint);
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    public void ReadMessage(int dataCount, Action<int, byte[]> processDataCallback)
    {
        storedSize += dataCount;
        if (storedSize >= data.Length)
        {
            Array.Resize(ref data, storedSize * 2);
        }

        while (true)
        {
            //小于数据长度+请求码,说明消息不完善,等待下一次消息
            if (storedSize < 4 + 4) return;
            //数据的总长度
            int contentAmount = BitConverter.ToInt32(data, 0);
            // Console.WriteLine("数据总长度:" + contentAmount);
            //请求码
            int requestCode = BitConverter.ToInt32(data, 4);
            // Console.WriteLine("请求码:" + requestCode);
            // Debug.Log(count);
            //内容长度
            int dataAmount = contentAmount - 4 - 4;
            // Console.WriteLine("内容长度:" + dataAmount);
            byte[] content = new byte[dataAmount];
            for (int i = 0; i < dataAmount; i++)
            {
                content[i] = data[i + 8];
            }
            processDataCallback(requestCode, content);
            Array.Copy(data, contentAmount, data, 0, contentAmount);
            storedSize -= contentAmount;
           
        }
    }

    /// <summary>
    /// 包装数据
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] PackData(int requestCode, string data)
    {
        return PackData(requestCode, Encoding.UTF8.GetBytes(data));
    }

    public byte[] PackData(int requestCode, byte[] data)
    {
        //请求码:长度4
        byte[] requestCodeBytes = BitConverter.GetBytes(requestCode);
        // Console.WriteLine("请求码:" + requestCodeBytes.Length);
        //字符串长度:长度根据内容
        // Console.WriteLine("数据长度:" + data.Length);
        //数据总长度 4数据长度+4请求码+数据长度
        int contentAmount = 4 + 4 + data.Length;
        //长度字节
        byte[] dataAmountBytes = BitConverter.GetBytes(contentAmount);
        // Console.WriteLine("内容总长度:" + contentAmount);
        //返回组装成功的数据
        byte[] sendData = dataAmountBytes.Concat(requestCodeBytes).ToArray().Concat(data).ToArray();
        return sendData;
    }

    public static byte[] UdpPackData(int frameIndex, string data)
    {
        //帧索引
        byte[] requestCodeBytes = BitConverter.GetBytes(frameIndex);
        // Debug.Log(requestCodeBytes.Length);
        //字符串长度
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        // Debug.Log(dataAmountBytes.Length);
        //返回组装成功的数据
        return requestCodeBytes.ToArray().Concat(dataBytes).ToArray();
    }
}