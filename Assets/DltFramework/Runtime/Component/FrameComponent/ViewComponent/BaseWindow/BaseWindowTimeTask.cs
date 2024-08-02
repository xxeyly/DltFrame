using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace DltFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <returns></returns>
        protected async UniTask AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            await UniTaskFrameComponent.Instance.AddTask(taskName, delay, taskCount, initAction, endAction, action);
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