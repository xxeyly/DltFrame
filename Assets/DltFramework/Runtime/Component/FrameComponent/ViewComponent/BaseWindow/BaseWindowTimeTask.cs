using UnityEngine.Events;

namespace DltFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <returns></returns>
        protected string AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, taskCount, initAction, endAction, action);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        protected void DeleteTimeTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }
    }
}