using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    public class AudioSvcData : ScriptableObject
    {
        [LabelText("音频内容")] public List<AudioInfo> audioInfos;

        [Serializable]
        public struct AudioInfo
        {
            [LabelText("名称")] public string audioName;
            [LabelText("片段")] public AudioClip audioClip;
        }
    }
}