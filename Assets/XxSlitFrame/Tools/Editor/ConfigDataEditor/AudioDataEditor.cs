using System.Collections.Generic;
using SRF;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.ConfigDataEditor
{
    [CustomEditor(typeof(AudioData))]
    public class AudioDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AudioData audioData = (AudioData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("增加"))
            {
                audioData.audioDataInfos.Add(new AudioData.AudioDataInfo());
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < audioData.audioDataInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("音频类型", GUILayout.MaxWidth(50));
                audioData.audioDataInfos[i].audioType = (AudioData.AudioType) EditorGUILayout.EnumPopup(audioData.audioDataInfos[i].audioType, GUILayout.MaxWidth(150));
                EditorGUILayout.LabelField("对应音频", GUILayout.MaxWidth(50));
#pragma warning disable 618
                audioData.audioDataInfos[i].dialogueAudioClip = (AudioClip) EditorGUILayout.ObjectField(audioData.audioDataInfos[i].dialogueAudioClip, typeof(AudioClip));
#pragma warning restore 618
                if (GUILayout.Button("删除音频", GUILayout.MaxWidth(80)))
                {
                    audioData.audioDataInfos.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
                serializedObject.ApplyModifiedProperties();

            }
        }
    }
}