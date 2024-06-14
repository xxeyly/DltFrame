using System;
using System.Linq;
using System.Net;
using System.Text;
// using UnityEngine;

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
            // Debug.Log("剩余长度:" + storedSize);
            for (int i = 0; i < data.Length; i++)
            {
                // Debug.Log(data[i]);
            }
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

    public byte[] PackData(int requestCode, int data)
    {
        return PackData(requestCode, BitConverter.GetBytes(data));
    }


    public byte[] PackData(int requestCode, byte[] data)
    {
        //请求码:长度4个字节
        byte[] requestCodeBytes = BitConverter.GetBytes(requestCode);
        // Console.WriteLine("请求码:" + requestCodeBytes.Length);
        //字符串长度:长度根据内容
        // Console.WriteLine("数据长度:" + data.Length);
        //数据总长度 4字节长度+4个请求码字节长度+数据长度
        int contentAmount = 4 + 4 + data.Length;
        //长度字节
        byte[] dataAmountBytes = BitConverter.GetBytes(contentAmount);
        // Console.WriteLine("内容总长度:" + contentAmount);
        //返回组装成功的数据
        byte[] sendData = dataAmountBytes.Concat(requestCodeBytes).ToArray().Concat(data).ToArray();
        return sendData;
    }
}