using System.Collections.Generic;
using DltFramework;

//快照
public class Snapshot
{
    public static List<ISnapshot> snapshotList = new List<ISnapshot>();

    //快照集合
    public static Dictionary<int, List<SnapshotData>> snapshotDic = new Dictionary<int, List<SnapshotData>>();

    public static void StartSnapshot()
    {
        snapshotList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<ISnapshot>();
    }

    //增加快照
    public static void AddSnapshot()
    {
        foreach (ISnapshot snapshot in snapshotList)
        {
            snapshot.OnSnapshot();
        }
    }
    
    public static void AddSnapshot(SnapshotData snapshotData)
    {
        if (!snapshotDic.ContainsKey(FrameRecord.frameIndex))
        {
            snapshotDic.Add(FrameRecord.frameIndex, new List<SnapshotData>());
        }
        else
        {
            snapshotDic[FrameRecord.frameIndex].Add(snapshotData);
        }
    }

    //获取快照
    public static void GetSnapshot(int frameIndex)
    {
        if (snapshotDic.ContainsKey(frameIndex))
        {
            foreach (SnapshotData snapshotData in snapshotDic[frameIndex])
            {
               
            }
        }
    }
}