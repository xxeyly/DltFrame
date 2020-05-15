using System;
using SRF;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(PropItemData))]
    public class PropItemDataEditor : UnityEditor.Editor
    {
        private Vector2 scrollViewPos;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PropItemData propItemData = (PropItemData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加", GUILayout.MaxHeight(30)))
            {
                propItemData.groupInfos.Add(new PropItemData.PropItemGroupInfo());
            }

            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < propItemData.groupInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("物品组:" + i, GUILayout.MaxWidth(50));
                if (GUILayout.Button("增加道具", GUILayout.MaxWidth(130)))
                {
                    propItemData.groupInfos[i].propItemGroupInfo.Add(new PropItemData.PropItemInfo());
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < propItemData.groupInfos[i].propItemGroupInfo.Count; j++)
                {
                    EditorGUILayout.EnumPopup(propItemData.groupInfos[i].propItemGroupInfo[j].propTypes, GUILayout.MinWidth(150));
                }
                EditorGUILayout.EndHorizontal();

            }
            EditorUtility.SetDirty(target);

        }
    }
}