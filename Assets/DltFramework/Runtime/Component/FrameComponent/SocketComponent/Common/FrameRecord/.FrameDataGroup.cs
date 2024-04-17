using System;
using System.Collections.Generic;

/// <summary>
/// 所有玩家一帧的数据
/// </summary>
[Serializable]
public class FrameDataGroup
{
    public int frameIndex;
    public List<FrameData> frameRecordData = new List<FrameData>();
}