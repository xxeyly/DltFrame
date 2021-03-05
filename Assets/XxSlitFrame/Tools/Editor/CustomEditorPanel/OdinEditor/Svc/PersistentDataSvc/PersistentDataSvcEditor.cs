using System;
using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.PersistentDataSvc
{
    [Serializable]
    public class PersistentDataSvcEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [ToggleLeft] [BoxGroup] [LabelText("初始化")]
        public bool isInit;
    }
}