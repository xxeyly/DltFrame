using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    /// <summary>
    /// 音频组件数据
    /// </summary>
    public class AudioComponentData : ScriptableObject
    {
        [Searchable] [TableList(AlwaysExpanded = true)] [LabelText("音频内容")]
        public List<AudioInfo> audioInfos = new List<AudioInfo>();

        [Serializable]
        public class AudioInfo
        {
            [HideLabel] [HorizontalGroup("名称")] public string audioName;
            [HideLabel] [HorizontalGroup("片段")] public AudioClip audioClip;
        }
    }
}