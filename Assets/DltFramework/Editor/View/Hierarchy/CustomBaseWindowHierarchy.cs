#if UNITY_EDITOR

using System;
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

                    #region 静态

                    if (tempBaseWindow.GetViewShowType() == ViewShowType.Static)
                    {
                        GUI.Label(GlobalHierarchy.SetRect(selectionrect, -7, 18), "S", GlobalHierarchy.LabelGUIStyle());
                    }

                    #endregion

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

                    Rect rectCheck = new Rect(selectionrect);
                    rectCheck.x += rectCheck.width - 20;
                    rectCheck.width = 18;
                    GameObject window = obj.transform.Find("Window").gameObject;

                    window.SetActive(GUI.Toggle(GlobalHierarchy.SetRect(selectionrect, -23, 18), window.activeSelf, string.Empty));
                    if (window.GetComponent<CanvasGroup>())
                    {
                        window.GetComponent<CanvasGroup>().alpha = window.activeSelf ? 1 : 0;
                    }
                    else
                    {
                        Debug.Log(window.transform.parent.name);
                    }
#if UNITY_2019_1_OR_NEWER
                    SceneVisibilityManager.instance.DisablePicking(obj, false);
                    SceneVisibilityManager.instance.DisablePicking(window, false);
#endif

                    #endregion

                    #region 热更

                    tempBaseWindow.HotFixAssetPathConfigIsExist = tempBaseWindow.GetComponent<HotFixAssetPathConfig>() != null;
                    
                    if (tempBaseWindow.HotFixAssetPathConfigIsExist)
                    {
                        GUI.Label(GlobalHierarchy.SetRect(selectionrect, -35, 18), "H", GlobalHierarchy.LabelGUIStyle());
                    }

                    #endregion
                    
                    #region 警告

                    if (tempBaseWindow.GetType().Name != obj.name)
                    {
                        if (GUI.Button(GlobalHierarchy.SetRect(selectionrect, -45, 18), "R", GlobalHierarchy.LabelGUIStyle()))
                        {
                            obj.name = tempBaseWindow.GetType().Name;
                        }
                    }

                    #endregion

                }
            }
        }
    }
}
#endif