﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    [LabelText("场景质量")]
    public enum QualitySettingType
    {
        [LabelText("低")] Low,
        [LabelText("中")] Center,
        [LabelText("高")] High
    }

    [Serializable]
    public class TimeTaskList
    {
        public enum TimeLoopType
        {
            [LabelText("单程")] Once,
            [LabelText("循环")] Loop,
            [LabelText("不死")] Immortal,
        }

        [HideLabel] [HorizontalGroup("任务ID")] public int tid;
        [HideLabel] [HorizontalGroup("任务名称")] public string tidName;
        [HideLabel] [HorizontalGroup("结束时间")] public float endTime;
        [HideLabel] [HorizontalGroup("等待时间")] public float waitingTime;
        [HideLabel] [HorizontalGroup("任务类型")] public TimeLoopType loopType;
    }

    public static class General
    {

        #region 视图时间

        [LabelText("视图切换时间")] public const float ViewSwitchTime = 2f;

        [LabelText("视图错误时间")] public const float ViewErrorTime = 1f;

        [LabelText("文件下载路径")] public const string DownFilePath = "/XFramework/Resources/DownFile/DownFileInfo.Json";

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

        /// <summary>
        /// 获得文件数据地址
        /// </summary>
        /// <returns></returns>
        public static string GetFileDataPath(string relativePath)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return GetUrlRootPath() + relativePath;
            }
            else if (Application.isEditor)
            {
                return "file://" + Application.dataPath + "/" + relativePath;
            }
            else
            {
                return "";
            }
        }

        [LabelText("BaseWindow模板地址")] public static string BaseWindowTemplatePath = "Assets/XFramework/Model/Template/BaseWindowTemplate.cs";

        [LabelText("子级BaseWindow模板地址")] public static string ChildBaseWindowTemplatePath = "Assets/XFramework/Model/Template/ChildBaseWindowTemplate.cs";

        [LabelText("自动打包配置存放路径")] public static string customBuildDataPath = "Assets/XFramework/Config/CustomBuildData.asset";

        [LabelText("音频配置存放路径")] public static string customAudioDataPath = "Assets/XFramework/Config/CustomAudioData.asset";

        [LabelText("框架配置存放路径")] public static string customFrameDataPath = "Assets/XFramework/Config/CustomFrameData.asset";

        [LabelText("生成配置存放路径")] public static string generateBaseWindowPath = "Assets/XFramework/Config/GenerateBaseWindowData.asset";

        [LabelText("场景配置存放路径")] public static string sceneLoadPath = "Assets/XFramework/Config/SceneLoadData.asset";

        [LabelText("场景配置存放路径")] public static string buildSceneAssetBundleDataPath = "Assets/XFramework/Config/BuildSceneAssetBundleData.asset";

        public enum GenerateAttributesType
        {
            @int,
            @string,
            @Sprite,
            @bool,
            @GameObject,
            @Transform,
            @Camera,
            @Color
        }

        [Serializable]
        public struct GenerateAttributesTypeGroup
        {
            [HorizontalGroup("属性类型")] [HideLabel] public GenerateAttributesType generateAttributesType;
            [HorizontalGroup("属性名称")] [HideLabel] public string attributesName;
            [HorizontalGroup("属性描述")] [HideLabel] public string attributesDescription;
            
        }
    }
}