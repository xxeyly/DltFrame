using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.Editor.ConfigBaseWindowEditor
{
    [InitializeOnLoad]
    public class CustomFrameHierarchyLogo
    {
        private static Texture XFrameworkLOGO;
        private static GUIStyle HierarchyIconStyle;
        private static Texture XFrameworkLOGOTitle;
        private static GUIStyle ProjectIconStyle;

        static CustomFrameHierarchyLogo()
        {
            HierarchyIconStyle = new GUIStyle();
            HierarchyIconStyle.alignment = TextAnchor.MiddleRight;
            HierarchyIconStyle.normal.textColor = Color.cyan;
            XFrameworkLOGO = AssetDatabase.LoadAssetAtPath<Texture>("Assets/XxSlitFrame/Editor/Texture/Root.png");
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyShow;

            ProjectIconStyle = new GUIStyle();
            ProjectIconStyle.alignment = TextAnchor.MiddleRight;
            ProjectIconStyle.normal.textColor = Color.cyan;
            XFrameworkLOGOTitle = AssetDatabase.LoadAssetAtPath<Texture>("Assets/XxSlitFrame/Editor/Texture/XFramework.png");

            
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemOnGUI;
        }

        private static void HierarchyShow(int instanceid, Rect selectionrect)
        {
            GameObject gameRootStart = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (gameRootStart)
            {
                if (gameRootStart.GetComponent<GameRootStart>())
                {
                    GUI.Box(selectionrect, XFrameworkLOGO, HierarchyIconStyle);
                }
            }
        }

        /// <summary>
        /// Project窗口元素GUI
        /// </summary>
        private static void OnProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            string mainFolder = AssetDatabase.GUIDToAssetPath(guid);
            if (string.Equals(mainFolder, "Assets/XxSlitFrame"))
            {
                GUI.Box(selectionRect, XFrameworkLOGOTitle, ProjectIconStyle);
            }
        }
    }
}