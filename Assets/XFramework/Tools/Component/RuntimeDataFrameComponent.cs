using System;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace XFramework
{
    /// <summary>
    /// 动态加载数据
    /// </summary>
    public partial class RuntimeDataFrameComponent : FrameComponent
    {
        public static RuntimeDataFrameComponent Instance;
        [BoxGroup("场景加载")] [LabelText("跳转场景")] public bool jump;

        [BoxGroup("场景加载")] [LabelText("跳转场景名称")]
        public string jumpSceneName;

        [LabelText("音乐开关")] public bool audioState;
        [LabelText("当前质量")] public QualitySettingType qualitySettingType = QualitySettingType.High;
        [LabelText("鼠标状态")] public bool mouseState;
        [LabelText("当前鼠标是否在UI上")] public bool uiOcclusion;

        public override void FrameEndComponent()
        {
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<RuntimeDataFrameComponent>();
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                uiOcclusion = true;
            }
            else
            {
                uiOcclusion = false;
            }
        }
    }
}