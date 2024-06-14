using System.Collections.Generic;

public interface ISnapshot
{
    void OnSnapshot();

    //读取快照
    void ReadSnapshot(List<SnapshotData> snapshotDataList);
}