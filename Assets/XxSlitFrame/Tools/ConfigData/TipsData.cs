using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    /// <summary>
    /// 提示数据
    /// </summary>
    [CreateAssetMenu(fileName = "TipsData", menuName = "配置文件/提示数据", order = 1)]
    public class TipsData : ScriptableObject
    {
        public List<TipsDataInfo> tipsDataInfos;

        [Serializable]
        public class TipsDataInfo
        {
            [Header("提示索引")] public int tipIndex;
            [TextArea] [Header("提示内容")] public string tipsContent;
            [Header("对话音频")] public AudioClip tipsAudioClip;
        }
    }
}