using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.ConfigData
{
    /// <summary>
    /// 提示数据
    /// </summary>
    public class TipsData : ScriptableObject
    {
        public List<TipsDataInfo> tipsDataInfos;

        [Serializable]
        public class TipsDataInfo
        {
            [Header("提示索引")] public int tipIndex;
            [TextArea] [Header("提示内容")] public string tipsContent;
            [Header("对话音频")] public AudioClip tipsAudioClip;
            [Header("对应时长")] public float tipsLength;
        }
    }
}