using System.Linq;
using UnityEngine;

namespace DltFramework
{
#if UNITY_EDITOR
    public class EntityEditor
    {
        [UnityEditor.MenuItem("GameObject/生成 /@(Alt+E) 生成实体 &e", false, 0)]
        public static void Generate()
        {
            GameObject uiObj = UnityEditor.Selection.objects.First() as GameObject;
            if (uiObj == null)
            {
                return;
            }

            if (!uiObj.GetComponent<EntityItem>())
            {
                UnityEditor.Undo.AddComponent<EntityItem>(uiObj).SetCurrentGameObjectName();
            }
            else
            {
                uiObj.GetComponent<EntityItem>().SetCurrentGameObjectName();
            }
        }
    }
#endif
}