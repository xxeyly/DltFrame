using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DltFramework;
using Sirenix.OdinInspector;

public class ClientFrameSync
{
    private static List<IFrameSync> frameSyncList = new List<IFrameSync>();

    private static void OnFrameSync()
    {
        foreach (IFrameSync frameSync in frameSyncList)
        {
            frameSync.FrameSync();
        }
    }

    [LabelText("开启帧同步")]
    public static async void StartFrameSync(int frameInterval)
    {
        frameSyncList = DataFrameComponent.Hierarchy_GetAllObjectsInScene<IFrameSync>();
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(frameInterval));
            OnFrameSync();
        }
    }
}