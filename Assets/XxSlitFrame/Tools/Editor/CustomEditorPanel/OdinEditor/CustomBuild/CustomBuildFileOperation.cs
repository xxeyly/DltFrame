﻿using System;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor.CustomBuild
{
    public class CustomBuildFileOperation
    {
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        public static void Copy(string sourceDirName, string destDirName)
        {
            if (sourceDirName.Substring(sourceDirName.Length - 1) != "\\")
            {
                sourceDirName = sourceDirName + "\\";
            }

            if (destDirName.Substring(destDirName.Length - 1) != "\\")
            {
                destDirName = destDirName + "\\";
            }

            if (Directory.Exists(sourceDirName))
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                foreach (string item in Directory.GetFiles(sourceDirName))
                {
                    if (item.Contains("meta"))
                    {
                        continue;
                    }

                    File.Copy(item, destDirName + Path.GetFileName(item), true);
                }

                foreach (string item in Directory.GetDirectories(sourceDirName))
                {
                    Copy(item, destDirName + item.Substring(item.LastIndexOf("\\", StringComparison.Ordinal) + 1));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exportPath">输出路径</param>
        /// <param name="chineseShell">中文外壳</param>
        /// <param name="exportCnProjectName">中文外壳名称</param>
        /// <param name="exportEnProjectName">英文外壳</param>
        /// <returns></returns>
        public static string GetProjectPath(string exportPath, bool chineseShell, string exportCnProjectName,
            string exportEnProjectName)
        {
            //打包路径
            string path = "";
            path += exportPath + "/";
            string chinsesPath = "";
            //如果启动中文外壳
            if (chineseShell)
            {
                chinsesPath += exportCnProjectName;
                chinsesPath += "/";
            }

            path += chinsesPath;
            path += exportEnProjectName;

            return path;
        }

        /// <summary>
        /// 保存版本数据
        /// </summary>
        public void SaveBuildData(CustomBuildData _buildData)
        {
            ResSvc.VersionInfo version = new ResSvc.VersionInfo
            {
                watermark = _buildData.versionWatermark, downLoad = _buildData.versionDownLoad,
                loadingProgress = _buildData.versionLoadingProgress, sceneProgress = _buildData.versionSceneProgress,
                assessmentTime = _buildData.versionAssessmentTime
            };
            string versionData = Encoding.UTF8.GetString(Encoding.Default.GetBytes(JsonMapper.ToJson(version)));

            ResSvc.FileOperation.SaveTextToLoad(Application.dataPath + "/XxSlitFrame/Resources/VersionData",
                "VersionInfo.Json", versionData);
        }
    }
}