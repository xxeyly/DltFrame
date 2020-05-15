using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    [Serializable]
    [CreateAssetMenu(fileName = "CustomBuildData", menuName = "配置文件/打包数据", order = 1)]
    public class CustomBuildData : ScriptableObject
    {
        /// <summary>
        /// 平台
        /// </summary>
        [Header("目标平台")] public BuildTarget buildTarget;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        [Header("输出文件夹")] public string exportPath;

        /// <summary>
        /// 项目名称
        /// </summary>
        [Header("项目名称")] public string exportProjectName;

        /// <summary>
        /// 项目日期
        /// </summary>
        [Header("项目日期")] public bool projectNameDate;

        /// <summary>
        /// 项目类型
        /// </summary>
        [Header("项目类型")] public bool projectBuildEdition;

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        [Header("拷贝文件夹")] public List<string> copyFolderPaths = new List<string>();

        /// <summary>
        ///粘贴文件夹
        /// </summary>
        [Header("粘贴文件夹")] public List<string> pasteFolderPaths = new List<string>();

        /// <summary>
        /// 更新到服务器
        /// </summary>
        [Header("更新到服务器")] public bool updateToFtp;

        /// <summary>
        /// 拷贝文件夹数量
        /// </summary>
        [Header("拷贝文件夹数量")] public int copyFolderCount;

        /// <summary>
        /// 服务器地址
        /// </summary>
        [Header("服务器地址")] public string ftpServerPath;

        /// <summary>
        /// Ftp用户名
        /// </summary>
        [Header("目标平台")] public string ftpUser;

        /// <summary>
        /// Ftp密码
        /// </summary>
        [Header("Ftp密码")] public string ftpPwd;

        /// <summary>
        /// Ftp根目录
        /// </summary>
        [Header("Ftp根目录")] public string ftpRoot;
    }
}