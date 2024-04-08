using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomEntityItemHierarchy
    {
        static CustomEntityItemHierarchy()
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
                EntityItem entityItem = obj.GetComponent<EntityItem>();
                if (entityItem != null)
                {
                    #region 静态

                    GUI.Label(GlobalHierarchy.SetRect(selectionrect, -7, 18), "E", GlobalHierarchy.LabelGUIStyle());

                    #endregion

                    #region 警告

                    if (entityItem.entityName != obj.name)
                    {
                        if (GUI.Button(GlobalHierarchy.SetRect(selectionrect, -22, 18), "R", GlobalHierarchy.LabelGUIStyle()))
                        {
                            entityItem.entityName = obj.name;
                        }
                    }

                    #endregion

                    #region 描述

                    if (!string.IsNullOrEmpty(entityItem.descriptionName))
                    {
                        string descriptionName = " --- " + entityItem.descriptionName;
                        Rect viewNameRect;
                        if (GlobalHierarchy.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionrect.position + new Vector2(18 * 1 + DataFrameComponent.Hierarchy_CalculationHierarchyContentLength(obj.name), 0), selectionrect.size);
                        }
                        else
                        {
                            viewNameRect = GlobalHierarchy.SetRect(selectionrect, -40 - ((descriptionName.Length - 1) * 12f), descriptionName.Length * 15);
                        }

                        GUI.Label(viewNameRect, descriptionName, GlobalHierarchy.LabelGUIStyle(Color.blue));
                    }

                    #endregion

                    #region 热更

                    entityItem.HotFixAssetPathConfigIsExist = entityItem.GetComponent<HotFixAssetPathConfig>() != null;

                    #endregion
                }
            }
        }
    }
}