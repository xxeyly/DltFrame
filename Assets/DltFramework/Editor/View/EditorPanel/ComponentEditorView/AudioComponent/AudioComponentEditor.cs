#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DltFramework
{
    [Serializable]
    public class AudioComponentEditor : BaseEditor
    {
        [LabelText("音频配置")] public List<AudioComponentData.AudioInfo> audioInfos;
        private AudioComponentData _customAudioData;

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _customAudioData = AssetDatabase.LoadAssetAtPath<AudioComponentData>(RuntimeGlobal.customAudioDataPath);
            if (_customAudioData == null)
            {
                if (!Directory.Exists(RuntimeGlobal.assetRootPath))
                {
                    Directory.CreateDirectory(RuntimeGlobal.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AudioComponentData>(), RuntimeGlobal.customAudioDataPath);
                //读取数据
                _customAudioData = AssetDatabase.LoadAssetAtPath<AudioComponentData>(RuntimeGlobal.customAudioDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _customAudioData.audioInfos = audioInfos;
            //标记脏区
            EditorUtility.SetDirty(_customAudioData);
        }

        public override void OnLoadConfig()
        {
            audioInfos = _customAudioData.audioInfos;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
  
    }
}
#endif