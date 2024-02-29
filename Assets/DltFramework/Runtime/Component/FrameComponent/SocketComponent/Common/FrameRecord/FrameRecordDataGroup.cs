using System;
using System.Collections.Generic;

[Serializable]
public class FrameRecordDataGroup
{
    public int frameIndex;
    public List<FrameRecordData> frameRecordData = new List<FrameRecordData>();
}