using System;
using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.Listener
{
    [Serializable]
    public class ListenerSvcEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [ToggleLeft] [BoxGroup] [LabelText("初始化")]
        public bool isInit;
    }
}