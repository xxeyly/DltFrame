using System;
using System.Linq;
using System.Text;

public class Message
{
    private byte[] data = new byte[1024];
    private int startIndex = 0; //我们存取了多少个字节的数据在数组里面

    public byte[] Data
    {
        get { return data; }
    }

    public int StartIndex
    {
        get { return startIndex; }
    }

    public int RemainSize
    {
        get { return data.Length - startIndex; }
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    public void ReadMessage(int newDataAmount, Action<RequestCode, string> processDataCallback)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);
            if (startIndex - 4 >= count)
            {
                RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
                string s = Encoding.UTF8.GetString(data, 8, count - 4);
                processDataCallback(requestCode, s);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);

                startIndex -= (count + 4);
                
            }
            else
            {
                return;
            }
        }
    }

    /// <summary>
    /// 包装数据
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] PackData(RequestCode requestCode, string data)
    {
        //请求码
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        // Debug.Log(requestCodeBytes.Length);
        //字符串长度
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        // Debug.Log(dataBytes.Length);
        //数据总长度
        int dataAmount = requestCodeBytes.Length + dataBytes.Length;
        // Debug.Log(dataAmount);
        //长度字节
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        // Debug.Log(dataAmountBytes.Length);
        //返回组装成功的数据
        return dataAmountBytes.Concat(requestCodeBytes).ToArray().Concat(dataBytes).ToArray();
    }
}