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
        protected string AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callback);
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
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callbackList.ToArray());
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
        protected void DeleteSwitchTask(string timeTaskId)
        {
            UniTaskFrameComponent.Instance.RemoveTask(timeTaskId);
        }
    }
}