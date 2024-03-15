public interface IFrameSyncPull
{
    int id { get; set; }
    //拉取帧数据
    void PullFrameRecordData(FrameRecordData frameRecordData);
}