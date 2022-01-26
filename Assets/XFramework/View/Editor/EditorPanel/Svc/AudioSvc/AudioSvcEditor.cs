using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
#if UNITY_EDITOR

    [Serializable]
    public class AudioSvcEditor : BaseEditor
    {
        private bool hideView;
        [ShowIf("hideView")] public bool Enabled;

        [ToggleLeft] [BoxGroup] [LabelText("框架初始化")]
        public bool isFrameInit;

        [ToggleLeft] [BoxGroup] [LabelText("场景初始化")]
        public bool isSceneInit;

        [LabelText("音频配置")] public List<AudioSvcData.AudioInfo> audioInfos;

        private AudioSvcData _customAudioData;
        [BoxGroup] [LabelText("服务索引")] public int svcIndex;


        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _customAudioData = AssetDatabase.LoadAssetAtPath<AudioSvcData>(General.customAudioDataPath);
            if (_customAudioData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AudioSvcData>(), General.customAudioDataPath);
                //读取数据
                _customAudioData = AssetDatabase.LoadAssetAtPath<AudioSvcData>(General.customAudioDataPath);
            }
        }

        public override void OnSaveConfig()
        {
            _customAudioData.audioInfos = audioInfos;
            //标记脏区
            EditorUtility.SetDirty(_customAudioData);
            // 保存所有修改
            // AssetDatabase.SaveAssets();
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
#endif
}