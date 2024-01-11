using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public class GlobalHierarchy
    {
        [LabelText("Hierarchy内容跟随")] public static bool HierarchyContentFollow = true;

        public static GUIStyle LabelGUIStyle(Color color)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = color;
            guiStyle.fontSize = 12;
            guiStyle.fontStyle = FontStyle.Italic;
            return guiStyle;
        }

        public static GUIStyle LabelGUIStyle()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = Color.cyan;
            guiStyle.fontSize = 12;
            guiStyle.fontStyle = FontStyle.Italic;
            return guiStyle;
        }

        public static Rect SetRect(Rect selectionRect, float offset, float width)
        {
            Rect rect = new Rect(selectionRect);
            rect.x += rect.width + offset;
            rect.width = width;
            return rect;
        }
    }
}