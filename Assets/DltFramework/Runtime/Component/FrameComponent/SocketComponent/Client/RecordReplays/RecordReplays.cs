using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class RecordReplays
{
    //回放间隔倍数
    public static int replayInterval = 2;
    public static Queue<List<FrameRecordData>> allFrameData = new Queue<List<FrameRecordData>>();

    public static bool isReplay = false;

    //添加回放数据
    public static void AddFrameData(List<FrameRecordData> frameRecordData)
    {
        allFrameData.Enqueue(frameRecordData);
    }

    //回放
    public static async UniTask RecordReplay()
    {
        isReplay = true;
        for (int i = 0; i < allFrameData.Count; i++)
        {
            List<FrameRecordData> frameRecordDatas = allFrameData.Dequeue();
            if (frameRecordDatas.Count > 0)
            {
                await UniTask.Delay(TimeSpan.FromMilliseconds(ClientFrameSync.frameInterval / replayInterval));
                foreach (FrameRecordData frameRecordData in frameRecordDatas)
                {
                    ClientFrameSync.ExecuteFrameLogic(frameRecordData);
                }
            }
        }

        isReplay = false;
    }
}