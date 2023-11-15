using UnityEditor;
using UnityEngine;

namespace XFramework
{
    [InitializeOnLoad]
    public class CustomUITypeHierarchy
    {
        static CustomUITypeHierarchy()
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
                if (obj.GetComponent<BindUiType>() != null)
                {
                    #region 静态

                    GUI.Label(SetRect(selectionrect, -7, 18), "U");

                    #endregion
                }
            }
        }

        private static Rect SetRect(Rect selectionRect, float offset, float width)
        {
            Rect rect = new Rect(selectionRect);
            rect.x += rect.width + offset;
            rect.width = width;
            return rect;
        }
    }
}