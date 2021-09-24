using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    /// <summary>
    /// 场景服务--用于场景的加载
    /// </summary>
    public class SceneSvc : SvcBase
    {
        public static SceneSvc Instance;
        private DownSvc.DownData _currentSceneDownData;
        private string _sceneName;
        private bool _asyncLoad;
        private AssetBundle _sceneAssetBundle;
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

        public override void EndSvc()
        {
            SceneManager.sceneLoaded -= SceneLoadOverCallBack;
        }

        private void Update()
        {
            if (_asyncLoad)
            {
                if (_currentSceneDownData != null)
                {
                    if (_currentSceneDownData.downOver)
                    {
                        _sceneAssetBundle = AssetBundle.LoadFromMemory(_currentSceneDownData.downContent);
                        while (Application.CanStreamedLevelBeLoaded(_sceneName))
                        {
                            // SceneManager.LoadScene(_sceneName);
                            Debug.Log("场景加载完毕:" + _sceneName);
                            _asyncLoad = false;
                            _sceneName = String.Empty;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void SceneLoad(string sceneName)
        {
            _sceneName = sceneName;
            SceneLoadBeforeInit();
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                Debug.Log("加载同步场景");
                if (!GameRootStart.Instance.dontDestroyOnLoad)
                {
                    Destroy(GameRootStart.Instance.gameObject);
                }
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                _currentSceneDownData = DownSvc.Instance.GetDownSvcDataByFileName(sceneName);
                if (_currentSceneDownData != null)
                {
                    DownSvc.Instance.InsertDownTask(sceneName);
                    ViewSvc.Instance.ShowView(typeof(DownProgress));
                    _asyncLoad = true;
                    ListenerSvc.Instance.ExecuteEvent("OnShowDownLoadProgress", _sceneName);
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
            //音频停止播放
            AudioSvc.Instance.StopEffectAudio();
            AudioSvc.Instance.StopTipAndDialogAudio();
        }

        /// <summary>
        /// 加载场景初始化单例
        /// 加载顺序 场景服务-场景工具-View静态界面
        /// </summary>
        private void InitSceneStartSingletons()
        {
            // GameRootStart.Instance.sceneStartSingletons = new List<StartSingleton>(FindObjectsOfType<StartSingleton>());
            GameRootStart.Instance.sceneStartSingletons = DataSvc.GetAllObjectsInScene<StartSingleton>();

            for (int i = 0; i < GameRootStart.Instance.sceneStartSingletons.Count; i++)
            {
                GameRootStart.Instance.sceneStartSingletons[i].StartSvc();
                GameRootStart.Instance.sceneStartSingletons[i].Init();
            }

            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景工具加载完毕");
            //所有条件都加载完毕后,开始视图的初始化
            foreach (SvcBase svcBase in GameRootStart.Instance.activeSvcBase)
            {
                if (svcBase.sceneInit)
                {
                    svcBase.InitSvc();
                }
            }

            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景服务加载完毕");
            GameRootStart.Instance.FrameSceneLoadEnd();
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