using System.IO;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools;
using XxSlitFrame.View.Editor;

namespace XxSlitFrame.Model.ConfigData.Editor
{
    /// <summary>
    /// 创建ChildBaseWindow模板
    /// </summary>
    public class CreateChildBaseWindowTemplate
    {
        [MenuItem("Assets/Create/C# ChildBaseWindow", false, 71)]
        public static void OnCreateBaseWindowTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateScriptAsset>(), path + "/NewChildBaseWindow.cs", null,
                General.ChildBaseWindowTemplatePath);
        }

        /// <summary>
        /// 获得选择文件地址
        /// </summary>
        /// <returns></returns>
        private static string GetSelectedPath()
        {
            //默认路径为Assets
            string selectedPath = "Assets";

            //获取选中的资源
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            if (selection.Length != 1)
                return "";
            //遍历选中的资源以返回路径
            foreach (Object obj in selection)
            {
                selectedPath = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
                {
                    selectedPath = Path.GetDirectoryName(selectedPath);
                    break;
                }
            }

            return selectedPath;
        }
    }
}