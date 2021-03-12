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
        [LabelText("当前场景索引")] public int sceneIndex;
        [LabelText("当前场景名称")] public string sceneName;
        [BoxGroup("场景加载")] [LabelText("跳转场景")] public bool jump;

        [BoxGroup("场景加载")] [LabelText("跳转场景名称")]
        public string jumpSceneName;

        [LabelText("音乐开关")] public bool audioState;
        [LabelText("下载文件数据")] [SerializeField] public Dictionary<string, byte[]> downFileData;
        [LabelText("当前质量")] public QualitySettingType qualitySettingType = QualitySettingType.High;
        [LabelText("鼠标状态")] public bool mouseState;
        [LabelText("加载场景方式")] public SceneLoadType sceneLoadType;
        [LabelText("场景资源加载完毕")] public bool sceneResLoad;

        public override void InitSvc()
        {
        }


        public override void StartSvc()
        {
            Instance = GetComponent<PersistentDataSvc>();
        }
    }
}