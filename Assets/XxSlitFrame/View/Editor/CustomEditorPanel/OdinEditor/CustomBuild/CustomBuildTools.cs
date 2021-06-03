using System.Collections.Generic;
using UnityEditor;

namespace XxSlitFrame.View.Editor.CustomEditorPanel.OdinEditor.CustomBuild
{
    public class CustomBuildTools
    {
        /// <summary>
        /// 获得打包的场景
        /// </summary>
        /// <returns></returns>
        public static string[] FindEnableEditorScenes()
        {
            List<string> editorScenes = new List<string>();
            foreach (EditorBuildSettingsScene editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                if (editorBuildSettingsScene.enabled)
                {
                    editorScenes.Add(editorBuildSettingsScene.path);
                }
            }

            return editorScenes.ToArray();
        }
    }
}