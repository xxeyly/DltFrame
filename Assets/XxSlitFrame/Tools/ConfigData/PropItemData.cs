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
            哈哈哈1,
            哈哈哈2,
            哈哈哈3,
            哈哈哈4,
            哈哈哈5,
            哈哈哈6,
            哈哈哈7,
            哈哈哈8,
            哈哈哈9,
            哈哈哈11,
            哈哈哈12,
            哈哈哈13,
            哈哈哈14,
            哈哈哈15,
            哈哈哈16,
            哈哈哈17,
            哈哈哈18,
            哈哈哈19,
            哈哈哈21,
            哈哈哈22,
            哈哈哈23,
            哈哈哈24,
            哈哈哈25,
            哈哈哈26,
        }

        public List<PropItemGroupInfo> groupInfos;


        [Serializable]
        public class PropItemGroupInfo
        {
            public List<PropItemInfo> propItemGroupInfo = new List<PropItemInfo>();
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