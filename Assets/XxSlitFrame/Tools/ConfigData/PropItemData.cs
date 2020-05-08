using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "PropItemData", menuName = "配置文件/物品数据", order = 1)]
    public class PropItemData : ScriptableObject
    {
        public enum PropType
        {
            Normal,
            医生静态口罩,
            医生静态左手套,
            医生静态右手套,
            桡动脉被子,
            口罩,
            手套,
            拔针注射器,
            桡动脉穿刺消毒道具,
            桡动脉穿刺消毒动画道具,
            道具车,
            桡动脉穿刺穿刺道具,
            消毒手臂,
            股动脉穿刺道具,
            股动脉被子,
            股动脉棉棒,
            消毒液,
            患者上衣,
            静态患者,
            患者
        }

        public List<PropItemGroupInfo> groupInfos;

        [Serializable]
        public struct PropItemGroupInfo
        {
            public int group;
            public List<PropItemInfo> propItemGroupInfo;
        }

        [Serializable]
        public struct PropItemInfo
        {
            /// <summary>
            /// 道具类型
            /// </summary>
            public PropType propTypes;

            /// <summary>
            /// 显示隐藏
            /// </summary>
            public bool display;
        }
    }
}