using System.Collections.Generic;
using UnityEngine;
using AudioType = XxSlitFrame.Tools.Svc.AudioType;

namespace XxSlitFrame.Tools.General
{
    public static class General
    {
        #region 点击声音

        #endregion

        #region 视图时间

        /// <summary>
        /// 视图切换时间
        /// </summary>
        public const float ViewSwitchTime = 0.5f;

        /// <summary>
        /// 视图错误显示时间
        /// </summary>
        public const float ViewErrorTime = 1f;

        /// <summary>
        /// 版本信息位置
        /// </summary>
        public const string VersionDataInfoPath = "VersionData/VersionInfo.Json";

        /// <summary>
        /// 文件下载路径
        /// </summary>
        public const string DownFilePath = "DownFile/DownFileInfo.Json";

        #endregion

        #region 对话声音

        #endregion

        #region 网络地址

        public const string ServerAssetBundle = "http://127.0.0.1/AssetBundle/";

        public static readonly Dictionary<AudioType, string> AudioPath = new Dictionary<AudioType, string>()
        {
            {AudioType.EBackground, "Effect/Background"},
            {AudioType.EEnterExperiment, "Effect/EnterScene"},
            {AudioType.EErrorPopup, "Effect/TipsError"},
            {AudioType.ETipsPopup, "Effect/TipsPopup"},
            {AudioType.EClose, "Effect/Close"},
            {AudioType.ECloseBackground, "Effect/CloseBackGround"},
            {AudioType.ESure, "Effect/Sure"},
            {AudioType.ENextStep, "Effect/NextStep"},
            {AudioType.ERedo, "Effect/Redo"},
            {AudioType.EClick, "Effect/Click"},
            {AudioType.DisplayObjects, "Effect/DisplayObjects"},
        };

        #endregion

        /// <summary>
        /// 获得版本信息地址
        /// </summary>
        /// <returns></returns>
        public static string GetFileConfigPath(string relativePath)
        {
#if UNITY_5
            if (Application.isWebPlayer)
            {
#endif
#if UNITY_2017_1_OR_NEWER
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
#endif
                string url = Application.absoluteURL;
                //当前网页的url
                int index = url.LastIndexOf('/');
                if (index > 0)
                {
                    var path = url.Substring(0, index);
                    path = path + '/' + relativePath;
                    return path;
                }
                else
                {
                    Debug.LogError("未找到文件");
                    return "";
                }
            }
            else if (Application.isEditor)
            {
                return "file://" + Application.dataPath + "/XxSlitFrame/Resources/" + relativePath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获得文件数据地址
        /// </summary>
        /// <returns></returns>
        public static string GetFileDataPath(string relativePath)
        {
#if UNITY_5
            if (Application.isWebPlayer)
            {
#endif
#if UNITY_2017_1_OR_NEWER
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
#endif
                string url = Application.absoluteURL;
                //当前网页的url
                int index = url.LastIndexOf('/');
                if (index > 0)
                {
                    var path = url.Substring(0, index);
                    path = path + '/' + relativePath;
                    return path;
                }
                else
                {
                    Debug.LogError("未找到文件");
                    return "";
                }
            }
            else if (Application.isEditor)
            {
                return "file://" + Application.dataPath + "/XxSlitFrame/" + relativePath;
            }
            else
            {
                return "";
            }
        }
    }
}