using System;
using System.Collections.Generic;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "StepInitData", menuName = "配置文件/步骤初始化数据", order = 1)]
    public class StepInitData : ScriptableObject
    {
        [HideInInspector] [Header("当前场景的步骤组")] public List<StepInitDataInfo> stepInitDataInfoGroups;

        public StepInitDataInfo GetCurrentStepIndex()
        {
            foreach (StepInitDataInfo stepInitDataInfo in stepInitDataInfoGroups)
            {
                if (stepInitDataInfo.bigIndex == PersistentDataSvc.Instance.currentStepBigIndex && stepInitDataInfo.smallIndex == PersistentDataSvc.Instance.currentStepSmallIndex)
                {
                    return stepInitDataInfo;
                }
            }

            return new StepInitDataInfo();
        }
    }


    [Serializable]
    public class StepInitDataInfo
    {
        /// <summary>
        /// 大步骤索引
        /// </summary>
        public int bigIndex;

        /// <summary>
        /// 小步骤索引
        /// </summary>
        public int smallIndex;

        /// <summary>
        /// 播报的语音索引
        /// </summary>
        [Header("播报的语音索引")] public int tipIndex;

        /// <summary>
        /// 物品组索引
        /// </summary>
        [Header("物品组索引")] public int propGroupIndex;

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