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
    /// 场景组件--用于场景的加载
    /// </summary>
    public class SceneLoadComponent : ComponentBase
    {
        public static SceneLoadComponent Instance;
#pragma warning disable 649
        private DownComponent.DownData _currentSceneDownData;
#pragma warning restore 649
        private string _sceneName;
        private bool _asyncLoad;
        private AssetBundle _sceneAssetBundle;
        private SceneFile.SceneInfo _sceneInfo;

        #region 异步加载场景

        private AsyncOperation _sceneAsyncOperation;

        public delegate void AsyncLoadSceneProgressDelegate(float progress, bool over);

        public AsyncLoadSceneProgressDelegate asyncLoadSceneProgress;

        #endregion

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void WindowClose();
#endif

        public override void FrameInitComponent()
        {
            Instance = GetComponent<SceneLoadComponent>();
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

        public override void SceneInitComponent()
        {
            if (DownComponent.Instance != null)
            {
                DownComponent.Instance.downTaskDelegate += DownSceneTaskOver;
            }
        }


        private void DownSceneTaskOver(float progress, bool downOver)
        {
            if (downOver)
            {
                //直接加载缓存场景文件
                AssetBundle.LoadFromFile(General.GetPlatformDownLoadDataPath() + DownComponent.Instance.GetGetSceneFileCachePath(_sceneInfo.sceneName));
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


        public override void EndComponent()
        {
            SceneManager.sceneLoaded -= SceneLoadOverCallBack;
            if (DownComponent.Instance != null)
            {
                DownComponent.Instance.downTaskDelegate -= DownSceneTaskOver;
            }
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

        public void SceneLoad(string sceneName)
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
                        bool sceneFileIsDownOver = DownComponent.Instance.GetSceneFileCacheState(sceneName);
                        if (sceneFileIsDownOver)
                        {
                            //直接加载缓存场景文件
                            AssetBundle.LoadFromFile(General.GetPlatformDownLoadDataPath() + DownComponent.Instance.GetGetSceneFileCachePath(sceneName));
                            LoadSynchronizationScene(sceneName);
                        }
                        else
                        {
                            //开启下载场景任务
                            DownComponent.Instance.DownSceneTask(sceneName);
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

            //处理场景加载时需要卸载的逻辑
            GameRootStart.Instance.ComponentEnd();
            ViewComponent.Instance.AllViewDestroy();

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
        /// 加载场景初始化单例
        /// 加载顺序 场景组件-场景工具-View静态界面
        /// </summary>
        private void InitSceneStartSingletons()
        {
            // GameRootStart.Instance.sceneStartSingletons = new List<StartSingleton>(FindObjectsOfType<StartSingleton>());
            GameRootStart.Instance.sceneStartSingletons = DataComponent.GetAllObjectsInScene<SceneComponent>();

            //所有条件都加载完毕后,开始视图的初始化
            foreach (ComponentBase componentBase in GameRootStart.Instance.activeComponentBase)
            {
                if (componentBase.sceneInit)
                {
                    componentBase.SceneInitComponent();
                }
            }

            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景组件加载完毕");

            for (int i = 0; i < GameRootStart.Instance.sceneStartSingletons.Count; i++)
            {
                GameRootStart.Instance.sceneStartSingletons[i].StartComponent();
            }

            for (int i = 0; i < GameRootStart.Instance.sceneStartSingletons.Count; i++)
            {
                GameRootStart.Instance.sceneStartSingletons[i].InitComponent();
            }

            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景工具加载完毕");

            DataComponent.GetObjectsInScene<SceneComponentInit>()?.InitComponent();
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