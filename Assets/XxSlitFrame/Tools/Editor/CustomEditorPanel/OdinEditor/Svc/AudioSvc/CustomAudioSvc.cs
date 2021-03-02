using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc
{
    [Serializable]
    public class CustomAudioSvc
    {
        private CustomScriptableObject.CustomScriptableObject _customScriptableObject;

        public CustomAudioSvc(CustomScriptableObject.CustomScriptableObject customScriptableObject)
        {
            _customScriptableObject = customScriptableObject;
            LoadConfig();
        }

        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [LabelText("音频配置")] [OnValueChanged("OnChangeSaveData")]
        public List<CustomAudioData.AudioInfo> audioInfos;

        private void OnChangeSaveData()
        {
            CustomAudioData customAudioData =
                AssetDatabase.LoadAssetAtPath<CustomAudioData>(_customScriptableObject.customAudioDataPath);
            if (customAudioData != null)
            {
                customAudioData.audioInfos = audioInfos;
            }
            else
            {
                CustomAudioData tempCustomAudioData = ScriptableObject.CreateInstance<CustomAudioData>();
                tempCustomAudioData.audioInfos = audioInfos;
                AssetDatabase.CreateAsset(tempCustomAudioData,
                    _customScriptableObject.customAudioDataPath);
                customAudioData =
                    AssetDatabase.LoadAssetAtPath<CustomAudioData>(_customScriptableObject.customAudioDataPath);
            }

            //标记脏区
            EditorUtility.SetDirty(customAudioData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        private void LoadConfig()
        {
            CustomAudioData customAudioData =
                AssetDatabase.LoadAssetAtPath<CustomAudioData>(_customScriptableObject.customAudioDataPath);
            if (customAudioData != null)
            {
                audioInfos = customAudioData.audioInfos;
            }
        }
    }
}