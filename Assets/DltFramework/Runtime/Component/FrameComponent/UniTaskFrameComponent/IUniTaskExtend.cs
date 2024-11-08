using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace DltFramework
{
    public interface IUniTaskExtend
    {
        /// <summary>
        /// 添加场景任务
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="delay">延迟</param>
        /// <param name="taskCount">任务数量</param>
        /// <param name="initAction">初始化</param>
        /// <param name="endAction">结束</param>
        /// <param name="action">任务内容</param>
        /// <returns></returns>
        UniTask AddSceneTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="delay">延迟</param>
        /// <param name="taskCount">任务数量</param>
        /// <param name="initAction">初始化</param>
        /// <param name="endAction">结束</param>
        /// <param name="action">任务内容</param>
        UniTask AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action);


        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="taskName">任务名称</param>
        public void RemoveTask(string taskName);

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="taskName">任务名称</param>
        public void RemoveSceneTask(string taskName);
    }
}