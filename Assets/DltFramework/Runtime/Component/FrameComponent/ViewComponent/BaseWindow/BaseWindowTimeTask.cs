using UnityEngine.Events;

namespace DltFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <returns></returns>
        protected string AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, count, null, null, callback);
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