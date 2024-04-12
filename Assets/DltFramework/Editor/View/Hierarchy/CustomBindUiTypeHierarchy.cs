using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomBindUiTypeHierarchy
    {
        static CustomBindUiTypeHierarchy()
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
                BindUiType bindUiType = obj.GetComponent<BindUiType>();
                if (bindUiType != null)
                {
                    #region 描述

                    if (!string.IsNullOrEmpty(bindUiType.descriptionName))
                    {
                        string descriptionName = " --- " + bindUiType.descriptionName;
                        Rect viewNameRect;
                        if (GlobalHierarchy.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionrect.position + new Vector2(18 * 1 + DataFrameComponent.Hierarchy_CalculationHierarchyContentLength(obj.name), 0), selectionrect.size);
                        }
                        else
                        {
                            viewNameRect = GlobalHierarchy.SetRect(selectionrect, -40 - ((descriptionName.Length - 1) * 12f), descriptionName.Length * 15);
                        }

                        GUI.Label(viewNameRect, descriptionName, GlobalHierarchy.LabelGUIStyle(Color.yellow));
                    }

                    #endregion
                }
            }
        }
    }
}