﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class CreateTemplate
    {
        [MenuItem("Assets/Create/XFramework/C# BaseWindow", false, 70)]
        public static void OnCreateBaseWindowTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateScriptAsset>(), path + "/NewBaseWindow.cs", null,
                General.BaseWindowTemplatePath);
        }

        [MenuItem("Assets/Create/XFramework/C# ChildBaseWindow", false, 71)]
        public static void OnCreateChildBaseWindowTemplate()
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
        [MenuItem("Assets/Create/XFramework/C# CircuitBaseData", false, 72)]
        public static void OnCreateCircuitBaseDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateScriptAsset>(), path + "/NewCircuitBaseData.cs", null,
                General.CircuitBaseDataTemplatePath);
        }
        [MenuItem("Assets/Create/XFramework/C# ListenerSvcData", false, 73)]
        public static void OnCreateListenerSvcDataTemplate()
        {
            string path = GetSelectedPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<DoCreateScriptAsset>(), path + "/ListenerSvcData.cs", null,
                General.ListenerSvcDataTemplatePath);
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