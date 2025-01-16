using UnityEditor;
using UnityEngine;

namespace DltFramework
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

                    GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, 1, "U", () => { });

                    #endregion

                    string objName = obj.name;

                    if (!DataFrameComponent.String_IsScriptsStandard(objName))
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, 0, "R!Error", () => { });
                    }
                }
            }
        }
    }
}