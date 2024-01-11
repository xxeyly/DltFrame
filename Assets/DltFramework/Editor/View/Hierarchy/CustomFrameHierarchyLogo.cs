#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomFrameHierarchyLogo
    {
        private static Texture DltFrameworkLOGO;
        private static GUIStyle HierarchyIconStyle;
        private static Texture DltFrameworkLOGOTitle;
        private static GUIStyle ProjectIconStyle;

        static CustomFrameHierarchyLogo()
        {
            HierarchyIconStyle = new GUIStyle();
            HierarchyIconStyle.alignment = TextAnchor.MiddleRight;
            HierarchyIconStyle.normal.textColor = Color.cyan;
            DltFrameworkLOGO = AssetDatabase.LoadAssetAtPath<Texture>("Assets/DltFramework/Editor/View/Texture/Root.png");
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyShow;

            ProjectIconStyle = new GUIStyle();
            ProjectIconStyle.alignment = TextAnchor.MiddleRight;
            ProjectIconStyle.normal.textColor = Color.cyan;
            DltFrameworkLOGOTitle = AssetDatabase.LoadAssetAtPath<Texture>("Assets/DltFramework/Editor/View/Texture/DltFramework.png");

            
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemOnGUI;
        }

        private static void HierarchyShow(int instanceid, Rect selectionrect)
        {
            GameObject gameRootStart = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (gameRootStart)
            {
                if (gameRootStart != null && gameRootStart.GetComponent<GameRootStart>())
                {
                    GUI.Box(selectionrect, DltFrameworkLOGO, HierarchyIconStyle);
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
                GUI.Box(selectionRect, DltFrameworkLOGOTitle, ProjectIconStyle);
            }
        }
    }
}
#endif
