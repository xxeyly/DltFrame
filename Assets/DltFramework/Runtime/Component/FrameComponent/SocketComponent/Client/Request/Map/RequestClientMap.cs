using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RequestClientMap
{
    [AddRequestCode(RequestCode.Map_EnterMap, RequestType.Client)]
    public async void Map_EnterMap(byte[] data)
    {
        FrameInitData frameInitData = ProtobufTool.DeserializeFromByteArray<FrameInitData>(data);

        Debug.Log("第一次接收的帧数据大小:" + frameInitData.FrameRecord.Count);
        Debug.Log("当前服务器帧数:" + frameInitData.FrameIndex);
        //创建房间
        ClientMap clientMap = ClientMapManager.CreateServerMap(ClientSocketFrameComponent.Instance.roomId);
        clientMap.StartFrameSync(frameInitData);
        //回放初始化帧索引
        RecordReplays.ReplaysFrameIndex = 0;
        //进入回放模式
        //如果还处于回放模式,要一致记录接收的数据
        foreach (FrameDataGroup frameDataGroup in frameInitData.FrameRecord)
        {
            RecordReplays.AddFrameData(new List<FrameData>(frameDataGroup.FrameData));
        }

        //记录当前客户端帧
        clientMap.clientFrameIndex = frameInitData.FrameIndex;

        //进入回放模式
        Debug.Log("回放模式");

        await RecordReplays.RecordReplay();
        Debug.Log("回放结束");
    }
}