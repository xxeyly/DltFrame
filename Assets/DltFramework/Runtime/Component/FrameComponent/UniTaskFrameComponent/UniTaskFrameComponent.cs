using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using DltFramework;

public partial class UniTaskFrameComponent : FrameComponent
{
    public static UniTaskFrameComponent Instance;
    [LabelText("事件列表")] public Dictionary<string, CancellationTokenSource> cancellationTokenSources = new Dictionary<string, CancellationTokenSource>();
    [LabelText("场景事件列表")] public Dictionary<string, CancellationTokenSource> sceneLoadCancellationTokenSources = new Dictionary<string, CancellationTokenSource>();

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

    /// <summary>
    /// 移除所有任务
    /// </summary>
    private void RemoveAllTask()
    {
        List<string> keys = new List<string>(cancellationTokenSources.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            RemoveTask(keys[i]);
        }
    }

    /// <summary>
    /// 移除任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <param name="delay">延迟</param>
    /// <param name="taskCount">任务数量</param>
    /// <param name="initAction">初始化</param>
    /// <param name="endAction">结束</param>
    /// <param name="action">任务内容</param>
    /// <returns></returns>
    public async UniTask AddSceneTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
    {
        if (IsSceneContainCurrentTask(taskName))
        {
            Debug.LogError(taskName + "已存在");
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        sceneLoadCancellationTokenSources.Add(taskName, cancellationTokenSource);
#pragma warning disable 4014
        await ExecuteSceneTask(taskName, cancellationTokenSource.Token, delay, taskCount, initAction, endAction, action);
#pragma warning restore 4014
    }

    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <param name="delay">延迟</param>
    /// <param name="taskCount">任务数量</param>
    /// <param name="initAction">初始化</param>
    /// <param name="endAction">结束</param>
    /// <param name="action">任务内容</param>
    public async UniTask AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
    {
        if (IsContainCurrentTask(taskName))
        {
            Debug.LogError(taskName + "已存在");
            return;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSources.Add(taskName, cancellationTokenSource);
#pragma warning disable 4014
        await ExecuteTask(taskName, cancellationTokenSource.Token, delay, taskCount, initAction, endAction, action);
#pragma warning restore 4014
    }

    /// <summary>
    /// 移除任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <param name="delay">延迟</param>
    /// <param name="taskCount">任务数量</param>
    /// <param name="initAction">初始化</param>
    /// <param name="endAction">结束</param>
    /// <param name="action">任务内容</param>
    /// <returns></returns>
    private async UniTask<string> ExecuteSceneTask(string taskName, CancellationToken cancellationToken, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null,
        params UnityAction[] action)
    {
        cancellationToken.ThrowIfCancellationRequested();
        initAction?.Invoke();
        //0代表无限循环
        if (taskCount == 0)
        {
            while (true)
            {
                foreach (UnityAction unityAction in action)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
                    unityAction?.Invoke();
                }
            }
        }
        else
        {
            for (int i = 0; i < taskCount; i++)
            {
                foreach (UnityAction unityAction in action)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
                    unityAction?.Invoke();
                }
            }
        }

        endAction?.Invoke();
        RemoveSceneTask(taskName);
        return taskName;
    }

    /// <summary>
    /// 移除任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    public void RemoveTask(string taskName)
    {
        if (IsContainCurrentTask(taskName))
        {
            cancellationTokenSources[taskName].Cancel();
            cancellationTokenSources[taskName].Dispose();
            cancellationTokenSources.Remove(taskName);
        }

        RemoveTaskProcessTaskName(taskName);
    }

    /// <summary>
    /// 移除任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    public void RemoveSceneTask(string taskName)
    {
        if (IsContainCurrentTask(taskName))
        {
            sceneLoadCancellationTokenSources[taskName].Cancel();
            sceneLoadCancellationTokenSources[taskName].Dispose();
            sceneLoadCancellationTokenSources.Remove(taskName);
        }
    }

    public override void FrameEndComponent()
    {
        Instance = null;
    }

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <param name="cancellationToken">取消标记</param>
    /// <param name="delay">延迟</param>
    /// <param name="taskCount">任务数量</param>
    /// <param name="initAction">初始化</param>
    /// <param name="endAction">结束</param>
    /// <param name="action">任务内容</param>
    /// <returns></returns>
    private async UniTask<string> ExecuteTask(string taskName, CancellationToken cancellationToken, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null,
        params UnityAction[] action)
    {
        cancellationToken.ThrowIfCancellationRequested();
        initAction?.Invoke();
        //0代表无限循环
        if (taskCount == 0)
        {
            while (true)
            {
                foreach (UnityAction unityAction in action)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
                    unityAction?.Invoke();
                }
            }
        }
        else
        {
            for (int i = 0; i < taskCount; i++)
            {
                foreach (UnityAction unityAction in action)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
                    unityAction?.Invoke();
                }
            }
        }

        endAction?.Invoke();
        RemoveTask(taskName);
        return taskName;
    }

    /// <summary>
    /// 判断是否包含当前任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <returns></returns>
    private bool IsContainCurrentTask(string taskName)
    {
        return cancellationTokenSources.ContainsKey(taskName);
    }

    /// <summary>
    /// 判断是否包含当前任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <returns></returns>
    private bool IsSceneContainCurrentTask(string taskName)
    {
        return cancellationTokenSources.ContainsKey(taskName);
    }
}