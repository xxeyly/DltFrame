#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomBaseWindowHierarchy
    {
        static CustomBaseWindowHierarchy()
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
                if (obj.GetComponent<BaseWindow>() != null && obj.GetComponent<ChildBaseWindow>() == null)
                {
                    BaseWindow tempBaseWindow = obj.GetComponent<BaseWindow>();
                    int offsetIndex = 1;

                    #region 描述

                    if (!string.IsNullOrEmpty(tempBaseWindow.viewName))
                    {
                        string viewName = " --- " + tempBaseWindow.viewName;
                        Rect viewNameRect;
                        if (GlobalHierarchy.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionrect.position + new Vector2(18 + DataFrameComponent.Hierarchy_CalculationHierarchyContentLength(obj.name), 0), selectionrect.size);
                        }
                        else
                        {
                            viewNameRect = GlobalHierarchy.SetRect(selectionrect, -40 - ((viewName.Length - 1) * 12f), viewName.Length * 15);
                        }

                        GUI.Label(viewNameRect, viewName, GlobalHierarchy.LabelGUIStyle());
                    }

                    #endregion
                    #region 开关

                    GameObject window = obj.transform.Find("Window").gameObject;
                    if (window.activeSelf)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, offsetIndex, "BaseWindowOnIcon", () =>
                        {
                            window.SetActive(!window.activeSelf);
                            if (window.GetComponent<CanvasGroup>())
                            {
                                window.GetComponent<CanvasGroup>().alpha = 0;
                            }
                        });
                    }
                    else
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, offsetIndex, "BaseWindowOffIcon", () =>
                        {
                            window.SetActive(!window.activeSelf);
                            if (window.GetComponent<CanvasGroup>())
                            {
                                window.GetComponent<CanvasGroup>().alpha = 1;
                            }
                        });
                    }

                    offsetIndex -= 1;

#if UNITY_2019_1_OR_NEWER
                    SceneVisibilityManager.instance.DisablePicking(obj, false);
                    SceneVisibilityManager.instance.DisablePicking(window, false);
#endif

                    #endregion
                    #region 静态

                    if (tempBaseWindow.GetViewShowType() == ViewShowType.Static)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, offsetIndex, "BaseWindowLockIcon", () => { tempBaseWindow.viewShowType = ViewShowType.Activity; });
                    }
                    else
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, offsetIndex, "BaseWindowUnlockIcon", () => { tempBaseWindow.viewShowType = ViewShowType.Static; });
                    }

                    offsetIndex -= 1;

                    #endregion

                    #region 热更

                    tempBaseWindow.HotFixAssetPathConfigIsExist = tempBaseWindow.GetComponent<HotFixAssetPathConfig>() != null;

                    if (tempBaseWindow.HotFixAssetPathConfigIsExist)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, offsetIndex, "H", () => { });
                        offsetIndex -= 1;
                    }

                    #endregion

                    #region 警告

                    if (tempBaseWindow.GetType().Name != obj.name)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, offsetIndex, "R", () => { obj.name = tempBaseWindow.GetType().Name; });
                    }

                    #endregion
                }
            }
        }
    }
}
#endif