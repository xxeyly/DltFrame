using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.General
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum ListenerEventType
    {
        Normal,
    }

    public enum AnimType
    {
        Normal,

        /// <summary>
        /// 自检
        /// </summary>
        SelfInspectionPage
    }

    [LabelText("场景加载方式")]
    public enum SceneLoadType
    {
        Normal,
        [LabelText("场景名称")] SceneName,
        [LabelText("场景索引")] SceneIndex
    }

    [LabelText("场景质量")]
    public enum QualitySettingType
    {
        [LabelText("低")] Low,
        [LabelText("中")] Center,
        [LabelText("高")] High
    }

    [Serializable]
    public struct TimeTaskList
    {
        public enum TimeLoopType
        {
            [Header("一次")] Once,
            [Header("循环")] Loop,
            [Header("不死")] Immortal,
        }

        [Header("任务ID")] public int tid;
        [Header("任务名称")] public string tidName;
        [Header("任务类型")] public TimeLoopType loopType;
    }

    public static class General
    {
        #region 点击声音

        #endregion

        #region 视图时间

        [LabelText("视图切换时间")] public const float ViewSwitchTime = 2f;

        [LabelText("视图错误时间")] public const float ViewErrorTime = 1f;

        [LabelText("文件下载路径")] public const string DownFilePath = "/XxSlitFrame/Resources/DownFile/DownFileInfo.Json";

        #endregion

        [LabelText("获得网页跟目录地址")]
        public static string GetUrlRootPath()
        {
            string url = Application.absoluteURL;
            //当前网页的url
            int index = url.LastIndexOf('/');
            if (index > 0)
            {
                var path = url.Substring(0, index);
                path = path + '/';
                return path;
            }
            else
            {
                Debug.LogError("未找到文件");
                return "";
            }
        }
    }
}