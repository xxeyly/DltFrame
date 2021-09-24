using System.Linq;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class EntityEditor
    {
        [MenuItem("GameObject/生成 /@(Alt+E) 生成实体 &e", false, 0)]
        public static void Generate()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<EntityItem>())
            {
                uiObj.AddComponent<EntityItem>().GetCurrentGameObjectName();
                Undo.RegisterCompleteObjectUndo(uiObj, uiObj.name);
            }
        }
    }
}