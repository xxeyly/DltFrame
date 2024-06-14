using System;
using System.Collections.Generic;
using DltFramework;
using HotFix;
using UnityEngine;

public class ClientFrameSync
{
    private static List<IFrameSyncCreate> _frameSyncCreate = new List<IFrameSyncCreate>();
    private static List<IFrameSyncPush> _frameSyncPush = new List<IFrameSyncPush>();
    private static List<IFrameSyncPull> _frameSyncPull = new List<IFrameSyncPull>();

    private static FrameRecordData sendFrameRecordData = new FrameRecordData();
    private static FrameRecordData sendEmptyFrameRecordData = new FrameRecordData();

    public static bool isForecast;

    //服务器帧索引
    public static int serverFrameIndex;

    //客户端帧索引
    public static int clientFrameIndex;

    //帧间隔
    //客户端帧间隔会比服务器满一半,为了错开帧,避免客户端和服务器同时发送帧数据
    public static int frameInterval = 1000;

    public static void ClientFrameSyncInit()
    {
        _frameSyncCreate = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IFrameSyncCreate>();
        _frameSyncPush = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IFrameSyncPush>();
        _frameSyncPull = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IFrameSyncPull>();
    }


    //执行帧逻辑
    public static void ExecuteFrameLogic(List<FrameData> frameRecordDataList)
    {
        foreach (FrameData frameRecordData in frameRecordDataList)
        {
            //创建接口与别的接口不大一样,需要一个类去继承这个接口,然后去实例化,只有这个接口被创建出来了,才行执行后续操作
            /*if (frameRecordData.create)
            {
                foreach (IFrameSyncCreate frameSync in _frameSyncCreate)
                {
                    frameSync.Create(frameRecordData);
                }
            }
            else
            {
                foreach (IFrameSyncPull frameSync in _frameSyncPull)
                {
                    if (frameSync.id == frameRecordData.id)
                    {
                        frameSync.PullFrameRecordData(frameRecordData);
                    }
                }
            }*/
        }
    }

    //执行帧逻辑
    public static void ExecuteFrameLogic(FrameData frameRecordData)
    {
        /*//创建接口与别的接口不大一样,需要一个类去继承这个接口,然后去实例化,只有这个接口被创建出来了,才行执行后续操作
        if (frameRecordData.create)
        {
            foreach (IFrameSyncCreate frameSync in _frameSyncCreate)
            {
                frameSync.Create(frameRecordData);
            }
        }
        else
        {
            foreach (IFrameSyncPull frameSync in _frameSyncPull)
            {
                if (frameSync.id == frameRecordData.id)
                {
                    frameSync.PullFrameRecordData(frameRecordData);
                }
            }
        }*/
    }

    //添加到帧同步
    public static void AddFrameRecordData(FrameRecordData frameRecordData, bool IsForecast = true)
    {
        ClientFrameSync.sendFrameRecordData = frameRecordData;
        isForecast = IsForecast;
    }



    //解析数据
    public static void ExecuteReflection(int frameIndex, string data)
    {
        //更新当前客户端帧
        clientFrameIndex = frameIndex;

        FrameDataGroup frameDataGroup = JsonUtil.FromJson<FrameDataGroup>(data);
        //如果还处于回放模式,要一致记录接收的数据
        /*
        if (RecordReplays.isReplay)
        {
            RecordReplays.AddFrameData(frameDataGroup.frameRecordData);
        }
        else
        {
            //如果有保存的当前帧记录表示本地预测帧和服务器帧保持一致
            if (FrameRecord.ContainsFrameIndex(frameIndex))
            {
                //本地的数据与服务器相同
                if (IsSameFrame(FrameRecord.GetFrameRecordDataGroup(frameIndex).frameRecordData, frameDataGroup.frameRecordData))
                {
                    // Debug.Log("本地数据与服务器相同");
                    if (FrameRecord.IsNoForecastFrameRecordData(frameIndex))
                    {
                        Debug.Log("当前帧不参与预测");
                        //不参与预测的帧
                        ExecuteFrameLogic(FrameRecord.GetNoForecastFrameRecordData(frameIndex));
                    }
                    // Snapshot.AddSnapshot();
                }
                else
                {
                    //读取上一帧的快照
                    // Snapshot.GetSnapshot(frameIndex - 1);
                    //直接执行服务器逻辑
                    ExecuteFrameLogic(frameDataGroup.frameRecordData);
                }
            }
            else
            {
                // Debug.Log("本地无数据");
                //直接执行服务器逻辑
                ExecuteFrameLogic(frameDataGroup.frameRecordData);
                Snapshot.AddSnapshot();
            }
        }
        */

        //服务器帧数据
    }

