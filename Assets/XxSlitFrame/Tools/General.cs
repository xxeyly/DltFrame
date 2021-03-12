using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Tools
{
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
            [LabelText("一次")] Once,
            [LabelText("循环")] Loop,
            [LabelText("不死")] Immortal,
        }

        [LabelText("任务ID")] public int tid;
        [LabelText("任务名称")] public string tidName;
        [LabelText("任务类型")] public TimeLoopType loopType;
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

        public static string BaseWindowTemplatePath = "Assets/XxSlitFrame/Tools/General/BaseWindowTemplate.cs";

        public static string ChildBaseWindowTemplatePath =
            "Assets/XxSlitFrame/Tools/General/ChildBaseWindowTemplate.cs";
        
        [FilePath] [LabelText("自动打包配置存放路径")] [LabelWidth(120)]
        public static string customBuildDataPath = "Assets/XxSlitFrame/Config/CustomBuildData.asset";

        [FilePath] [LabelText("音频配置存放路径")] [LabelWidth(120)]
        public  static string customAudioDataPath = "Assets/XxSlitFrame/Config/CustomAudioData.asset";

        [FilePath] [LabelText("框架配置存放路径")] [LabelWidth(120)]
        public  static string customFrameDataPath = "Assets/XxSlitFrame/Config/CustomFrameData.asset";

        [FilePath] [LabelText("生成配置存放路径")] [LabelWidth(120)]
        public static string generateBaseWindowPath = "Assets/XxSlitFrame/Config/GenerateBaseWindowData.asset";

        [FilePath] [LabelText("场景配置存放路径")] [LabelWidth(120)]
        public static string sceneLoadPath = "Assets/XxSlitFrame/Config/SceneLoadData.asset";

        [FilePath] [LabelText("场景配置存放路径")] [LabelWidth(120)]
        public static string buildSceneAssetBundleDataPath =
            "Assets/XxSlitFrame/Config/BuildSceneAssetBundleData.asset";
    }
}