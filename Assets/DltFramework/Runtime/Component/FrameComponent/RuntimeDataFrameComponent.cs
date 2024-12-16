using Sirenix.OdinInspector;

namespace DltFramework
{
    /// <summary>
    /// 动态加载数据
    /// </summary>
    public partial class RuntimeDataFrameComponent : FrameComponent
    {
        public static RuntimeDataFrameComponent Instance;

        [LabelText("音乐开关")] public bool audioState;
        [LabelText("鼠标状态")] public bool mouseState;


        public override void SetFrameInitIndex()
        {
            frameInitIndex = 0;
        }

        public override void FrameInitComponent()
        {
            Instance = this;
        }

        public override void FrameEndComponent()
        {
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameSceneEndComponent()
        {
        }
    }
}