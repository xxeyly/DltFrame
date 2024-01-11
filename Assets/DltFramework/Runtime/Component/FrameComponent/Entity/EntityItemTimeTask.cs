using System.Collections.Generic;
using UnityEngine.Events;

namespace DltFramework
{
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
        protected string AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callback);
            return taskName;
        }

        /// <summary>
        /// 增加循环计时任务
        /// </summary>
        /// <param name="callbackList"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected string AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callbackList.ToArray());
            return taskName;
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteTimeTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteSwitchTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }
    }
}