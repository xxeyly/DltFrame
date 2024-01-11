using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace DltFramework
{
    public partial class SceneComponentInit
    {
        [LabelText("增加计时任务")]
        protected string AddTask(string taskName, float delay, int taskCount, UnityAction initAction = null, UnityAction endAction = null, params UnityAction[] action)
        {
            return UniTaskFrameComponent.Instance.AddTask(taskName, delay, taskCount, initAction, endAction, action);
        }

        [LabelText("移除计时任务")]
        protected void DeleteTask(string taskName)
        {
            UniTaskFrameComponent.Instance.RemoveTask(taskName);
        }
    }
}