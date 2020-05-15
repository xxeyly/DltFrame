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
            if (GUILayout.Button("增加", GUILayout.MaxHeight(30)))
            {
                audioData.audioDataInfos.Add(new AudioData.AudioDataInfo());
            }

            if (GUILayout.Button("删除", GUILayout.MaxHeight(30)))
            {
                audioData.audioDataInfos.PopLast();
            }

            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < audioData.audioDataInfos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("音频类型", GUILayout.MaxWidth(50));
                audioData.audioDataInfos[i].audioType = (AudioData.AudioType) EditorGUILayout.EnumPopup(audioData.audioDataInfos[i].audioType, GUILayout.MaxWidth(150));
                EditorGUILayout.LabelField("对应音频", GUILayout.MaxWidth(50));
                audioData.audioDataInfos[i].dialogueAudioClip = (AudioClip) EditorGUILayout.ObjectField(audioData.audioDataInfos[i].dialogueAudioClip, typeof(AudioClip));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}