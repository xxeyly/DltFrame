using Sirenix.OdinInspector;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomScriptableObject
{
    public class CustomScriptableObject
    {
        [FilePath] [LabelText("自动打包配置存放路径")] [LabelWidth(120)]
        public  string customBuildDataPath = "Assets/XxSlitFrame/Config/CustomBuildData.asset";

        [FilePath] [LabelText("音频配置存放路径")] [LabelWidth(120)]
        public  string customAudioDataPath = "Assets/XxSlitFrame/Config/CustomAudioData.asset";

        [FilePath] [LabelText("框架配置存放路径")] [LabelWidth(120)]
        public  string customFrameDataPath = "Assets/XxSlitFrame/Config/CustomFrameData.asset";
        
        [FilePath] [LabelText("生成配置存放路径")] [LabelWidth(120)]
        public  string generateBaseWindowPath = "Assets/XxSlitFrame/Config/GenerateBaseWindowData.asset";
    }
}