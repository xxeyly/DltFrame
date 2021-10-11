using System;
using UnityEditor;
using UnityEngine;

namespace XFramework
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
            GameObject obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                if (obj.GetComponent<BaseWindow>() != null && obj.GetComponent<ChildBaseWindow>() == null)
                {
                    BaseWindow tempBaseWindow = obj.GetComponent<BaseWindow>();

                    #region 静态

                    if (tempBaseWindow.GetViewShowType() == ViewShowType.Static)
                    {
                        Rect sRectIcon = GetRect(selectionrect, 20, 18, 1);
                        GUI.Label(sRectIcon, "S");
                    }

                    #endregion

                    #region 描述

                    if (tempBaseWindow.viewName != String.Empty)
                    {
                        Rect sRectIcon = GetRect(selectionrect, 50, 180, 2);
                        GUI.Label(sRectIcon, obj.GetComponent<BaseWindow>().viewName);
                    }

                    #endregion

                    Rect rectCheck = new Rect(selectionrect);
                    rectCheck.x += rectCheck.width - 20;
                    rectCheck.width = 18;
                    GameObject window = obj.transform.Find("Window").gameObject;
                    window.SetActive(GUI.Toggle(rectCheck, window.activeSelf, string.Empty));
                    if (window.GetComponent<CanvasGroup>())
                    {
                        window.GetComponent<CanvasGroup>().alpha = window.activeSelf ? 1 : 0;
                    }
                    else
                    {
                        Debug.Log(window.transform.parent.name);
                    }
#if UNITY_2019
                    SceneVisibilityManager.instance.DisablePicking(obj, false);
                    SceneVisibilityManager.instance.DisablePicking(window, false);
#endif
                }
            }
        }

        private static Rect GetRect(Rect selectionRect, int length, int width, int index)
        {
            Rect rect = new Rect(selectionRect);
            rect.x += rect.width - length - (length * index);
            rect.width = width;
            return rect;
        }
    }
}