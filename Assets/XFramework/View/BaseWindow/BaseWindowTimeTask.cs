using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework
{
    partial class BaseWindow
    {
        protected virtual void Update()
        {
            if (timeTaskInfoList.Count >= 1)
            {
                foreach (TimeTaskInfo timeTaskInfo in timeTaskInfoList)
                {
                    if (!TimeFrameComponent.Instance.GetAllTimeTaskId().Contains(timeTaskInfo.timeTaskId))
                    {
                        timeTaskInfoList.Remove(timeTaskInfo);
                        return;
                    }
                }
            }
        }

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
            int timeTaskId = TimeFrameComponent.Instance.AddTimeTask(callback, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo() { timeTaskId = timeTaskId, timeLoopType = TimeTaskList.TimeLoopType.Once, timeTaskName = taskName });
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
            int timeTaskId = TimeFrameComponent.Instance.AddSwitchTask(callbackList, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo()
                { timeTaskId = timeTaskId, timeLoopType = TimeTaskList.TimeLoopType.Loop, timeTaskName = taskName });
            return timeTaskId;
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteTimeTask(int timeTaskId)
        {
            TimeFrameComponent.Instance.DeleteTimeTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteSwitchTask(int timeTaskId)
        {
            TimeFrameComponent.Instance.DeleteSwitchTask(timeTaskId);
        }

        /// <summary>
        /// 图片闪烁
        /// </summary>
        /// <param name="twinkleImage"></param>
        /// <param name="twinkleInterval"></param>
        /// <returns></returns>
        protected int ImageTwinkle(Image twinkleImage, float twinkleInterval)
        {
            int twinkleTimeTask = TimeFrameComponent.Instance.ImageTwinkle(twinkleImage, twinkleInterval);
            timeTaskInfoList.Add(new TimeTaskInfo()
                { timeTaskId = twinkleTimeTask, timeLoopType = TimeTaskList.TimeLoopType.Once, timeTaskName = "图片闪烁" });
            return twinkleTimeTask;
        }

        private void RemoveTimeTask()
        {
            for (int i = 0; i < timeTaskInfoList.Count; i++)
            {
                switch (timeTaskInfoList[i].timeLoopType)
                {
                    case TimeTaskList.TimeLoopType.Once:
                        DeleteTimeTask(timeTaskInfoList[i].timeTaskId);
                        break;
                    case TimeTaskList.TimeLoopType.Loop:
                        DeleteSwitchTask(timeTaskInfoList[i].timeTaskId);
                        break;
                }
            }

            timeTaskInfoList.Clear();
        }
    }
}