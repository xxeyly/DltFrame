using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Svc
{
    [Serializable]
    public class CircuitData
    {
        [LabelText("活动视图")] public List<Type> activityViewType;
        [LabelText("一般计时任务")] public List<int> timeTask;
        [LabelText("循环计时任务")] public List<int> switchTask;
        [LabelText("实体")] public List<string> entity;
    }
}