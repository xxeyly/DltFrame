using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

public class RequestServerMap
{
    [AddRequestCode(RequestCode.Map_EnterMap, RequestType.Server)]
    public void Map_EnterMap(byte[] data, ClientSocket clientSocket)
    {
        string content = Encoding.UTF8.GetString(data);
        //获得地图数据
        ServerMap serverMap = ServerMapManager.GetServerMap(Convert.ToInt32(content));
        if (serverMap == null)
        {
            Console.WriteLine("地图不存在:" + content);
            return;
        }

        //获得所有帧数据
        //获得所有帧数据
        FrameInitData frameInitData = new FrameInitData();
        frameInitData.MapName = "士大夫艰苦六九三零附近开了";
        frameInitData.FrameIndex = serverMap.mapFrameIndex;
        frameInitData.StartTime = serverMap.startTime;
        frameInitData.CurrentTime = ServerFrameSync.currentTime;
        //精简了帧数据,无操作的帧数据不传输,需要客户端自己计算
        frameInitData.FrameRecord.AddRange(serverMap.frameDataGroups);
        clientSocket.TcpSend(RequestCode.Map_EnterMap, ProtobufTool.SerializeToByteArray(frameInitData));
    }
}