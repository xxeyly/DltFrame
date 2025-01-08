using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DltFramework
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
                    if (text.font == null || text.font.name == "Arial")
                    {
                        if (obj.GetComponent<BindUiType>())
                        {
                            GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, 0, "!", () => { });
                        }
                        else
                        {
                            GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, 1, "!", () => { });
                        }
                    }
                }
                
            }
        }
    }
}