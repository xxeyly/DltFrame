using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    [InitializeOnLoad]
    public class CustomTextWarning
    {
        static CustomTextWarning()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyShow;
        }

        private static void HierarchyShow(int instanceid, Rect selectionrect)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                return;
            }

            GameObject obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                Text text = obj.GetComponent<Text>();
                if (text != null)
                {
                    if (text.font == null)
                    {
                        GUI.Label(GlobalHierarchy.SetRect(selectionrect, -14, 18), "!", GlobalHierarchy.LabelGUIStyle());
                    }
                    else
                    {
                        if (text.font.name == "Arial")
                        {
                            GUI.Label(GlobalHierarchy.SetRect(selectionrect, -14, 18), "!", GlobalHierarchy.LabelGUIStyle());
                        }
                    }
                }

                TextMeshProUGUI textMeshProUgui = obj.GetComponent<TextMeshProUGUI>();
                if (textMeshProUgui != null)
                {
                    if (textMeshProUgui.font == null)
                    {
                        GUI.Label(GlobalHierarchy.SetRect(selectionrect, -14, 18), "!", GlobalHierarchy.LabelGUIStyle());
                    }
                    else
                    {
                        if (textMeshProUgui.font.name == "Arial")
                        {
                            GUI.Label(GlobalHierarchy.SetRect(selectionrect, -14, 18), "!", GlobalHierarchy.LabelGUIStyle());
                        }
                    }
                }
            }
        }
    }
}