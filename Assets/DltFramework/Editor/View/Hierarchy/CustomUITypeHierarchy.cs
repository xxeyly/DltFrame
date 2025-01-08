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

                    if (objName.Length == 0 || objName.Contains(" ") ||
                        objName[0] == '0' || objName[0] == '1' || objName[0] == '2' || objName[0] == '3' || objName[0] == '4' ||
                        objName[0] == '5' || objName[0] == '6' || objName[0] == '7' || objName[0] == '8' || objName[0] == '9' ||
                        objName[0] == '!' || objName[0] == '@' || objName[0] == '#' || objName[0] == '$' || objName[0] == '%' ||
                        objName[0] == '^' || objName[0] == '&' || objName[0] == '*' || objName[0] == '(' || objName[0] == ')' ||
                        objName[0] == '-' || objName[0] == '_' || objName[0] == '+' || objName[0] == '=' || objName[0] == '[' ||
                        objName[0] == ']' || objName[0] == '{' || objName[0] == '}' || objName[0] == '|' || objName[0] == '\\' ||
                        objName[0] == ':' || objName[0] == ';' || objName[0] == '\'' || objName[0] == '"' || objName[0] == ',' ||
                        objName[0] == '.' || objName[0] == '/' || objName[0] == '?' || objName[0] == '<' || objName[0] == '>')
                    {
                        GlobalHierarchy.DrawHierarchyButtons(obj, selectionrect, 0, "!", () => { });
                    }
                }
            }
        }
    }
}