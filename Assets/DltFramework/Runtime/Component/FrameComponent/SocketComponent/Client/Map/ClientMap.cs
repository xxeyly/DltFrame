using System;
using UnityEngine;

public class ClientMap
{
    //旧的时间
    public long serverOldTime;
    public long clientOldTime;

    //当前时间
    public long currentTime;

    //等待时间
    public long waitServerTime;

    //偏差计算
    public bool serverTimeOffsetBool;

    //计时器
    public bool serverFrameBool;

    public bool clientFrameBool;

    //服务器帧索引
    public int serverFrameIndex;

    //客户端帧索引
    public int clientFrameIndex;

    //帧间隔
    //客户端帧间隔会比服务器满一半,为了错开帧,避免客户端和服务器同时发送帧数据
    public int frameInterval = 60;

    public void MapInit()
    {
        ClientSocketFrameComponent.Instance.onUpdate += Update;
    }

    public void StartFrameSync(FrameInitData frameInitData)
    {
        //开启帧同步
        TimeSpan TimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        currentTime = (long)TimeSpan.TotalMilliseconds;
        // Debug.Log("当前时间:" + currentTime);
        // Debug.Log("当前时间:" + frameInitData.currentTime);
        // Debug.Log("当前服务器帧:" + frameInitData.frameIndex);
        clientFrameIndex = frameInitData.FrameIndex;
        Debug.Log("当前客户端帧:" + clientFrameIndex);
        //传输过来用了多少时间
        long transmissionInterval = currentTime - frameInitData.CurrentTime;
        // Debug.Log("传输过来用了多少时间:" + transmissionInterval);
        //服务器传输时到下个帧的所需时间
        //加入服务器发送时帧是59_20,那么下个帧是60_00,那么下个帧时间是60-20=40
        long serverTransmissionNextFrameTime = frameInterval - (frameInitData.CurrentTime - frameInitData.StartTime) % frameInterval;
        // Debug.Log("服务器到下个帧时间:" + serverTransmissionNextFrameTime);

        //服务器到下一帧所需时间
        long serverNextFrameTime;
        //服务当前帧
        int serverCurrentFrameIndex;
        //如果传输时间大于服务器到下一帧所需时间
        //时间大于剩余的时间,要进行补帧
        if (transmissionInterval >= serverTransmissionNextFrameTime)
        {
            //补充上一服务器帧剩余的帧
            frameInitData.CurrentTime += serverTransmissionNextFrameTime;
            frameInitData.FrameIndex += 1;
            //剩下的帧数
            transmissionInterval -= serverTransmissionNextFrameTime;

            // Debug.Log("剩下传输时间:" + transmissionInterval);
            serverNextFrameTime = frameInitData.CurrentTime + (transmissionInterval / frameInterval + 1) * frameInterval;
            serverCurrentFrameIndex = frameInitData.FrameIndex += (int)(transmissionInterval / frameInterval);
            // Debug.Log("服务器到下个帧时间:" + serverNextFrameTime + "当前时间指针:" + serverCurrentFrameIndex);
        }
        else
        {
            //时间小于不用进行补帧,服务器还没到一帧发送的时间,我们已经向服务器发了最新数据
            serverNextFrameTime = frameInitData.CurrentTime + serverTransmissionNextFrameTime;
            serverCurrentFrameIndex = frameInitData.FrameIndex;

            // Debug.Log("服务器到下个帧时间:" + serverNextFrameTime + "当前时间指针:" + serverCurrentFrameIndex);
        }

        //等待服务下一帧的时间
        waitServerTime = serverNextFrameTime - currentTime;
        serverOldTime = currentTime;
        serverFrameIndex = serverCurrentFrameIndex;
        serverTimeOffsetBool = true;
    }

    public void Update()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        currentTime = (long)mTimeSpan.TotalMilliseconds;
        //与服务器偏移时间
        if (serverTimeOffsetBool)
        {
            // Debug.Log("当前时间:" + currentTime);
            // Debug.Log("旧的时间:" + oldTime);
            if (currentTime - serverOldTime >= waitServerTime)
            {
                int timeOffset = (int)(currentTime - serverOldTime - waitServerTime);

                // Debug.Log("消除后的正确时间:" + currentTime);
                // Debug.Log("偏差时间:" + timeOffset);
                serverTimeOffsetBool = false;
                serverOldTime = currentTime;
                //消除偏差
                serverOldTime -= timeOffset;
                serverFrameBool = true;
                serverFrameIndex += 1;
                //单数 客户端向服务器发数据
                clientFrameBool = true;
                clientOldTime = currentTime + frameInterval / 2;
            }
        }

        //服务器帧同步
        if (serverFrameBool)
        {
            //时间偏差
            if (currentTime - serverOldTime >= frameInterval)
            {
                int timeOffset = (int)(currentTime - serverOldTime) - frameInterval;
                serverOldTime = currentTime;
                //有时可能会多出来1-2,减去偏差,下次不用计算了
                serverOldTime -= timeOffset;
                serverFrameIndex += 1;
                //客户端向服务器发数据的时间
                //时间取一半
                clientFrameBool = true;
                clientOldTime = currentTime + frameInterval / 2;
                // Debug.Log(currentTime + ":" + serverFrameIndex);
            }
        }

        //客户端向服务器发送数据
        if (clientFrameBool)
        {
            if (currentTime >= clientOldTime)
            {
                clientFrameBool = false;
                //TODO 向服务器发送数据
            }
        }
    }
}