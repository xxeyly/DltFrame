using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class DownFileData : ScriptableObject
    {
        public DownFile downFile = new DownFile();

        /// <summary>
        /// 下载文件
        /// </summary>
        [Serializable]
        public class DownFile
        {
            /// <summary>
            /// 下载文件列表
            /// </summary>
            [LabelText("版本信息列表")] public List<FileInfo> fileInfoList;

            /// <summary>
            /// 下载文件信息
            /// </summary>
            [Serializable]
            public struct FileInfo
            {
                [LabelText("文件名称")] public string fileName;
                [LabelText("文件原名称")] public string fileOriginalName;

                [LabelText("文件路径")] public string filePath;
                [LabelText("文件大小")] public long fileSize;
                [LabelText("文件MD5")] public string fileMd5;
            }
        }
    }
}