using System;
using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.ViewSvc
{
    [Serializable]
    public class ViewSvcEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;
        [ToggleLeft][BoxGroup][LabelText("初始化")] public bool isInit;

    }
}