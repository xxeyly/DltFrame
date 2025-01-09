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

        private static void HierarchyShow(int instanceid, Rect selectionRect)
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
                            viewNameRect = new Rect(selectionRect.position + new Vector2(18 + GUI.skin.label.CalcSize(new(obj.name)).x, 0), selectionRect.size);
                        }
                        else
                        {
                            viewNameRect = GlobalHierarchy.SetRect(selectionRect, -40 - ((descriptionName.Length - 1) * 12f), descriptionName.Length * 15);
                        }


                        if (selectionRect.Contains(Event.current.mousePosition))
                        {
                            GUI.Label(viewNameRect, descriptionName, GlobalHierarchy.LabelGUIStyle(GlobalHierarchy.BindUiTypeHierarchyHoverColor));
                        }
                        else
                        {
                            GUI.Label(viewNameRect, descriptionName, GlobalHierarchy.LabelGUIStyle(GlobalHierarchy.BindUiTypeHierarchyOutColor));
                        }
                    }

                    #endregion
                }
            }
        }
    }
}