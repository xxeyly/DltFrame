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
            拔针注射器,
            消毒手臂,
        }

        public List<PropItemGroupInfoGroup> groupInfos;

        [Serializable]
        public class PropItemGroupInfoGroup
        {
            public List<PropItemGroupInfo> propItemGroupInfos;
        }

        [Serializable]
        public class PropItemGroupInfo
        {
            public List<PropItemInfo> propItemGroupInfo;
        }

        [Serializable]
        public class PropItemInfo
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