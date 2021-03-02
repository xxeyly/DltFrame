using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomScriptableObject
{
    public class CustomScriptableObject
    {
        [HorizontalGroup("打包数据")] [FilePath] [LabelText("自动打包存放路径")] [LabelWidth(100)]
        public string customBuildDataPath = "Assets/XxSlitFrame/Config/CustomBuildData.asset";
    }
}