using SRF;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(DialogueData))]
    public class DialogueDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DialogueData dialogueData = (DialogueData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加对话组"))
            {
                dialogueData.dataInfos.Add(new DialogDataInfoContainer());
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < dialogueData.dataInfos.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("对话组:" + i, GUILayout.MaxWidth(50));
                if (GUILayout.Button("增加对话片段", GUILayout.MaxWidth(120)))
                {
                    dialogueData.dataInfos[i].dialogDataInfos.Add(new DialogDataInfo());
                }


                if (GUILayout.Button("删除对话片段", GUILayout.MaxWidth(120)))
                {
                    if (dialogueData.dataInfos[i].dialogDataInfos.Count > 0)
                    {
                        dialogueData.dataInfos[i].dialogDataInfos.PopLast();
                    }
                }

                if (GUILayout.Button("删除对话组", GUILayout.MaxWidth(120)))
                {
                    dialogueData.dataInfos.RemoveAt(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();

                for (int j = 0; j < dialogueData.dataInfos[i].dialogDataInfos.Count; j++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("对话角色", GUILayout.MaxWidth(50));
                    dialogueData.dataInfos[i].dialogDataInfos[j].role =
                        (Role) EditorGUILayout.EnumPopup(dialogueData.dataInfos[i].dialogDataInfos[j].role, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField("对话长度", GUILayout.MaxWidth(50));
                    dialogueData.dataInfos[i].dialogDataInfos[j].length =
                        (Length) EditorGUILayout.EnumPopup(dialogueData.dataInfos[i].dialogDataInfos[j].length, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField("对话内容:", GUILayout.MaxWidth(60));
                    dialogueData.dataInfos[i].dialogDataInfos[j].dialogContent = EditorGUILayout.TextField(dialogueData.dataInfos[i].dialogDataInfos[j].dialogContent);
                    EditorGUILayout.LabelField("对话音频", GUILayout.MaxWidth(50));
                    dialogueData.dataInfos[i].dialogDataInfos[j].dialogueAudioClip =
#pragma warning disable 618
                        (AudioClip) EditorGUILayout.ObjectField(dialogueData.dataInfos[i].dialogDataInfos[j].dialogueAudioClip, typeof(AudioClip));
#pragma warning restore 618
                    if (GUILayout.Button("增加", GUILayout.MaxWidth(80)))
                    {
                        dialogueData.dataInfos[i].dialogDataInfos.Insert(j + 1, new DialogDataInfo());
                    }

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(80)))
                    {
                        dialogueData.dataInfos[i].dialogDataInfos.RemoveAt(j);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();
                serializedObject.ApplyModifiedProperties();

            }
        }
    }
}