#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [InitializeOnLoad]
    public class CustomChildBaseWindowHierarchy
    {
        static CustomChildBaseWindowHierarchy()
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
                if (obj.GetComponent<ChildBaseWindow>() != null)
                {
                    ChildBaseWindow childBaseWindow = obj.GetComponent<ChildBaseWindow>();

                    #region 警告

                    if (childBaseWindow.GetType().Name != obj.name)
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, 1, "R", () => { obj.name = childBaseWindow.GetType().Name; });
                    }

                    #endregion
                }
            }
        }
    }
}
#endif