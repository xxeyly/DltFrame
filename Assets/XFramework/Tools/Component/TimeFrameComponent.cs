using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace XFramework
{
    /// <summary>
    /// 计时系统
    /// </summary>
    public class TimeFrameComponent : FrameComponent
    {
        public static TimeFrameComponent Instance;

        [BoxGroup("计时任务")] [TableList(AlwaysExpanded = true)] [Searchable] [LabelText("所有计时任务")]
        public List<TimeTaskList> timeTaskList;

        [BoxGroup("计时任务")] [SerializeField] [LabelText("单任务计时器")]
        private List<TimeTask> taskTimeList;

        [BoxGroup("计时任务")] [SerializeField] [LabelText("多任务计时器")]
        private List<SwitchTask> taskSwitchList;

        private List<int> _tidTimeList;
        private List<int> _tidSwitchList;
        private bool _clear;

        [BoxGroup("计时器")] [LabelText("当前运行时间")]
        public float currentRunTime = 0;

        [BoxGroup("计时器")] [LabelText("时间暂停")] public bool pause;
        [BoxGroup("计时器")] [LabelText("系统速度")] public float systemSpeed = 1f;

        [BoxGroup("计时器")] [Searchable] [LabelText("当前时间")]
        public List<string> currentTime;


        private List<int> _timeList;

        [BoxGroup("日期失效")] [SerializeField] [LabelText("开启日期失效")]
        private bool isExpire = false;

        [BoxGroup("日期失效")] [SerializeField] [LabelText("更新当前时间")]
        private bool isUpdateCurrentTime = false;


        [BoxGroup("日期失效")] [Searchable] [LabelText("到期时间")]
        public List<int> expireDate;


        public override void FrameInitComponent()
        {
            Instance = GetComponent<TimeFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
            currentRunTime = 0;
            taskTimeList = new List<TimeTask>();
            taskSwitchList = new List<SwitchTask>();
            _tidTimeList = new List<int>();
            _tidSwitchList = new List<int>();
            currentTime = new List<string>() { "", "", "", "", "", "" };
            if (isExpire)
            {
                if (expireDate.Count != 6)
                {
                    return;
                }

                TimeSpan expire = new DateTime(expireDate[0], expireDate[1], expireDate[2], expireDate[3], expireDate[4], expireDate[5]).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
                TimeSpan currentTimeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
                if (Convert.ToInt64(expire.TotalSeconds) > Convert.ToInt64(currentTimeSpan.TotalSeconds))
                {
                    Debug.Log("未过期");
                }
                else
                {
                    Debug.Log("过期了");
                }
            }
        }

        private void FixedUpdate()
        {
            if (!pause)
            {
                currentRunTime += Time.fixedDeltaTime;
            }
        }

        private void Update()
        {
            UpdateTimer();
            if (isUpdateCurrentTime)
            {
                UpdateCurrentSystemTime();
            }

            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = systemSpeed;
            }
        }


        public override void FrameEndComponent()
        {
        }

        /// <summary>
        /// 返回当前系统时间
        /// </summary>
        /// <returns></returns>
        public List<string> UpdateCurrentSystemTime()
        {
            currentTime[0] = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month < 10)
            {
                currentTime[1] = "0" + DateTime.Now.Month;
            }
            else
            {
                currentTime[1] = DateTime.Now.Month.ToString();
            }

            if (DateTime.Now.Day < 10)
            {
                currentTime[2] = "0" + DateTime.Now.Day;
            }
            else
            {
                currentTime[2] = DateTime.Now.Day.ToString();
            }

            if (DateTime.Now.Hour < 10)
            {
                currentTime[3] = "0" + DateTime.Now.Hour;
            }
            else
            {
                currentTime[3] = DateTime.Now.Hour.ToString();
            }

            if (DateTime.Now.Minute < 10)
            {
                currentTime[4] = "0" + DateTime.Now.Minute;
            }
            else
            {
                currentTime[4] = DateTime.Now.Minute.ToString();
            }

            if (DateTime.Now.Second < 10)
            {
                currentTime[5] = "0" + DateTime.Now.Second;
            }
            else
            {
                currentTime[5] = DateTime.Now.Second.ToString();
            }

            return currentTime;
        }

        /// <summary>
        /// 获得所有计时任务的ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllTimeTaskId()
        {
            List<int> timeTaskIdList = new List<int>();
            foreach (TimeTaskList taskList in timeTaskList)
            {
                timeTaskIdList.Add(taskList.tid);
            }

            return timeTaskIdList;
        }

        #region 定时任务

        /// <summary>
        /// 增加定时任务
        /// </summary>
        public int AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            float destTime = currentRunTime + delay;
            int tid = GetTimeTaskTid();
            TimeTask timeTask = new TimeTask
            {
                tid = tid, destTime = destTime, taskName = taskName, Callback = callback, count = count, delay = delay
            };
            taskTimeList.Add(timeTask);
            timeTaskList.Add(new TimeTaskList
            {
                tid = timeTask.tid,
                tidName = timeTask.taskName,
                loopType = TimeTaskList.TimeLoopType.Once,
                waitingTime = delay,
                endTime = destTime
            });

            return tid;
        }

        /// <summary>
        /// 增加持续调用时间定时任务
        /// </summary>
        /// <param name="callback">任务</param>
        /// <param name="taskName">任务名称</param>
        /// <param name="delay">间隔时间</param>
        /// <param name="time"></param>
        public int AddContinuedTimeTask(UnityAction callback, string taskName, float delay, float time)
        {
            float count = 0;
            if (delay >= time)
            {
                count = 1;
            }
            else
            {
                count = time / delay;
            }

            return AddTimeTask(callback, taskName, delay, (int)count);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid"></param>
        public bool DeleteTimeTask(int tid)
        {
            bool exist = false;
            for (int i = 0; i < taskTimeList.Count; i++)
            {
                if (taskTimeList[i].tid == tid)
                {
                    if (_tidTimeList.Contains(tid))
                    {
                        _tidTimeList.Remove(tid);
                    }

                    if (taskTimeList.Contains(taskTimeList[i]))
                    {
                        taskTimeList.Remove(taskTimeList[i]);
                    }

                    exist = true;
                    break;
                }
            }

            foreach (TimeTaskList taskList in timeTaskList)
            {
                if (taskList.tid == tid)
                {
                    timeTaskList.Remove(taskList);
                    break;
                }
            }


            return exist;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid"></param>
        public void DeleteTimeTask(List<int> tid)
        {
            for (int i = 0; i < tid.Count; i++)
            {
                DeleteTimeTask(tid[i]);
            }
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public bool DeleteTimeTask()
        {
            _clear = true;
            for (int i = 0; i < timeTaskList.Count; i++)
            {
                if (timeTaskList[i].loopType == TimeTaskList.TimeLoopType.Once)
                {
                    timeTaskList.Remove(timeTaskList[i]);
                }
            }

            taskTimeList.Clear();
            _clear = false;
            return false;
        }

        /// <summary>
        /// 删除所有计时任务,不死任务除外
        /// </summary>
        private void DeleteAllTimeTask()
        {
            DeleteTimeTask();
            DeleteSwitchTask();
        }

        /// <summary>
        /// 生成计时唯一ID
        /// </summary>
        private int GetTimeTaskTid()
        {
            int tid = Random.Range(0, Int32.MaxValue);
            if (!_tidTimeList.Contains(tid))
            {
                _tidTimeList.Add(tid);
                return tid;
            }
            else
            {
                return GetTimeTaskTid();
            }
        }

        #endregion

        #region 切换类型定时任务

        /// <summary>
        /// 增加定时任务
        /// </summary>
        public int AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            float destTime = currentRunTime + delay;
            int tid = GetSwitchTaskTid();
            SwitchTask switchTask = new SwitchTask
            {
                tid = tid,
                destTime = destTime,
                taskName = taskName,
                CallbackList = callbackList,
                count = count,
                delay = delay
            };
            taskSwitchList.Add(switchTask);
            timeTaskList.Add(new TimeTaskList
            {
                tid = switchTask.tid,
                tidName = switchTask.taskName,
                loopType = TimeTaskList.TimeLoopType.Loop
            });
            return tid;
        }


        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid"></param>
        public bool DeleteSwitchTask(int tid)
        {
            bool exist = false;
            for (int i = 0; i < taskSwitchList.Count; i++)
            {
                if (taskSwitchList[i].tid == tid)
                {
                    if (_tidSwitchList.Contains(tid))
                    {
                        _tidSwitchList.Remove(tid);
                    }

                    if (taskSwitchList.Contains(taskSwitchList[i]))
                    {
                        taskSwitchList.Remove(taskSwitchList[i]);
                    }

                    exist = true;
                    break;
                }
            }

            foreach (TimeTaskList taskList in timeTaskList)
            {
                if (taskList.tid == tid)
                {
                    timeTaskList.Remove(taskList);
                    break;
                }
            }

            return exist;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        private bool DeleteSwitchTask()
        {
            bool exist = false;

            for (int i = 0; i < timeTaskList.Count; i++)
            {
                if (timeTaskList[i].loopType == TimeTaskList.TimeLoopType.Loop)
                {
                    timeTaskList.Remove(timeTaskList[i]);
                }
            }

            taskSwitchList.Clear();
            return exist;
        }

        /// <summary>
        /// 生成计时唯一ID
        /// </summary>
        private int GetSwitchTaskTid()
        {
            int tid = Random.Range(0, Int32.MaxValue);
            if (!_tidSwitchList.Contains(tid))
            {
                _tidSwitchList.Add(tid);
                return tid;
            }
            else
            {
                return GetSwitchTaskTid();
            }
        }

        #endregion

        private void UpdateTimer()
        {
            if (!_clear)
            {
                if (taskTimeList != null)
                {
                    for (int i = 0; i < taskTimeList.Count; i++)
                    {
                        TimeTask timeTask = taskTimeList[i];
                        if (timeTask.destTime > currentRunTime)
                        {
                            foreach (TimeTaskList taskList in timeTaskList)
                            {
                                if (taskList.tid == timeTask.tid)
                                {
                                    taskList.waitingTime = timeTask.destTime - currentRunTime;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (timeTask.count == 1)
                            {
                                int timeTaskTid = timeTask.tid;
                                taskTimeList[i].Callback.Invoke();
                                if (_tidTimeList.Contains(timeTaskTid))
                                {
                                    _tidTimeList.Remove(timeTaskTid);
                                }

                                foreach (TimeTaskList taskList in timeTaskList)
                                {
                                    if (taskList.tid == timeTaskTid)
                                    {
                                        timeTaskList.Remove(taskList);
                                        break;
                                    }
                                }

                                if (taskTimeList.Contains(timeTask))
                                {
                                    taskTimeList.Remove(timeTask);
                                }
                            }
                            else
                            {
                                //不是无限循环
                                if (timeTask.count != 0)
                                {
                                    timeTask.count -= 1;
                                    timeTask.destTime += timeTask.delay;
                                    taskTimeList[i].Callback.Invoke();
                                }
                                else
                                {
                                    timeTask.destTime += timeTask.delay;
                                    taskTimeList[i].Callback.Invoke();
                                }
                            }
                        }
                    }
                }

                if (taskSwitchList != null)
                {
                    for (int i = 0; i < taskSwitchList.Count; i++)
                    {
                        SwitchTask switchTask = taskSwitchList[i];
                        if (switchTask.destTime > currentRunTime)
                        {
                        }
                        else
                        {
                            switchTask.CallbackList[switchTask.currentTaskNumber].Invoke();
                            if (switchTask.currentTaskNumber == switchTask.CallbackList.Count - 1)
                            {
                                switchTask.currentTaskNumber = 0;
                                if (switchTask.count == 0)
                                {
                                }
                                else if (switchTask.count == 1)
                                {
                                    int timeTaskTid = switchTask.tid;

                                    if (_tidSwitchList.Contains(timeTaskTid))
                                    {
                                        _tidSwitchList.Remove(timeTaskTid);
                                    }

                                    foreach (TimeTaskList taskList in timeTaskList)
                                    {
                                        if (taskList.tid == timeTaskTid)
                                        {
                                            timeTaskList.Remove(taskList);
                                            break;
                                        }
                                    }

                                    if (taskSwitchList.Contains(switchTask))
                                    {
                                        taskSwitchList.Remove(switchTask);
                                    }
                                }
                                else
                                {
                                    switchTask.count -= 1;
                                }
                            }
                            else
                            {
                                switchTask.currentTaskNumber++;
                            }

                            switchTask.destTime += switchTask.delay;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 秒数转换时间
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public List<int> TimeConversion(int duration)
        {
            _timeList = new List<int>();
            if (duration <= 0)
            {
                return new List<int>() { 0, 0, 0, 0 };
            }

            if (duration <= 9)
            {
                _timeList.Add(0);
                _timeList.Add(0);
                _timeList.Add(0);
                _timeList.Add(duration);
            }
            else if (duration >= 10 && duration <= 59)
            {
                int tenSecond = duration / 10;
                int second = duration - tenSecond * 10;
                _timeList.Add(0);
                _timeList.Add(0);
                _timeList.Add(tenSecond);
                _timeList.Add(second);
            }
            else if (duration >= 60 && duration <= 599)
            {
                int minute = duration / 60;
                int tenSecond = (duration - minute * 60) / 10;
                int second = duration - (tenSecond * 10 + minute * 60);
                _timeList.Add(0);
                _timeList.Add(minute);
                _timeList.Add(tenSecond);
                _timeList.Add(second);
            }
            else if (duration >= 600 && duration <= 3599)
            {
                int tenMinute = duration / 600;
                int minute = (duration - tenMinute * 600) / 60;
                int tenSecond = (duration - tenMinute * 600 - minute * 60) / 10;
                int second = duration - tenMinute * 600 - tenSecond * 10 - minute * 60;
                _timeList.Add(tenMinute);
                _timeList.Add(minute);
                _timeList.Add(tenSecond);
                _timeList.Add(second);
            }

            return _timeList;
        }


        /// <summary>
        /// 时间转换秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int TimeConversion(List<int> time)
        {
            return time[0] * 600 + time[1] * 60 + time[2] * 10 + time[3];
        }

        /// <summary>
        /// 图片闪烁
        /// </summary>
        /// <param name="twinkleImage"></param>
        /// <param name="twinkleInterval"></param>
        /// <returns></returns>
        public int ImageTwinkle(Image twinkleImage, float twinkleInterval)
        {
            int twinkleTimeTask = 0;
            float apache = 1f;
            bool _enhance = true;
            twinkleTimeTask = Instance.AddTimeTask(() =>
            {
                if (_enhance)
                {
                    apache -= twinkleInterval;
                    if (apache <= 0.2)
                    {
                        _enhance = false;
                    }
                }
                else
                {
                    apache += twinkleInterval;
                    if (apache >= 1)
                    {
                        _enhance = true;
                    }
                }

                twinkleImage.color =
                    new Color(twinkleImage.color.r, twinkleImage.color.g, twinkleImage.color.b, apache);
            }, "提示", twinkleInterval, 0);
            return twinkleTimeTask;
        }
    }

    /// <summary>
    /// 定时任务数据类
    /// </summary>
    [Serializable]
    public class TimeTask
    {
        [LabelText("任务ID")] public int tid; //任务ID
        [LabelText("任务名称")] public string taskName; //任务名称
        [LabelText("执行时间")] public float destTime; //执行时间
        [LabelText("执行的逻辑")] public UnityAction Callback; // 执行的逻辑
        [LabelText("执行次数")] public int count; //执行次数
        [LabelText("执行间隔")] public float delay; //执行间隔
    }

    [Serializable]
    public class SwitchTask
    {
        public int tid; //任务ID
        public string taskName; //任务名称
        public float destTime; //执行时间
        public List<UnityAction> CallbackList; // 执行的逻辑
        public int currentTaskNumber; //当前执行的任务索引
        public int count; //执行次数
        public float delay; //执行间隔
    }
}