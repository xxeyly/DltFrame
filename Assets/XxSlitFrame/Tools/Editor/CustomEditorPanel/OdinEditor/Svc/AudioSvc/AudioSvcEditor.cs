using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc.AudioSvc
{
    [Serializable]
    public class AudioSvcEditor : BaseEditor
    {
        private CustomScriptableObject.CustomScriptableObject _customScriptableObject;

        public AudioSvcEditor(CustomScriptableObject.CustomScriptableObject customScriptableObject)
        {
            _customScriptableObject = customScriptableObject;
            OnCreateConfig();
            OnLoadConfig();
        }

        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [ToggleLeft] [BoxGroup] [LabelText("初始化")]
        public bool isInit;

        [LabelText("音频配置")] public List<AudioSvcData.AudioInfo> audioInfos;

        private AudioSvcData _customAudioData;


        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _customAudioData = AssetDatabase.LoadAssetAtPath<AudioSvcData>(_customScriptableObject.customAudioDataPath);
            if (_customAudioData == null)
            {
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AudioSvcData>(),
                    _customScriptableObject.customAudioDataPath);
                //读取数据
                _customAudioData =
                    AssetDatabase.LoadAssetAtPath<AudioSvcData>(_customScriptableObject.customAudioDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _customAudioData.audioInfos = audioInfos;
            //标记脏区
            EditorUtility.SetDirty(_customAudioData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public override void OnLoadConfig()
        {
            audioInfos = _customAudioData.audioInfos;
        }
    }
}