using UnityEditor;
using UnityEngine;

namespace XFramework
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
                            obj.name = entityItem.entityName;
                        }
                    }

                    #endregion
                }
            }
        }
    }
}