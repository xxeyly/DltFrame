using System;

public class HeartBeat
{
    //旧的心跳时间
    private long oldHeartBeatTime;

    //新的心跳时间
    private long newHeartBeatTime;

    //心跳间隔
    private long heartBeatInterval = 1;

    //创建心跳包
    public void CreateHeartBeat()
    {
        //创建心跳包,等于当前时间
        oldHeartBeatTime = GetTimeStamp();
        //创建心跳包
        while (true)
        {
            if (GetTimeStamp() - oldHeartBeatTime >= heartBeatInterval)
            {
                oldHeartBeatTime = GetTimeStamp();
            }
        }
    }

    private long GetTimeStamp()
    {
        return Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
    }

    //发送心跳包
    public void SendHeartBeat()
    {
        //发送心跳包
    }
}