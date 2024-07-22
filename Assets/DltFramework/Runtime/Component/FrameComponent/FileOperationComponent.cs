using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public class FileOperationComponent
    {
        private static FileStream _file;
        private static StreamReader _sr;
        private static StringBuilder _sb;
        private static byte[] _readAllBytes;

        /// <summary>
        /// 保存文本信息到本地
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="information">保存信息</param>
        public static void SaveTextToLoad(string path, string fileName, string information)
        {
            if (Directory.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(DataFrameComponent.String_BuilderString(path, "/", fileName), information, new UTF8Encoding(false));
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        private static byte[] _bts;

        public static void SaveTextToLoad(string path, string information)
        {
            if (File.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(DataFrameComponent.Path_GetPathDontContainFileName(path));
            }

            _file = new FileStream(path, FileMode.Create);
            //得到字符串的UTF8 数据流
            information = Regex.Unescape(information);
            _bts = Encoding.UTF8.GetBytes(information);
            // StreamWriter sw = new StreamWriter(aFile, Encoding.UTF8);
            // sw.WriteLine(information);
            // sw.Close();
            _file.Write(_bts, 0, _bts.Length);
            if (_file != null)
            {
                //清空缓存
                _file.Flush();
                // 关闭流
                _file.Close();
                //销毁资源
                _file.Dispose();
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        [LabelText("保存文件到本地")]
        public static void SaveFileToLocal(string path, string fileName, byte[] information, FileMode fileMode, int buffSize = 1024 * 1024)
        {
            _file = new FileStream(DataFrameComponent.String_BuilderString(path, "/", fileName), fileMode, FileAccess.Write);
            if (File.Exists(path))
            {
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            //是否被完整除开
            bool integer = information.Length % buffSize == 0;
            int numberCycles = 0;
            if (integer)
            {
                numberCycles = information.Length / buffSize;
            }
            else
            {
                numberCycles = information.Length / buffSize + 1;
            }

            for (int i = 0; i < numberCycles; i++)
            {
                if (integer)
                {
                    _file.Write(information, i * buffSize, buffSize);
                }
                else
                {
                    if (i < numberCycles - 1)
                    {
                        _file.Write(information, i * buffSize, buffSize);
                    }
                    else
                    {
                        _file.Write(information, i * buffSize, information.Length - i * buffSize);
                    }
                }
            }

            // DebugFrameComponent.Log(fileName + "写入次数" + numberCycles);
            // 关闭流
            _file.Close();
        }

        /// <summary>
        /// 读取本地文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetTextToLoad(string path, string fileName)
        {
            if (Directory.Exists(path))
            {
            }
            else
            {
                DebugFrameComponent.LogError("文件不存在:" + path + "/" + fileName);
            }

            _file = new FileStream(DataFrameComponent.String_BuilderString(path, "/", fileName), FileMode.Open);
            _sr = new StreamReader(_file);
            var textData = _sr.ReadToEnd();
            _sr.Close();
            return textData;
        }

        /// <summary>
        /// 读取本地文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string GetTextToLoad(string path)
        {
            if (File.Exists(path))
            {
            }
            else
            {
                DebugFrameComponent.LogError("文件不存在:" + path);
            }

            _file = new FileStream(path, FileMode.Open);
            _sr = new StreamReader(_file);
            var textData = _sr.ReadToEnd();
            _sr.Close();
            return textData;
        }

        /// <summary>
        /// 转换为本地路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertToLocalPath(string path)
        {
            return path.Remove(0, path.IndexOf("Assets", StringComparison.Ordinal));
        }

        private static byte[] _retVal;

        [LabelText("获取文件的md5校验码")]
        public static string GetMD5HashFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                _file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                _retVal = md5.ComputeHash(_file);
                _file.Close();
                _sb = new StringBuilder();
                for (int i = 0; i < _retVal.Length; i++)
                {
                    _sb.Append(_retVal[i].ToString("x2"));
                }

                return _sb.ToString();
            }

            return null;
        }

        public static long GetFileSize(string fileName)
        {
            if (File.Exists(fileName))
            {
                _file = new FileStream(fileName, FileMode.Open);
                long size = _file.Length;
                _file.Dispose();
                return size;
            }

            return 0;
        }

        [LabelText("获得文件的字节")]
        public static string GetFileByte(string filePath, int startIndex, int endIndex)
        {
            string byteContent = "";
            _readAllBytes = File.ReadAllBytes(filePath);
            for (int i = 0; i < _readAllBytes.Length; i++)
            {
                if (i >= startIndex && i < endIndex)
                {
                    byteContent = DataFrameComponent.String_BuilderString(byteContent, _readAllBytes[i].ToString());
                }
            }

            return byteContent;
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        public static void Copy(string sourceDirName, string destDirName)
        {
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

                    if (destDirName[destDirName.Length - 1] != '/')
                    {
                        destDirName = DataFrameComponent.String_BuilderString(destDirName, "/");
                    }

                    File.Copy(item, DataFrameComponent.String_BuilderString(destDirName, "/", Path.GetFileName(item)), true);
                }

                foreach (string item in Directory.GetDirectories(sourceDirName))
                {
                    Copy(DataFrameComponent.String_BuilderString(item, "/"), DataFrameComponent.String_BuilderString(destDirName, "/", DataFrameComponent.Path_GetPathFileName(item)));
                }
            }
            else
            {
                DebugFrameComponent.Log(sourceDirName + "不存在");
            }
        }

        public static void CopyFile(string sourcePath, string destinationPath)
        {
            if (!Directory.Exists(DataFrameComponent.Path_GetPathDontContainFileName(destinationPath)))
            {
                Directory.CreateDirectory(DataFrameComponent.Path_GetPathDontContainFileName(destinationPath));
            }

            File.Copy(sourcePath, destinationPath, true);
        }
    }
}