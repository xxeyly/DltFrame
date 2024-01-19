#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomFrameHierarchyLogo
    {
        private static GUIStyle HierarchyIconStyle;
        private static GUIStyle ProjectIconStyle;

        static CustomFrameHierarchyLogo()
        {
            HierarchyIconStyle = new GUIStyle();
            HierarchyIconStyle.alignment = TextAnchor.MiddleRight;
            HierarchyIconStyle.normal.textColor = Color.cyan;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyShow;

            ProjectIconStyle = new GUIStyle();
            ProjectIconStyle.alignment = TextAnchor.MiddleRight;
            ProjectIconStyle.normal.textColor = Color.cyan;
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemOnGUI;
        }

        private static void HierarchyShow(int instanceid, Rect selectionrect)
        {
            GameObject gameRootStart = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (gameRootStart)
            {
                if (gameRootStart != null && gameRootStart.GetComponent<GameRootStart>())
                {
                    GUI.Box(selectionrect, "框架根目录", HierarchyIconStyle);
                }
            }
        }

        /// <summary>
        /// Project窗口元素GUI
        /// </summary>
        private static void OnProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            string mainFolder = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(mainFolder, "Assets/DltFramework"))
            {
                GUI.Box(selectionRect, "框架根目录", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/DltFramework/Aot"))
            {
                GUI.Box(selectionRect, "不可热更", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/DltFramework/Editor"))
            {
                GUI.Box(selectionRect, "框架提示UI", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/DltFramework/HotFix"))
            {
                GUI.Box(selectionRect, "可热更", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/DltFramework/Runtime"))
            {
                GUI.Box(selectionRect, "可热更", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/DltFramework/Runtime/Component/Start"))
            {
                GUI.Box(selectionRect, "启动类", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/Config"))
            {
                GUI.Box(selectionRect, "配置文件", ProjectIconStyle);
            }

            if (string.Equals(mainFolder, "Assets/Config/SceneHotfixAsset"))
            {
                GUI.Box(selectionRect, "场景打包配置文件", ProjectIconStyle);
            }
        }
    }
}
#endif