using SRF;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(TipsData))]
    public class TipsDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TipsData tipsData = (TipsData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加"))
            {
                tipsData.tipsDataInfos.Add(new TipsData.TipsDataInfo() {tipIndex = tipsData.tipsDataInfos.Count});
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < tipsData.tipsDataInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("提示索引", GUILayout.MaxWidth(50));
                tipsData.tipsDataInfos[i].tipIndex = EditorGUILayout.IntField(tipsData.tipsDataInfos[i].tipIndex, GUILayout.MaxWidth(30));
                EditorGUILayout.LabelField("确认操作", GUILayout.MaxWidth(50));
                tipsData.tipsDataInfos[i].sureOperation = EditorGUILayout.Toggle(tipsData.tipsDataInfos[i].sureOperation, GUILayout.MaxWidth(10));
                EditorGUILayout.LabelField("对话音频", GUILayout.MaxWidth(50));
#pragma warning disable 618
                tipsData.tipsDataInfos[i].tipsAudioClip = (AudioClip) EditorGUILayout.ObjectField(tipsData.tipsDataInfos[i].tipsAudioClip, typeof(AudioClip));
#pragma warning restore 618
                EditorGUILayout.LabelField("提示内容", GUILayout.MaxWidth(50));
                tipsData.tipsDataInfos[i].tipsContent = EditorGUILayout.TextField(tipsData.tipsDataInfos[i].tipsContent);

                if (GUILayout.Button("删除"))
                {
                    tipsData.tipsDataInfos.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorUtility.SetDirty(target);
        }
    }
}