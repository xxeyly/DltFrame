using System.Collections.Generic;
using UnityEngine.Events;

namespace XFramework
{
    public partial class SceneComponent
    {
        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            int timeTaskId = TimeComponent.Instance.AddTimeTask(callback, taskName, delay, count);
            return timeTaskId;
        }

        /// <summary>
        /// 增加循环计时任务
        /// </summary>
        /// <param name="callbackList"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            int timeTaskId = TimeComponent.Instance.AddSwitchTask(callbackList, taskName, delay, count);
            return timeTaskId;
        }

        /// <summary>
        /// 增加不摧毁任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddImmortalTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            int timeTaskId = TimeComponent.Instance.AddImmortalTimeTask(callback, taskName, delay, count);
            return timeTaskId;
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteTimeTask(int timeTaskId)
        {
            TimeComponent.Instance.DeleteTimeTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteSwitchTask(int timeTaskId)
        {
            TimeComponent.Instance.DeleteSwitchTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteImmortalTimeTask(int timeTaskId)
        {
            TimeComponent.Instance.DeleteImmortalTimeTask(timeTaskId);
        }
    }
}