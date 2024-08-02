using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace DltFramework
{
    /// <summary>
    /// 实体计时器
    /// </summary>
    public partial class EntityItem
    {
        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected async UniTask AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            await UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callback);
        }

        /// <summary>
        /// 增加循环计时任务
        /// </summary>
        /// <param name="callbackList"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected async UniTask AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            await UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callbackList.ToArray());
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        protected void DeleteTimeTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        protected void DeleteSwitchTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }
    }
}