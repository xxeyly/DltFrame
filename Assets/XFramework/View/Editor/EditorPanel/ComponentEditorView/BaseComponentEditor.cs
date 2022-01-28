using System;
using Sirenix.OdinInspector;

namespace XFramework
{
    [Serializable]
    public class BaseComponentEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [ToggleLeft] [BoxGroup] [LabelText("框架初始化")]
        public bool isFrameInit;

        [ToggleLeft] [BoxGroup] [LabelText("场景初始化")]
        public bool isSceneInit;

        [BoxGroup] [LabelText("组件索引")] public int componentIndex;
    }
}