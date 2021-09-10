using System;
using Sirenix.OdinInspector;

namespace XFramework
{
    [Serializable]
    public class BaseSvcEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [ToggleLeft] [BoxGroup] [LabelText("框架初始化")]
        public bool isFrameInit;

        [ToggleLeft] [BoxGroup] [LabelText("场景初始化")]
        public bool isSceneInit;
    }
}