using System.Collections.Generic;

public class FrameInitData
{
    public int frameIndex;
    public long startTime;
    public long currentTime;

    public List<FrameRecordDataGroup> frameRecord = new List<FrameRecordDataGroup>();
}