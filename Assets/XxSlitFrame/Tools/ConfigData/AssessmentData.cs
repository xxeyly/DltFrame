using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [CreateAssetMenu(fileName = "AssessmentData", menuName = "配置文件/考核数据", order = 1)]
    [Serializable]
    public class AssessmentData : ScriptableObject
    {
        /// <summary>
        /// 本次操作记录的id 如果是练习就是练习的id,如果是自测就是自测的id，如果是考核就是考核的id
        /// </summary>
        public string pid;

        /// <summary>
        /// 总分数
        /// </summary>
        public string zfs;

        /// <summary>
        /// 及格分数
        /// </summary>
        public string jgfs;

        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        public string updateTime;

        /// <summary>
        /// 本次操作用时
        /// </summary>
        public string useTime;

        /// <summary>
        /// 本次得分
        /// </summary>
        public string score;

        /// <summary>
        /// 状态
        /// </summary>
        public string status;

        /// <summary>
        /// 用户考核提交数据
        /// </summary>
        public List<TopicInfoData> list;
    }

    /// <summary>
    /// 存储数据小类
    /// </summary>
    [Serializable]
    public class TopicInfoData
    {
        public string title; //题目或实验步骤名称-------------------------------------------------------------------------需要面板填写
        public string number; //题号或实验步骤，在本次实验或考试中不能重复------------------------------------------------需要面板填写
        public string type; //1.试题、2.操作步骤--------------------------------------------------------------------------需要面板填写
        public string op; //选项  "A:选项说明;B:选项说明"-----------------------------------------------------------------需要面板填写
        public string isSuccess; //结果是否正确,0不正确，1正确              非必填项 
        public string zqda; //题目的正确选项或正确操作步骤，多个直接用分号分隔  ”A:选项说明；B:选项说明….”-------------需要面板填写
        public string useTime; //本次操作的用时,整数？ = "0"             必填项 直接填0  数据上传时自动填0
        public string unit; //单位  ”秒”--------------------------------------------------------------------------------默认填秒
        public string opCount; //操作次数，整数？            非必填项     做对或做错都填1次或真实次数
        public string score; //本次操作的得分？             必填项
        public string otherData; //其他数据
        public string time; //提交时间
        public string fs; //题目或步骤的分数------------------------------------------------------------------------------需要面板填写
    }
}