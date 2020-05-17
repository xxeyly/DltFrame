using System;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.View.CustomInspector.Editor
{
    [InitializeOnLoad]
    public class CustomBaseWindowHierarchy
    {
        static CustomBaseWindowHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCb;
        }

        private static void HierarchyItemCb(int instanceid, Rect selectionrect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                if (obj.GetComponent<BaseWindow>() != null &&
                    obj.GetComponent<BaseWindow>().GetViewShowType() == ViewShowType.Activity)
                {
                    // CheckBox 
                    Rect rectCheck = new Rect(selectionrect);
                    rectCheck.x += rectCheck.width - 20;
                    rectCheck.width = 18;
                    GameObject window = obj.transform.Find("Window").gameObject;
                    window.SetActive(GUI.Toggle(rectCheck, window.activeSelf, string.Empty)
                    );
                }
            }
        }
    }
}