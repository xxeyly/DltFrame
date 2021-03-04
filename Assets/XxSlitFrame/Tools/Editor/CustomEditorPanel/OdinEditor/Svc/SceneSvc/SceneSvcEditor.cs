using System;
using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.SceneSvc
{
    [Serializable]
    public class SceneSvcEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;
        [ToggleLeft][BoxGroup][LabelText("初始化")] public bool isInit;
    }
}