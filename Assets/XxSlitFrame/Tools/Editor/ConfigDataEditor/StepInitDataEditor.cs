using UnityEditor;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(StepInitData))]
    public class StepInitDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StepInitData stepInitData = (StepInitData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加"))
            {
                stepInitData.stepInitDataInfoGroups.Add(new StepInitDataInfo());
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < stepInitData.stepInitDataInfoGroups.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("大步骤", GUILayout.MaxWidth(50));
                stepInitData.stepInitDataInfoGroups[i].bigIndex = EditorGUILayout.IntField(stepInitData.stepInitDataInfoGroups[i].bigIndex);
                EditorGUILayout.LabelField("小步骤", GUILayout.MaxWidth(50));
                stepInitData.stepInitDataInfoGroups[i].smallIndex = EditorGUILayout.IntField(stepInitData.stepInitDataInfoGroups[i].smallIndex);
                EditorGUILayout.LabelField("语音索引", GUILayout.MaxWidth(50));
                stepInitData.stepInitDataInfoGroups[i].tipIndex = EditorGUILayout.IntField(stepInitData.stepInitDataInfoGroups[i].tipIndex);
                EditorGUILayout.LabelField("相机位置", GUILayout.MaxWidth(50));
                stepInitData.stepInitDataInfoGroups[i].cameraPosType =
                    (CameraPosData.CameraPosType) EditorGUILayout.EnumPopup(stepInitData.stepInitDataInfoGroups[i].cameraPosType);
                EditorGUILayout.LabelField("动画类型", GUILayout.MaxWidth(50));
                stepInitData.stepInitDataInfoGroups[i].animType = (AnimType) EditorGUILayout.EnumPopup(stepInitData.stepInitDataInfoGroups[i].animType);
                EditorGUILayout.LabelField("动画进度", GUILayout.MaxWidth(50));
                stepInitData.stepInitDataInfoGroups[i].animSpeedProgress = (AnimSpeedProgress) EditorGUILayout.EnumPopup(stepInitData.stepInitDataInfoGroups[i].animSpeedProgress);
                if (GUILayout.Button("删除步骤", GUILayout.MaxWidth(80)))
                {
                    stepInitData.stepInitDataInfoGroups.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}