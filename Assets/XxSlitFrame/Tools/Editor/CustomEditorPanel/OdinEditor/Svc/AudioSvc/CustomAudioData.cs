using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.Svc
{
    public class CustomAudioData : ScriptableObject
    {
        [LabelText("音频配置")] public List<AudioInfo> audioInfos;

        [Serializable]
        public struct AudioInfo
        {
            [LabelText("音频名字")] public string audioName;
            [LabelText("音频文件")] public AudioClip audioClip;
        }
    }
}