using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using Sirenix.OdinInspector;
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
        [SerializeField] private SceneFile.SceneInfo _sceneInfo;

        #region 异步加载场景

        private AsyncOperation _sceneAsyncOperation;

        public delegate void AsyncLoadSceneProgressDelegate(float progress, bool over);

        public AsyncLoadSceneProgressDelegate asyncLoadSceneProgress;

        #endregion

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void WindowClose();
#endif

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
            DownSvc.Instance.downTaskDelegate += DownSceneTaskOver;
        }


        private void DownSceneTaskOver(float progress, bool downOver)
        {
            if (downOver)
            {
                //直接加载缓存场景文件
                AssetBundle.LoadFromFile(General.GetPlatformDownLoadDataPath() + DownSvc.Instance.GetGetSceneFileCachePath(_sceneInfo.sceneName));
                switch (_sceneInfo.sceneLoadType)
                {
                    case SceneFile.SceneLoadType.下载同步:
                        LoadSynchronizationScene(_sceneInfo.sceneName);
                        break;
                    case SceneFile.SceneLoadType.下载异步:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public override void EndSvc()
        {
            SceneManager.sceneLoaded -= SceneLoadOverCallBack;
            DownSvc.Instance.downTaskDelegate -= DownSceneTaskOver;
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

            if (_sceneAsyncOperation != null)
            {
                asyncLoadSceneProgress.Invoke(_sceneAsyncOperation.progress, _sceneAsyncOperation.isDone);
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
                }
            }
        }

        public void NewSceneLoad(string sceneName)
        {
            _sceneName = sceneName;
            _sceneInfo = GetSceneLoadTypeBySceneName(sceneName);
            switch (_sceneInfo.sceneLoadType)
            {
                case SceneFile.SceneLoadType.不加载:
                    break;
                case SceneFile.SceneLoadType.同步:
                    LoadSynchronizationScene(sceneName);
                    break;
                case SceneFile.SceneLoadType.异步:
                    _sceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
                    break;
                case SceneFile.SceneLoadType.下载同步:
                    if (Application.CanStreamedLevelBeLoaded(sceneName))
                    {
                        LoadSynchronizationScene(sceneName);
                    }
                    else
                    {
                        //当前文件是否下载过了
                        bool sceneFileIsDownOver = DownSvc.Instance.GetSceneFileCacheState(sceneName);
                        if (sceneFileIsDownOver)
                        {
                            //直接加载缓存场景文件
                            AssetBundle.LoadFromFile(General.GetPlatformDownLoadDataPath() + DownSvc.Instance.GetGetSceneFileCachePath(sceneName));
                            LoadSynchronizationScene(sceneName);
                        }
                        else
                        {
                            //开启下载场景任务
                            DownSvc.Instance.DownSceneTask(sceneName);
                        }
                    }


                    break;
                case SceneFile.SceneLoadType.下载异步:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 加载同步场景
        /// </summary>
        private void LoadSynchronizationScene(string sceneName)
        {
            Debug.Log("加载同步场景");
            if (!GameRootStart.Instance.dontDestroyOnLoad)
            {
                Destroy(GameRootStart.Instance.gameObject);
            }

            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 获得场景加载方式
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private SceneFile.SceneInfo GetSceneLoadTypeBySceneName(string sceneName)
        {
            SceneFile sceneFile = JsonMapper.ToObject<SceneFile>(Resources.Load<TextAsset>("DownFile/SceneLoadInfo").text);
            foreach (SceneFile.SceneInfo sceneInfo in sceneFile.sceneInfoList)
            {
                if (sceneInfo.sceneName == sceneName)
                {
                    return sceneInfo;
                }
            }

            return new SceneFile.SceneInfo();
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

            //所有条件都加载完毕后,开始视图的初始化
            foreach (SvcBase svcBase in GameRootStart.Instance.activeSvcBase)
            {
                if (svcBase.sceneInit)
                {
                    svcBase.InitSvc();
                }
            }

            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景服务加载完毕");

            for (int i = 0; i < GameRootStart.Instance.sceneStartSingletons.Count; i++)
            {
                GameRootStart.Instance.sceneStartSingletons[i].StartSvc();
                GameRootStart.Instance.sceneStartSingletons[i].Init();
            }

            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景工具加载完毕");
            GameRootStart.Instance.FrameSceneLoadEnd();
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public void SceneEsc()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.Quit();
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
#pragma warning disable 0618
#if UNITY_WEBGL
                WindowClose();
#endif
                // Application.Quit();
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

        /// <summary>
        /// 下载文件
        /// </summary>
        [Serializable]
        public class SceneFile
        {
            /// <summary>
            /// 场景文件列表
            /// </summary>
            [Header("版本信息列表")] public List<SceneInfo> sceneInfoList;

            /// <summary>
            /// 场景文件信息
            /// </summary>
            [Serializable]
            public struct SceneInfo
            {
                [Header("场景名称")] public string sceneName;
                [Header("场景加载方式")] public SceneLoadType sceneLoadType;
            }

            [LabelText("场景加载方式")]
            public enum SceneLoadType
            {
                不加载,
                同步,
                异步,
                下载同步,
                下载异步
            }
        }
    }
}