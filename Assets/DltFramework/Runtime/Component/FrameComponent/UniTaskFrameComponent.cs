using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using DltFramework;

public class UniTaskFrameComponent : FrameComponent
{
    public static UniTaskFrameComponent Instance;
    [LabelText("事件列表")] public Dictionary<string, CancellationTokenSource> cancellationTokenSources = new Dictionary<string, CancellationTokenSource>();

    public override void FrameInitComponent()
    {
        Instance = this;
    }

    public override void FrameSceneInitComponent()
    {
    }

    public override void FrameSceneEndComponent()
    {
        RemoveAllTask();
    }

    private void RemoveAllTask()
    {
        List<string> keys = new List<string>(cancellationTokenSources.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            RemoveTask(keys[i]);
        }
    }

    [LabelText("添加任务")]
    public string AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
    {
        if (IsContainCurrentTask(taskName))
        {
            Debug.LogError(taskName + "已存在");
            return String.Empty;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSources.Add(taskName, cancellationTokenSource);
#pragma warning disable 4014
        ExecuteTask(taskName, cancellationTokenSource.Token, delay, taskCount, initAction, endAction, action);
#pragma warning restore 4014
        return taskName;
    }

    [LabelText("移除任务")]
    public void RemoveTask(string taskName)
    {
        if (IsContainCurrentTask(taskName))
        {
            cancellationTokenSources[taskName].Cancel();
            cancellationTokenSources[taskName].Dispose();
            cancellationTokenSources.Remove(taskName);
        }
    }


    public override void FrameEndComponent()
    {
        Instance = null;
    }

    [LabelText("执行任务")]
    private async UniTask<string> ExecuteTask(string taskName, CancellationToken cancellationToken, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
    {
        cancellationToken.ThrowIfCancellationRequested();
        initAction?.Invoke();
        for (int i = 0; i < taskCount; i++)
        {
            foreach (UnityAction unityAction in action)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
                unityAction?.Invoke();
            }
        }

        endAction?.Invoke();
        RemoveTask(taskName);
        return taskName;
    }


    [LabelText("包含当前任务")]
    private bool IsContainCurrentTask(string taskName)
    {
        return cancellationTokenSources.ContainsKey(taskName);
    }
}