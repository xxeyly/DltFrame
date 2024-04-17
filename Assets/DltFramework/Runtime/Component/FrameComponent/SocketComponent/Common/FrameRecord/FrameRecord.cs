using System;
using System.Collections.Generic;

/// <summary>
/// 帧记录
/// </summary>
public class FrameRecord
{
    //所有客户端帧记录
    public static List<FrameDataGroup> frameRecord = new List<FrameDataGroup>();

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
        int nextFrameIndex = ServerFrameSync.serverFrameIndex + 1;
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

    /// <summary>
    /// 记录发过来的所有操作
    /// </summary>
    /// <param name="frameRecordData"></param>
    public static void ServerRecordFrameSyncData(FrameRecordData frameRecordData)
    {
        if (!ContainsFrameIndex(ServerFrameSync.serverFrameIndex))
        {
            AddFrameRecordData(ServerFrameSync.serverFrameIndex, frameRecordData);
        }

        if (!noForecastFrameRecordDic.ContainsKey(ServerFrameSync.serverFrameIndex))
        {
            noForecastFrameRecordDic.Add(ServerFrameSync.serverFrameIndex, frameRecordData);
        }
    }

    /// <summary>
    /// 记录帧数据
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <param name="frameRecordData"></param>
    public static void AddFrameRecordData(int frameIndex, FrameRecordData frameRecordData)
    {
        FrameDataGroup frameDataGroup = null;
        if (!ContainsFrameIndex(frameIndex))
        {
            frameDataGroup = new FrameDataGroup();
            frameDataGroup.FrameIndex = frameIndex;
            // frameDataGroup.frameRecordData = new List<FrameRecordData>() { };
            frameRecord.Add(frameDataGroup);
        }
        else
        {
            frameDataGroup = GetFrameRecordDataGroup(frameIndex);
        }

        if (frameRecordData != null)
        {
            // frameDataGroup.frameRecordData.Add(frameRecordData);
        }   
    }

    /// <summary>
    /// 获得当前帧数据组
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <returns></returns>
    public static FrameDataGroup GetFrameRecordDataGroup(int frameIndex)
    {
        for (int i = 0; i < frameRecord.Count; i++)
        {
            if (frameRecord[i].FrameIndex == frameIndex)
            {
                return frameRecord[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 包含当前帧
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <returns></returns>
    public static bool ContainsFrameIndex(int frameIndex)
    {
        for (int i = 0; i < frameRecord.Count; i++)
        {
            if (frameRecord[i].FrameIndex == frameIndex)
            {
                return true;
            }
        }

        return false;
    }
}