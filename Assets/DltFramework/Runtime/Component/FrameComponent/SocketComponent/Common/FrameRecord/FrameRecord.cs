using System;
using System.Collections.Generic;

public class FrameRecord
{
    //帧记录
    public static List<FrameRecordDataGroup> frameRecord = new List<FrameRecordDataGroup>();

    //不参与预测的帧,一般指帧同步时不好回退的帧,比如UI交互等,这些帧不参与预测,等服务器返回了当前帧,再执行,这样可以保证帧同步的准确性
    public static Dictionary<int, FrameRecordData> noForecastFrameRecordDic = new Dictionary<int, FrameRecordData>();

    //是否是相同帧
    public static bool IsSameFrame(FrameRecordData frameRecordData1, FrameRecordData frameRecordData2)
    {
        return frameRecordData1.create == frameRecordData2.create &&
               frameRecordData1.exit == frameRecordData2.exit &&
               frameRecordData1.w == frameRecordData2.w &&
               frameRecordData1.a == frameRecordData2.a &&
               frameRecordData1.s == frameRecordData2.s &&
               frameRecordData1.d == frameRecordData2.d;
    }

    //是否是不参与预测的帧
    public static bool IsNoForecastFrameRecordData(int frameIndex)
    {
        return noForecastFrameRecordDic.ContainsKey(frameIndex);
    }

    //获取不参与预测的帧
    public static FrameRecordData GetNoForecastFrameRecordData(int frameIndex)
    {
        return noForecastFrameRecordDic[frameIndex];
    }

    //记录所有操作
    public static void ClientRecordFrameSyncData(FrameRecordData frameRecordData, bool isForecast = true)
    {
        int nextFrameIndex = ClientFrameSync.clientFrameIndex + 1;
        if (!ContainsFrameIndex(nextFrameIndex))
        {
            AddFrameRecordData(nextFrameIndex, frameRecordData);
        }

        //该帧不参与预测
        if (!isForecast)
        {
            if (!noForecastFrameRecordDic.ContainsKey(nextFrameIndex))
            {
                noForecastFrameRecordDic.Add(nextFrameIndex, frameRecordData);
            }
        }
    }

    //记录发过来的所有操作
    public static void ServerRecordFrameSyncData(FrameRecordData frameRecordData)
    {
        if (!ContainsFrameIndex(ClientFrameSync.serverFrameIndex))
        {
            AddFrameRecordData(ClientFrameSync.serverFrameIndex, frameRecordData);
        }

        if (!noForecastFrameRecordDic.ContainsKey(ClientFrameSync.serverFrameIndex))
        {
            noForecastFrameRecordDic.Add(ClientFrameSync.serverFrameIndex, frameRecordData);
        }
    }

    public static void AddFrameRecordData(int frameIndex, FrameRecordData frameRecordData)
    {
        FrameRecordDataGroup frameRecordDataGroup = null;
        if (!ContainsFrameIndex(frameIndex))
        {
            frameRecordDataGroup = new FrameRecordDataGroup();
            frameRecordDataGroup.frameIndex = frameIndex;
            frameRecordDataGroup.frameRecordData = new List<FrameRecordData>() { };
            frameRecord.Add(frameRecordDataGroup);
        }
        else
        {
            frameRecordDataGroup = GetFrameRecordDataGroup(frameIndex);
        }

        if (frameRecordData != null)
        {
            frameRecordDataGroup.frameRecordData.Add(frameRecordData);
        }
    }

    public static FrameRecordDataGroup GetFrameRecordDataGroup(int frameIndex)
    {
        for (int i = 0; i < frameRecord.Count; i++)
        {
            if (frameRecord[i].frameIndex == frameIndex)
            {
                return frameRecord[i];
            }
        }

        return null;
    }

    public static bool ContainsFrameIndex(int frameIndex)
    {
        for (int i = 0; i < frameRecord.Count; i++)
        {
            if (frameRecord[i].frameIndex == frameIndex)
            {
                return true;
            }
        }

        return false;
    }
}