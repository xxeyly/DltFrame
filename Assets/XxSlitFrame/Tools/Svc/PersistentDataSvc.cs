using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 加载场景方式
    /// </summary>
    public enum SceneLoadType
    {
        Normal,
        SceneName,
        SceneIndex
    }

    /// <summary>
    /// 动态加载数据
    /// </summary>
    public class PersistentDataSvc : SvcBase<PersistentDataSvc>
    {
        /// <summary>
        /// 场景索引
        /// </summary>
        [Header("场景索引")] public int sceneIndex;

        /// <summary>
        /// 场景名称
        /// </summary>
        [Header("场景名称")] public string sceneName;

        /// <summary>
        /// 跳转场景
        /// </summary>
        [Header("跳转场景")] public bool jump;

        /// <summary>
        /// 跳转场景索引
        /// </summary>
        [Header("跳转场景索引")] public string jumpSceneName;

        /// <summary>
        /// 音乐开关
        /// </summary>
        [Header("音乐开关")] public bool audioState;

        [Header("下载项目配置数据")] public ResSvc.VersionInfo versionInfo;
        [Header("下载文件配置数据")] public ResSvc.DownFile downFileInfo;
        [Header("下载文件配置数据完毕")] public bool downFileInfoOver;
        [Header("下载项目配置数据完毕")] public bool downVersionOver;
        [Header("下载数据完毕")] public bool downFileOver;

        [Header("下载文件数据")] [SerializeField] public Dictionary<string, byte[]> downFileData;


        /// <summary>
        /// 当前大步骤
        /// </summary>
        [Header("当前大步骤")] public int currentStepBigIndex;

        /// <summary>
        /// 当前小步骤
        /// </summary>
        [Header("当前小步骤")] public int currentStepSmallIndex;

        public override void InitSvc()
        {
            audioState = true;
        }

        /// <summary>
        /// 加载场景方式
        /// </summary>
        [Header("加载场景方式")] public SceneLoadType sceneLoadType;

        /// <summary>
        /// 服务器地址
        /// </summary>
        [Header("服务器地址")] public string serverPath;

        /// <summary>
        /// 设置服务器地址
        /// </summary>
        /// <param name="url"></param>
        private void SetServerURL(string url)
        {
            serverPath = url;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GetIP();
#endif
        public override void StartSvc()
        {
            base.StartSvc();
#if UNITY_WEBGL && !UNITY_EDITOR
            GetIP();
#endif
        }
    }
}