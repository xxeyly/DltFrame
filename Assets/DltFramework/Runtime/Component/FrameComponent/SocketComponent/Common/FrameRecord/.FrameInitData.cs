using System.Collections.Generic;

public class FrameInitData
{
    /// <summary>
    /// 当前帧索引
    /// </summary>
    public int frameIndex;

    /// <summary>
    /// 开始时间
    /// </summary>
    public long startTime;

    /// <summary>
    /// 当前时间
    /// </summary>
    public long currentTime;

    /// <summary>
    /// 帧数据组
    /// </summary>
    public List<FrameDataGroup> frameRecord = new List<FrameDataGroup>();
}