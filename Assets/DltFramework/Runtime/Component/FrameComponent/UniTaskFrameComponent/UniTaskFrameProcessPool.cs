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

    [LabelText("添加到任务池中")]
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

    [LabelText("移除任务池")]
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