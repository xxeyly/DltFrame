using System;
using System.Collections.Generic;
using System.Text;
using HotFix;
using UnityEngine;

public class RequestClientMap
{
    [AddRequestCode(RequestCode.Map_EnterMap, RequestType.Client)]
    public void Map_EnterMap(byte[] data)
    {
        Debug.Log(data.Length);
        FrameInitData frameInitData = ProtobufTool.DeserializeFromByteArray<FrameInitData>(data);
        Debug.Log(frameInitData.MapName);
        Debug.Log(frameInitData.FrameIndex);
        Debug.Log(frameInitData.StartTime);
        Debug.Log(frameInitData.CurrentTime);
        Debug.Log(frameInitData.FrameRecord.Count);

        foreach (FrameDataGroup frameDataGroup in frameInitData.FrameRecord)
        {
            Debug.Log(frameDataGroup.FrameIndex);
        }

        //创建房间
        ClientMap clientMap = ClientMapManager.CreateServerMap(ClientSocketFrameComponent.Instance.roomId);
        clientMap.StartFrameSync(frameInitData);
        //进入回放模式
        //如果还处于回放模式,要一致记录接收的数据
        RecordReplays.isReplay = true;
        foreach (FrameDataGroup frameDataGroup in frameInitData.FrameRecord)
        {
            RecordReplays.AddFrameData(new List<FrameData>(frameDataGroup.FrameData));
        }

        if (RecordReplays.isReplay)
        {
        }
        else
        {
            //如果有保存的当前帧记录表示本地预测帧和服务器帧保持一致
            /*if (FrameRecord.ContainsFrameIndex(frameIndex))
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
            }*/
        }
    }
}