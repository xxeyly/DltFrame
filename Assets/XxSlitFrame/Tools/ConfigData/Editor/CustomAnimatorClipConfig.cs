using System;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    [Serializable]
    public class CustomAnimatorClipConfig : ScriptableObject
    {
        /// <summary>
        /// 当前动画文件
        /// </summary>
        [Header("当前动画文件路径")] public GameObject currentFbx;

        /// <summary>
        /// 动画控制器名称
        /// </summary>
        [Header("项目名称")] public string animatorName;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        [Header("输出文件夹")] public string exportPath;

        /// <summary>
        /// 输出文件路径
        /// </summary>
        [Header("输出文件路径")] public string profilePath;
    }
}