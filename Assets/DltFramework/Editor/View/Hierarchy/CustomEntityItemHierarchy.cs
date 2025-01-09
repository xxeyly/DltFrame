using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomEntityItemHierarchy
    {
#if UNITY_EDITOR


        static CustomEntityItemHierarchy()
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
                EntityItem entityItem = obj.GetComponent<EntityItem>();
                if (entityItem != null)
                {
                    #region 描述

                    if (!string.IsNullOrEmpty(entityItem.descriptionName))
                    {
                        string descriptionName = " --- " + entityItem.descriptionName;
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
                            GUI.Label(viewNameRect, descriptionName, GlobalHierarchy.LabelGUIStyle(GlobalHierarchy.EntityItemHierarchyHoverColor));
                        }
                        else
                        {
                            GUI.Label(viewNameRect, descriptionName, GlobalHierarchy.LabelGUIStyle(GlobalHierarchy.EntityItemHierarchyOutColor));
                        }
                    }

                    #endregion

                    int offsetIndex = 1;

                    #region 静态

                    GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "E", () => { });

                    #endregion

                    offsetIndex -= 1;


                    #region 热更

                    entityItem.HotFixAssetPathConfigIsExist = entityItem.GetComponent<HotFixAssetPathConfig>() != null;
                    if (entityItem.HotFixAssetPathConfigIsExist)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "H", () => { });
                        offsetIndex -= 1;
                    }

                    #endregion

                    #region 警告

                    if (entityItem.entityName != obj.name)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "R", () => { entityItem.entityName = obj.name; });
                    }

                    #endregion
                }
            }
        }
    }
#endif
}