using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace XFramework
{
    [Serializable]
    public class CircuitTempData
    {
        [LabelText("活动视图")] public List<Type> activityViewType;
        [LabelText("一般计时任务")] public List<int> timeTask;
        [LabelText("循环计时任务")] public List<int> switchTask;
        [LabelText("实体")] public List<string> entity;
    }
}