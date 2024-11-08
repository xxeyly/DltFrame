#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    public class TemplateMenuItem
    {
        [MenuItem("Assets/Create/DltFramework/C# BaseWindow", false, 70)]
        public static void OnCreateBaseWindowTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/NewBaseWindow.cs", null, RuntimeGlobal.BaseWindowTemplatePath);
        }

        [MenuItem("Assets/Create/DltFramework/C# ChildBaseWindow", false, 71)]
        public static void OnCreateChildBaseWindowTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/NewChildBaseWindow.cs", null, RuntimeGlobal.ChildBaseWindowTemplatePath);
        }

        [MenuItem("Assets/Create/DltFramework/C# ListenerFrameComponentData", false, 73)]
        public static void OnCreateListenerComponentDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/ListenerComponentData.cs", null, RuntimeGlobal.ListenerComponentDataTemplatePath);
        }

        [MenuItem("Assets/Create/DltFramework/C# SceneComponent", false, 74)]
        public static void OnCreateSceneComponentTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/NewSceneComponent.cs", null, RuntimeGlobal.SceneComponentTemplatePath);
        }

        [MenuItem("Assets/Create/DltFramework/C# SceneComponentInit", false, 75)]
        public static void OnCreateSceneComponentInitTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/NewSceneComponentInit.cs", null, RuntimeGlobal.SceneComponentInitTemplatePath);
        }
        
        [MenuItem("Assets/Create/DltFramework/C# EntityItem", false, 75)]
        public static void OnCreateEntityItemTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/NewEntityItem.cs", null, RuntimeGlobal.EntityItemTemplatePath);
        }


        // [MenuItem("Assets/Create/DltFramework/C# AnimatorControllerParameterData", false, 76)]
        public static void OnCreateAnimatorControllbaerParameterDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<TemplateContentReplace>(), path + "/AnimatorControllerParameterData.cs", null, RuntimeGlobal.AnimatorControllerParameterDataTemplatePath);
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
            {
                return "";
            }

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
#endif