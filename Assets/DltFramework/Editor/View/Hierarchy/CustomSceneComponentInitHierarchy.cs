﻿using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomSceneComponentInitHierarchy
    {
        static CustomSceneComponentInitHierarchy()
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
                if (obj.GetComponent<SceneComponentInit>() != null)
                {
                    SceneComponentInit sceneComponent = obj.GetComponent<SceneComponentInit>();

                    #region 描述

                    if (!string.IsNullOrEmpty(sceneComponent.viewName))
                    {
                        string viewName = " --- " + sceneComponent.viewName;
                        Rect viewNameRect;
                        if (GlobalHierarchy.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionrect.position + new Vector2(18 + DataFrameComponent.Hierarchy_CalculationHierarchyContentLength(obj.name), 0), selectionrect.size);
                        }
                        else
                        {
                            viewNameRect = GlobalHierarchy.SetRect(selectionrect, -40 - ((viewName.Length - 1) * 12f), viewName.Length * 15);
                        }

                        GUI.Label(viewNameRect, viewName,GlobalHierarchy.LabelGUIStyle());
                    }

                    #endregion
                    #region 热更

                    sceneComponent.HotFixAssetPathConfigIsExist = sceneComponent.GetComponent<HotFixAssetPathConfig>() != null;

                    #endregion

                }
            }
        }
        
    }
}
#endif