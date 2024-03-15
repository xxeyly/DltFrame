using System;
using System.Collections.Generic;
using DltFramework;
using UnityEngine;

public class ClientEnterGame
{
    [AddRequestCode(RequestCode.EnterGame, RequestType.Client)]
    public async void OnClientEnterGame(string data)
    {
        // ViewFrameComponent.Instance.HideView(typeof(EnterGame));
        FrameInitData frameInitData = JsonUtil.FromJson<FrameInitData>(data);
        Debug.Log(data);
        ClientFrameSync.serverFrameIndex = frameInitData.frameIndex;
        for (int i = 0; i < frameInitData.frameIndex; i++)
        {
            //数据为空,自己计算
            if (FrameRecord.GetFrameRecordDataGroup(i) == null)
            {
                FrameRecordDataGroup temp = new FrameRecordDataGroup();
                temp.frameIndex = i;
                temp.frameRecordData = new List<FrameRecordData>();
                frameInitData.frameRecord.Add(temp);
            }
        }
        RecordReplays.isReplay = true;
        //开始帧同步,即使是回放模式也要一直同步数据
        //回放时间有长又有短,根据回放时间来计算
        ClientFrameSync.StartFrameSync(frameInitData);

        //回拨帧同步
        foreach (FrameRecordDataGroup frameRecordDataGroup in frameInitData.frameRecord)
        {
            RecordReplays.AddFrameData(frameRecordDataGroup.frameRecordData);
        }

        Debug.Log("开始回放");
        Debug.Log(frameInitData.frameRecord.Count);
        //等待回放结束
        await RecordReplays.RecordReplay();
        Debug.Log("回放结束");
        //创建本地角色
        FrameRecordData frameRecordData = new FrameRecordData();
        frameRecordData.id = ClientSocketFrameComponent.Instance.Token;
        frameRecordData.create = true;
        ClientSocketFrameComponent.Instance.UdpSend(frameRecordData, false);
    }
}