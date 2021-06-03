using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 场景服务--用于场景的加载
    /// </summary>
    public class SceneSvc : SvcBase
    {
        public static SceneSvc Instance;
        public override void StartSvc()
        {
            Instance = GetComponent<SceneSvc>();
            SceneManager.sceneLoaded += SceneLoadOverCallBack;
        }

        /// <summary>
        /// 场景加载完毕回调
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneType"></param>
        private void SceneLoadOverCallBack(Scene scene, LoadSceneMode sceneType)
        {
            InitSceneStartSingletons();
        }

        public override void InitSvc()
        {
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void SceneLoad(string sceneName)
        {
            SceneLoadBeforeInit();
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                DownSvc.DownData currentSceneDownData = DownSvc.Instance.GetDownSvcDataByFileName(sceneName);
                if (currentSceneDownData != null)
                {
                    if (currentSceneDownData.downOver)
                    {
                        AssetBundle.LoadFromMemory(currentSceneDownData.downContent);
                        while (Application.CanStreamedLevelBeLoaded(sceneName))
                        {
                            SceneManager.LoadScene(sceneName);
                            return;
                        }
                    }
                    else
                    {
                        DownSvc.Instance.InsertDownTask(sceneName);
                        ViewSvc.Instance.ShowView(typeof(DownProgress));
                        ListenerSvc.Instance.ExecuteEvent("OnShowDownLoadProgress", sceneName);
                    }
                }
            }
        }

        /// <summary>
        /// 场景跳转之前的初始化操作
        /// </summary>
        private void SceneLoadBeforeInit()
        {
            //全局禁止响应,不能继续点击
            ViewSvc.Instance.NoAllResponse();
            ViewSvc.Instance.AllViewDestroy();
            //删除所有计时任务
            TimeSvc.Instance.DeleteSwitchTask();
            TimeSvc.Instance.DeleteTimeTask();
            //音频提示播放
            AudioSvc.Instance.StopEffectAudio();
            AudioSvc.Instance.StopTipAndDialogAudio();
        }

        /// <summary>
        /// 加载场景初始化单例
        /// </summary>
        public void InitSceneStartSingletons()
        {
            GameRootStart.Instance.sceneStartSingletons = new List<StartSingleton>(FindObjectsOfType<StartSingleton>());

            for (int i = 0; i < GameRootStart.Instance.sceneStartSingletons.Count; i++)
            {
                GameRootStart.Instance.sceneStartSingletons[i].StartSvc();
                GameRootStart.Instance.sceneStartSingletons[i].Init();
            }

            //所有条件都加载完毕后,开始视图的初始化
            foreach (SvcBase svcBase in GameRootStart.Instance.activeSvcBase)
            {
                if (svcBase.sceneInit)
                {
                    svcBase.InitSvc();
                }
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public void SceneEsc()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Application.Quit();
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
#pragma warning disable 0618
                Application.Quit();
                // Application.ExternalCall("close", "close");
#pragma warning restore 0618
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                Debug.Log("Quit");
            }
        }
    }
}