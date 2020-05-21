using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [CreateAssetMenu(fileName = "AssessmentScoreData", menuName = "配置文件/考核成绩数据", order = 1)]
    public class AssessmentScoreData : ScriptableObject
    {
        /// <summary>
        /// 总分
        /// </summary>
        public int TotalScore;

        public List<AssessmentScoreItemData> AssessmentScoreItemDatas;
    }

    [Serializable]
    public class AssessmentScoreItemData
    {
        /// <summary>
        /// 题号
        /// </summary>
        public int QuestionIndex;

        /// <summary>
        /// 题得分
        /// </summary>
        public float QuestionScore;

        /// <summary>
        /// 提满分
        /// </summary>
        public float QuestionFullScore;

        /// <summary>
        /// 操作次数
        /// </summary>
        public int NumberOperations;
    }
}