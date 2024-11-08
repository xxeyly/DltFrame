using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DltFramework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public partial class UniTaskFrameComponent
{
    public Dictionary<string, List<string>> uniTaskProcess = new Dictionary<string, List<string>>();


    /// <summary>
    /// 添加任务到任务池
    /// </summary>
    /// <param name="processName">进程池</param>
    /// <param name="taskName">任务名称</param>
    /// <param name="delay">延迟</param>
    /// <param name="taskCount">任务数量</param>
    /// <param name="initAction">初始化动作</param>
    /// <param name="endAction">结束动作</param>
    /// <param name="action">任务动作</param>
    public async UniTask AddTask(string processName, string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
    {
        if (!uniTaskProcess.ContainsKey(processName))
        {
            uniTaskProcess.Add(processName, new List<string>());
        }

        if (uniTaskProcess[processName].Contains(taskName))
        {
            return;
        }
        else
        {
            uniTaskProcess[processName].Add(taskName);
        }

        await AddTask(taskName, delay, taskCount, initAction, endAction, action);
    }

    /// <summary>
    /// 移除任务池
    /// </summary>
    /// <param name="processName">进程池名称</param>
    public void RemoveTaskProcess(string processName)
    {
        if (uniTaskProcess.ContainsKey(processName))
        {
            foreach (string taskName in uniTaskProcess[processName])
            {
                RemoveTask(taskName);
            }
        }
    }

    /// <summary>
    /// 移除任务池中的任务
    /// </summary>
    /// <param name="taskName">任务名称</param>
    public void RemoveTaskProcessTaskName(string taskName)
    {
        foreach (KeyValuePair<string, List<string>> pair in uniTaskProcess)
        {
            if (pair.Value.Contains(taskName))
            {
                pair.Value.Remove(taskName);
                return;
            }
        }
    }
}