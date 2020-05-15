using SRF;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(AnimatorClipData))]
    public class AnimatorClipDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            AnimatorClipData animatorClipData = (AnimatorClipData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加关键帧"))
            {
                animatorClipData.animatorClipDataInfos.Add(new AnimatorClipData.AnimatorClipDataInfo() {animatorControllerParameterType = AnimatorControllerParameterType.Trigger});
            }

            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < animatorClipData.animatorClipDataInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("动画属性名称", GUILayout.MaxWidth(70));
                animatorClipData.animatorClipDataInfos[i].animatorClipName = EditorGUILayout.TextField(animatorClipData.animatorClipDataInfos[i].animatorClipName);
                EditorGUILayout.LabelField("属性类型", GUILayout.MaxWidth(50));
                animatorClipData.animatorClipDataInfos[i].animatorControllerParameterType =
                    (AnimatorControllerParameterType) EditorGUILayout.EnumPopup(animatorClipData.animatorClipDataInfos[i].animatorControllerParameterType, GUILayout.MaxWidth(100));
                EditorGUILayout.LabelField("固定过渡持续时间", GUILayout.MaxWidth(100));
                animatorClipData.animatorClipDataInfos[i].fixedDuration = EditorGUILayout.Toggle(animatorClipData.animatorClipDataInfos[i].fixedDuration, GUILayout.MaxWidth(10));
                EditorGUILayout.LabelField("持续过度时间", GUILayout.MaxWidth(70));
                animatorClipData.animatorClipDataInfos[i].transitionDuration =
                    EditorGUILayout.FloatField(animatorClipData.animatorClipDataInfos[i].transitionDuration, GUILayout.MaxWidth(30));
                EditorGUILayout.LabelField("循环", GUILayout.MaxWidth(30));
                animatorClipData.animatorClipDataInfos[i].animatorClipIsLoop =
                    EditorGUILayout.Toggle(animatorClipData.animatorClipDataInfos[i].animatorClipIsLoop, GUILayout.MaxWidth(10));
                EditorGUILayout.LabelField("开始帧", GUILayout.MaxWidth(40));
                animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame = EditorGUILayout.IntField(animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame);
                EditorGUILayout.LabelField("结束帧", GUILayout.MaxWidth(40));
                animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame = EditorGUILayout.IntField(animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame);
                if (GUILayout.Button("增加关键帧"))
                {
                    animatorClipData.animatorClipDataInfos.Insert(i + 1,
                        new AnimatorClipData.AnimatorClipDataInfo() {animatorControllerParameterType = AnimatorControllerParameterType.Trigger});
                    serializedObject.ApplyModifiedProperties();
                }

                if (GUILayout.Button("删除关键帧"))
                {
                    animatorClipData.animatorClipDataInfos.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
                EditorUtility.SetDirty(target);
            }
            

        }
    }
}