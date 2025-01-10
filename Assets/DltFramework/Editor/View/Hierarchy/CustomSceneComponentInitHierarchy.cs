using System.Collections.Generic;
using UnityEditor;
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

        private static void HierarchyShow(int instanceid, Rect selectionRect)
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

                    int offsetIndex = 1;

                    if (!string.IsNullOrEmpty(sceneComponent.viewName))
                    {
                        string viewName = " --- " + sceneComponent.viewName;
                        Rect viewNameRect;
                        if (GlobalHierarchy.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionRect.position + new Vector2(18 + GUI.skin.label.CalcSize(new(obj.name)).x, 0), selectionRect.size);
                        }
                        else
                        {
                            viewNameRect = GlobalHierarchy.SetRect(selectionRect, -40 - ((viewName.Length - 1) * 12f), viewName.Length * 15);
                        }

                        if (selectionRect.Contains(Event.current.mousePosition))
                        {
                            GUI.Label(viewNameRect, viewName, GlobalHierarchy.LabelGUIStyle(GlobalHierarchy.SceneComponentInitHierarchyHoverColor));
                        }
                        else
                        {
                            GUI.Label(viewNameRect, viewName, GlobalHierarchy.LabelGUIStyle(GlobalHierarchy.SceneComponentInitHierarchyOutColor));
                        }
                    }

                    #endregion

                    #region 场景

                    GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "S", () => { });

                    offsetIndex -= 1;

                    #endregion

                    #region 热更

                    sceneComponent.HotFixAssetPathConfigIsExist = sceneComponent.GetComponent<HotFixAssetPathConfig>() != null;
                    if (sceneComponent.HotFixAssetPathConfigIsExist)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "H", () => { });
                        offsetIndex -= 1;
                        List<HotFixAssetPathConfig> childHotFixAssetPathConfigs = new List<HotFixAssetPathConfig>(obj.GetComponentsInChildren<HotFixAssetPathConfig>());
                        if (childHotFixAssetPathConfigs.Count > 1)
                        {
                            GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "H!Error", () =>
                            {
                                for (int i = 0; i < childHotFixAssetPathConfigs.Count; i++)
                                {
                                    if (childHotFixAssetPathConfigs[i] != obj.GetComponent<HotFixAssetPathConfig>())
                                    {
                                        Object.DestroyImmediate(childHotFixAssetPathConfigs[i]);
                                    }
                                }
                            });
                            offsetIndex -= 1;
                        }
                    }

                    #endregion

                    #region 警告

                    if (sceneComponent.GetType().Name != obj.name)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionRect, offsetIndex, "R!", () => { obj.name = sceneComponent.GetType().Name; });
                        offsetIndex -= 1;
                    }

                    #endregion
                }
            }
        }
    }
}
#endif