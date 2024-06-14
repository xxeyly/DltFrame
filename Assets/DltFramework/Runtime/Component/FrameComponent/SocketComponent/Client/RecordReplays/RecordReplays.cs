using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RecordReplays
{
    //回放帧
    public static int ReplaysFrameIndex = 0;

    //回放间隔倍数
    public static int replayInterval = 2;

    public static Queue<List<FrameData>> allFrameData = new Queue<List<FrameData>>();

    //是否是回放模式
    public static bool isReplay = false;

    //添加回放数据
    public static void AddFrameData(List<FrameData> frameRecordData)
    {
        allFrameData.Enqueue(frameRecordData);
    }

    //回放
    public static async UniTask RecordReplay()
    {
        isReplay = true;
        if (allFrameData.Count > 0)
        {
            await Replay();
        }

        isReplay = false;
    }

    private static async UniTask Replay()
    {
        List<FrameData> frameDataList = allFrameData.Dequeue();
        if (frameDataList.Count > 0)
        {
            //回放帧间隔
            int replayFrameInterval = ClientMapManager.frameInterval / replayInterval;
            await UniTask.Delay(TimeSpan.FromMilliseconds(replayFrameInterval));
            ReplaysFrameIndex += 1;
            Debug.Log("回放中:" + ReplaysFrameIndex);
            //执行帧逻辑
            foreach (FrameData frameData in frameDataList)
            {
                FrameLogic.ExecuteReflection(frameData.DataType, frameData.Data.ToByteArray());
            }
        }

        //还有回放帧,递归回放
        if (allFrameData.Count > 0)
        {
            await Replay();
        }
    }
}