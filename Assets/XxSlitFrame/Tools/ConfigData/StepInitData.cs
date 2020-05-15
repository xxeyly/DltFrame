using System;
using System.Collections.Generic;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.View;
using XxSlitFrame.View.Button;
using XxSlitFrame.View.CustomInspector;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "StepInitData", menuName = "配置文件/步骤初始化数据", order = 1)]
    public class StepInitData : ScriptableObject
    {
        [Header("当前场景的步骤组")] public List<StepInitDataInfoGroup> stepInitDataInfoGroups;
    }

    [Serializable]
    public struct StepInitDataInfoGroup
    {
        public string currentBigSmallName;
        public List<StepInitDataInfo> stepInitDataInfos;
    }

    [Serializable]
    public struct StepInitDataInfo
    {
        /// <summary>
        /// 播报的语音索引
        /// </summary>
        [Header("播报的语音索引")] public int tipIndex;

        /// <summary>
        /// 要移动的位置
        /// </summary>
        [Header("要移动的位置")] public CameraPosData.CameraPosType cameraPosType;

        /// <summary>
        /// 要播放的动画类型
        /// </summary>
        [Header("要播放的动画类型")] public AnimType animType;

        /// <summary>
        /// 要播放的动画进度
        /// </summary>
        [Header("要播放的动画进度")] public AnimSpeedProgress animSpeedProgress;
    }
}