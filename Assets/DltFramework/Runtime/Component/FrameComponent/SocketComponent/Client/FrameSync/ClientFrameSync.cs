using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;

public class ClientFrameSync
{
    public static FrameRecord frameRecord = new FrameRecord();
    private static List<IFrameSync> frameSyncList = new List<IFrameSync>();
    private static FrameRecordData frameRecordData = new FrameRecordData();
    public static bool frameSyncBool;
    public static bool isForecast = false;

    private static void OnFrameSync()
    {
        if (!frameSyncBool)
        {
            return;
        }

        foreach (IFrameSync frameSync in frameSyncList)
        {
            frameSync.FrameSync();
        }

        Debug.Log(FrameRecord.frameIndex);

        frameRecordData.id = ClientSocketFrameComponent.Instance.Token;
        ClientSocketFrameComponent.Instance.UdpStartSend(frameRecordData, isForecast);
        frameRecordData = new FrameRecordData();
    }

    //执行帧逻辑
    public static void ExecuteFrameLogic(List<FrameRecordData> frameRecordDataList)
    {
        foreach (FrameRecordData frameRecordData in frameRecordDataList)
        {
            ExecuteFrameLogic(frameRecordData);
        }
    }

    //执行帧逻辑
    public static void ExecuteFrameLogic(FrameRecordData frameRecordData)
    {
        if (frameRecordData.id == ClientSocketFrameComponent.Instance.Token)
        {
            if (frameRecordData.create)
            {
                Debug.Log("创建");
                ListenerFrameComponent.Instance.playerSceneComponent.CreatePlayer();
            }

            if (frameRecordData.exit)
            {
                ListenerFrameComponent.Instance.playerSceneComponent.DeletePlayer();
            }

            ListenerFrameComponent.Instance.playerSceneComponent.PlayerMove(frameRecordData);
        }
        else
        {
            if (frameRecordData.create)
            {
                ListenerFrameComponent.Instance.playerSceneComponent.CreateOtherPlayer(frameRecordData);
            }

            if (frameRecordData.exit)
            {
                ListenerFrameComponent.Instance.playerSceneComponent.DeleteOtherPlayer(frameRecordData);
            }
        }
    }

    //添加到帧同步
    public static void AddFrameRecordData(FrameRecordData frameRecordData, bool IsForecast = true)
    {
        ClientFrameSync.frameRecordData = frameRecordData;
        isForecast = IsForecast;
    }

