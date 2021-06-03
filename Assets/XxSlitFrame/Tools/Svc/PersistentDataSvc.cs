using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 动态加载数据
    /// </summary>
    public class PersistentDataSvc : SvcBase
    {
        public static PersistentDataSvc Instance;
        [BoxGroup("场景加载")] [LabelText("跳转场景")] public bool jump;

        [BoxGroup("场景加载")] [LabelText("跳转场景名称")]
        public string jumpSceneName;

        [LabelText("音乐开关")] public bool audioState;
        [LabelText("当前质量")] public QualitySettingType qualitySettingType = QualitySettingType.High;
        [LabelText("鼠标状态")] public bool mouseState;
        [LabelText("考核模式")] public bool assessment;

        public override void InitSvc()
        {
        }


        public override void StartSvc()
        {
            Instance = GetComponent<PersistentDataSvc>();
        }
    }
}