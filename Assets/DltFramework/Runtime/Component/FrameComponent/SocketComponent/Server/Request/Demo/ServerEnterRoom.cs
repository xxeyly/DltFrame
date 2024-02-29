using System;
using System.Collections.Generic;
using LitJson;

public class ServerEnterRoom
{
    [AddRequestCode(RequestCode.EnterGame, RequestType.Server)]
    public void OnEnterRoom(string data, ClientSocket clientSocket)
    {
        //获得所有帧数据
        //第一次使用Tcp传输,后面使用Udp传输
        //获得所有帧数据
        FrameInitData frameInitData = new FrameInitData();
        frameInitData.frameIndex = FrameRecord.frameIndex;
        frameInitData.startTime = ServerFrameSync.startTime;
        frameInitData.currentTime = ServerFrameSync.currentTime;
        //精简了帧数据,无操作的帧数据不传输,需要客户端自己计算
        Console.WriteLine("帧数据数量:" + FrameRecord.frameRecord.Count);
        List<FrameRecordDataGroup> effectiveFrameRecord = new List<FrameRecordDataGroup>();
        for (int i = 0; i < FrameRecord.frameRecord.Count; i++)
        {
            bool isEffective = false;
            foreach (FrameRecordData frameRecordData in FrameRecord.frameRecord[i].frameRecordData)
            {
                if (!FrameRecordData.IsEmptyFrame(frameRecordData))
                {
                    isEffective = true;
                }
            }

            if (isEffective)
            {
                effectiveFrameRecord.Add(FrameRecord.frameRecord[i]);
            }
        }

        // frameInitData.frameRecord = FrameRecord.frameRecord;
        clientSocket.TcpSend(RequestCode.EnterGame, JsonMapper.ToJson(frameInitData));
    }
}