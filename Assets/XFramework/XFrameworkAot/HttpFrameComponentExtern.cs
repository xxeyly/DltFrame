using System.Runtime.InteropServices;

public class HttpFrameComponentExtern
{
    [DllImport("winInet.dll")] //引用外部库
    public static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved); //库中函数

    /// <summary>
    /// 判断连接状态
    /// </summary>
    public static bool IsConnected()
    {
        int dwFlag = new int();
        if (!InternetGetConnectedState(ref dwFlag, 0))
        {
            if ((dwFlag & 0x14) == 0)
            {
                return false;
            }
        }
        else
        {
            if ((dwFlag & 0x01) != 0)
            {
                return true;
            }
            else if ((dwFlag & 0x02) != 0)
            {
                return true;
            }
            else if ((dwFlag & 0x04) != 0)
            {
                return true;
            }
            else if ((dwFlag & 0x40) != 0)
            {
                return true;
            }
        }

        return false;
    }
}