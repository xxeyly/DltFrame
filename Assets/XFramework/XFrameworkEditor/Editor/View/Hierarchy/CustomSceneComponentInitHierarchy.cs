using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace XFramework
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
                        if (General.HierarchyContentFollow)
                        {
                            viewNameRect = new Rect(selectionrect.position + new Vector2(18 + DataFrameComponent.CalculationHierarchyContentLength(obj.name), 0), selectionrect.size);
                        }
                        else
                        {
                            viewNameRect = SetRect(selectionrect, -40 - ((viewName.Length - 1) * 12f), viewName.Length * 15);
                        }

                        GUI.Label(viewNameRect, viewName, new GUIStyle()
                        {
                            fontStyle = FontStyle.Italic
                        });
                    }

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
#endif