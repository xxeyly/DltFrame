using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    public class GlobalHierarchy
    {
        [LabelText("Hierarchy内容跟随")] public static bool HierarchyContentFollow = true;
        private const int hierarchyWindowOffsetRight = -53;
        private const int layoutBaseOffset = 15;
        private const int buttonsWidth = 24;

        public static Color BaseWindowHierarchyHoverColor = Color.white;
        public static Color BindUiTypeHierarchyHoverColor = Color.white;
        public static Color EntityItemHierarchyHoverColor = Color.white;
        public static Color SceneComponentHierarchyHoverColor = Color.white;
        public static Color SceneComponentInitHierarchyHoverColor = Color.white;
        
        public static Color BaseWindowHierarchyOutColor = Color.gray;
        public static Color BindUiTypeHierarchyOutColor = Color.gray;
        public static Color EntityItemHierarchyOutColor = Color.gray;
        public static Color SceneComponentHierarchyOutColor = Color.gray;
        public static Color SceneComponentInitHierarchyOutColor = Color.gray;
        

        public static GUIStyle LabelGUIStyle(Color color)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = color;
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

        public static void DrawHierarchyButtons(GameObject obj, Rect selectionRect, int offset, string iconName, Action action)
        {
            float iconStartX = CalculateButtonsRect(obj, selectionRect);
            if (GUI.Button(new(iconStartX + buttonsWidth * offset, selectionRect.y, buttonsWidth, selectionRect.height), LoadTexture(iconName)))
            {
                action?.Invoke();
            }
        }

        private static float CalculateButtonsRect(GameObject gameObject, Rect selectionRect)
        {
            selectionRect.x = hierarchyWindowOffsetRight - layoutBaseOffset;
            selectionRect.width = EditorGUIUtility.currentViewWidth;
            return selectionRect.x + selectionRect.width;
        }

        public static Texture2D LoadTexture(string textureName)
        {
            Texture2D loadedTexture = Resources.Load<Texture2D>(textureName);
            if (loadedTexture != null)
            {
                return loadedTexture;
            }

            return EditorGUIUtility.whiteTexture;
        }
    }
}