    //解析数据
    public static void ExecuteReflection(int frameIndex, string data)
    {
        List<FrameRecordData> serverFrameRecordDataList = JsonUtil.FromJson<List<FrameRecordData>>(data);
        //如果还处于回放模式,要一致记录接收的数据
        if (RecordReplays.isReplay)
        {
            RecordReplays.AddFrameData(serverFrameRecordDataList);
        }
        else
        {
            //如果有保存的当前帧记录表示本地预测帧和服务器帧保持一致
            if (FrameRecord.ContainsFrameIndex(frameIndex))
            {
                //本地的数据与服务器相同
                if (IsSameFrame(FrameRecord.GetFrameRecordDataGroup(frameIndex).frameRecordData, serverFrameRecordDataList))
                {
                    Debug.Log("本地数据与服务器相同");
                    if (FrameRecord.IsNoForecastFrameRecordData(frameIndex))
                    {
                        // Debug.Log("当前帧不参与预测");
                        //不参与预测的帧
                        ExecuteFrameLogic(FrameRecord.GetNoForecastFrameRecordData(frameIndex));
                    }

                    Snapshot.AddSnapshot();
                }
                else
                {
                    //读取上一帧的快照
                    Snapshot.GetSnapshot(frameIndex - 1);
                    //直接执行服务器逻辑
                    ExecuteFrameLogic(serverFrameRecordDataList);
                }
            }
            else
            {
                Debug.Log("本地无数据");
                //直接执行服务器逻辑
                ExecuteFrameLogic(serverFrameRecordDataList);
                Snapshot.AddSnapshot();
            }
        }

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
    public static long oldTime;

    //当前使劲按
    public static long currentTime;

    //等待使劲按
    public static long waitServerTime;

    //偏差计算
    public static bool serverTimeOffsetBool;

    //计时器
    public static bool timerBool;


    public static void Update()
    {
        TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        currentTime = (long)mTimeSpan.TotalMilliseconds;
        if (serverTimeOffsetBool)
        {
            // Debug.Log("当前时间:" + currentTime);
            // Debug.Log("旧的时间:" + oldTime);
            if (currentTime - oldTime >= waitServerTime)
            {
                int timeOffset = (int)(currentTime - oldTime - waitServerTime);

                // Debug.Log("消除后的正确时间:" + currentTime);
                // Debug.Log("偏差时间:" + timeOffset);
                serverTimeOffsetBool = false;
                oldTime = currentTime;
                //消除偏差
                oldTime -= timeOffset;
                timerBool = true;
                OnFrameSync();
                Debug.Log(currentTime + ":" + FrameRecord.frameIndex);
            }
        }

        if (timerBool)
        {
            //时间偏差
            if (currentTime - oldTime >= 60)
            {
                int timeOffset = (int)(currentTime - oldTime) - 60;
                oldTime = currentTime;
                //有时可能会多出来1-2,减去偏差,下次不用计算了
                oldTime -= timeOffset;
                FrameRecord.frameIndex += 1;
                Debug.Log(currentTime + ":" + FrameRecord.frameIndex);
                OnFrameSync();
            }
        }
    }

    public static void StartFrameSync(FrameInitData frameInitData)
    {
        //开启帧同步
        TimeSpan TimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
        long currentTime = (long)TimeSpan.TotalMilliseconds;
        // Debug.Log("当前时间:" + currentTime);
        //传输过来用了多少时间
        long transmissionInterval = currentTime - frameInitData.currentTime;
        // Debug.Log("传输过来用了多少时间:" + transmissionInterval);
        //服务器传输时下个帧的时间
        long serverTransmissionNextFrameTime = 60 - (frameInitData.currentTime - frameInitData.startTime) % 60;
        // Debug.Log("服务器下个时间:" + serverTransmissionNextFrameTime);

        //服务器下一帧时间
        long serverNextFrameTime;
        int serverNextFrameIndex;
        //如果传输时间大于服务器下一帧时间
        //时间大于剩余的时间,要进行补帧
        //如当前服务器是10帧第50毫秒,传输时间是60毫秒,客户端就会错过第11帧的数据
        if (transmissionInterval >= serverTransmissionNextFrameTime)
        {
            // Debug.Log("传输时间大于服务器下一帧时间");
            //补充服务器帧
            frameInitData.currentTime += serverTransmissionNextFrameTime;
            frameInitData.frameIndex += 1;
            //剩下的帧数
            transmissionInterval -= serverTransmissionNextFrameTime;

            // Debug.Log("剩下传输时间:" + transmissionInterval);
            serverNextFrameTime = frameInitData.currentTime + (transmissionInterval / 60 + 1) * 60;
            serverNextFrameIndex = frameInitData.frameIndex += (int)(transmissionInterval / 60 + 1);
        }
        else
        {
            //时间小于不用进行补帧,服务器还没到一帧发送的时间,我们已经向服务器发了最新数据
            serverNextFrameTime = frameInitData.currentTime + serverTransmissionNextFrameTime;
            serverNextFrameIndex = frameInitData.frameIndex += 1;

            // Debug.Log("服务器下个时间:" + serverNextFrameTime + "时间指针:" + serverNextFrameIndex);
        }

        //等待服务器的剩余时间
        long waitServerTime = serverNextFrameTime - currentTime;
        StartFrameSync(waitServerTime, currentTime, serverNextFrameIndex);
    }

    public static void StartFrameSync(long waitServerTime, long oldTime, int frameIndex)
    {
        FrameRecord.frameIndex = frameIndex;
        ClientFrameSync.waitServerTime = waitServerTime;
        ClientFrameSync.oldTime = oldTime;
        // Debug.Log(waitServerTime + ":" + ClientFrameSync.oldTime);
        serverTimeOffsetBool = true;
    }
}