    private static FrameRecordData FindThisFrameRecordData(List<FrameRecordData> frameRecordDataList)
    {
        foreach (FrameRecordData frameRecordData in frameRecordDataList)
        {
            if (frameRecordData.id == ClientSocketFrameComponent.Instance.Token)
            {
                return frameRecordData;
            }
        }

        return null;
    }

    //帧是否相同
    private static bool IsSameFrame(List<FrameRecordData> thisFrameRecordDataList, List<FrameRecordData> serverFrameRecordDataList)
    {
        //帧数据个数不一样
        if (thisFrameRecordDataList.Count != serverFrameRecordDataList.Count)
        {
            return false;
        }

        //帧比较
        foreach (FrameRecordData thisFrameRecordData in thisFrameRecordDataList)
        {
            FrameRecordData serverFrameRecordData = null;
            foreach (FrameRecordData ServerRecordData in serverFrameRecordDataList)
            {
                if (thisFrameRecordData.id == ServerRecordData.id)
                {
                    serverFrameRecordData = ServerRecordData;
                    break;
                }
            }

            //没有找到对应的帧
            if (serverFrameRecordData == null)
            {
                return false;
            }

            //帧数据不一样
            if (!FrameRecord.IsSameFrame(thisFrameRecordData, serverFrameRecordData))
            {
                return false;
            }
        }

        return true;
    }

    //旧的时间
    public static long serverOldTime;
    public static long clientOldTime;

    //当前时间
    public static long currentTime;

    //等待时间
    public static long waitServerTime;

    //偏差计算
    public static bool serverTimeOffsetBool;

    //计时器
    public static bool serverFrameBool;

    public static bool clientFrameBool;

    public static void Update()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        currentTime = (long)mTimeSpan.TotalMilliseconds;
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
                //单数 客户端向服务器发数据
                clientFrameBool = true;
                clientOldTime = currentTime + frameInterval / 2;
                // Debug.Log(currentTime + ":" + FrameRecord.serverFrameIndex);
            }
        }

        //客户端向服务器发送数据
        if (clientFrameBool)
        {
            if (currentTime >= clientOldTime)
            {
                clientFrameBool = false;
                foreach (IFrameSyncPush frameSync in _frameSyncPush)
                {
                    FrameRecordData tempFrameRecordData = frameSync.PushFrameRecordData();
                    if (tempFrameRecordData != null)
                    {
                        tempFrameRecordData.id = ClientSocketFrameComponent.Instance.Token;
                        //客户端发送的是本地已经存在的帧数据
                        //服务器第1帧发过来的是网络验证过后的1帧
                        /*if (clientFrameIndex < serverFrameIndex - 1)
                        {
                            sendEmptyFrameRecordData.id = ClientSocketFrameComponent.Instance.Token;
                            //只发送空数据
                            FrameRecord.ClientRecordFrameSyncData(tempFrameRecordData);
                            UdpStartSend(sendEmptyFrameRecordData);
                        }
                        else
                        {
                            //记录当前操作
                            // Debug.Log("服务器帧数" + FrameRecord.serverFrameIndex + "发送有效数据" + FrameRecord.clientFrameIndex + JsonUtil.ToJson(frameRecordData));
                            FrameRecord.ClientRecordFrameSyncData(tempFrameRecordData, isForecast);
                            UdpStartSend(tempFrameRecordData);
                        }*/
                    }
                }
            }
        }
    }

    public static void StartFrameSync(FrameInitData frameInitData)
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
}