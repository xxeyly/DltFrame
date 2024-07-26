using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    /// <summary>
    /// 场景组件--用于场景的加载
    /// </summary>
    public class SceneLoadFrameComponent : FrameComponent
    {
        public static SceneLoadFrameComponent Instance;

        #region 异步加载场景

        private List<ISceneLoadFrame> _sceneLoadFrames = new List<ISceneLoadFrame>();

        #endregion


        private AsyncOperation _tempSceneAsyncOperation;


        public override void FrameInitComponent()
        {
            Instance = this;
        }

        public override void FrameSceneInitComponent()
        {
            _sceneLoadFrames = DataFrameComponent.Hierarchy_GetAllObjectsInScene<ISceneLoadFrame>();
        }

        public override void FrameSceneEndComponent()
        {
        }

        public override void FrameEndComponent()
        {
        }

        [LabelText("获得异步加载进度")]
        public float GetAsyncSceneProgress(string sceneName)
        {
            if (_tempSceneAsyncOperation.isDone)
            {
                return 1;
            }
            else
            {
                return _tempSceneAsyncOperation.progress;
            }
        }

        private void Update()
        {
            if (_tempSceneAsyncOperation != null)
            {
                foreach (ISceneLoadFrame sceneLoadFrame in _sceneLoadFrames)
                {
                    sceneLoadFrame.AsyncLoadSceneProgressDelegate(_tempSceneAsyncOperation.progress / 0.9f, _tempSceneAsyncOperation.isDone);
                }
            }
        }

        #region 加载同步场景

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneMode"></param>
        public async void SceneLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (GameRootStart.Instance.hotFixLoad)
            {
                Debug.Log("加载热更配置表");
                await HotFixFrameComponent.Instance.LoadHotFixSceneConfig(sceneName);
                Debug.Log("加载AssetBundle");
                await HotFixFrameComponent.Instance.InstantiateHotFixAssetBundle();
                Debug.Log("加载场景AssetBundle");
                await HotFixFrameComponent.Instance.LoadAssetBundleSceneToSystem(sceneName);
                //等待显示UI
                //第一个场景加载时,还未有加载进度功能,这里就取消等待
                //这里延迟是为了能够显示出UI
                if (sceneName != GameRootStart.Instance.initJumpSceneName)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.02));
                }
            }

            LoadSynchronizationScene(sceneName, loadSceneMode);
        }

        [LabelText("同步加载逻辑")]
        private void LoadSynchronizationScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            //处理场景加载时需要卸载的逻辑
            DebugFrameComponent.Log("卸载的场景" + SceneManager.GetActiveScene().name);
            GameRootStart.Instance.OldSceneDestroy(SceneManager.GetActiveScene().name);

            // Debug.Log("加载的场景" + sceneName);
            SceneManager.LoadScene(sceneName, loadSceneMode);
        }

        #endregion

        #region 加载异步场景

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneMode"></param>
        public void SceneAsyncLoad(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            LoadAsyncScene(sceneName, loadSceneMode);
        }

        [LabelText("异步加载逻辑")]
        private async void LoadAsyncScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (GameRootStart.Instance.hotFixLoad)
            {
                //加载热更配置表
                await HotFixFrameComponent.Instance.LoadHotFixSceneConfig(sceneName);
                Debug.Log("加载AssetBundle");
                await HotFixFrameComponent.Instance.InstantiateHotFixAssetBundle();
                //加载场景AssetBundle
                await HotFixFrameComponent.Instance.LoadAssetBundleSceneToSystem(sceneName);
                //等待显示UI
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            _tempSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            _tempSceneAsyncOperation.allowSceneActivation = false;
            await _tempSceneAsyncOperation;
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="unSceneName"></param>
        /// <param name="action"></param>
        public void UnScene(string unSceneName, Action action = null)
        {
            GameRootStart.Instance.unScene = SceneManager.GetSceneByName(unSceneName);
            //处理场景加载时需要卸载的逻辑
            GameRootStart.Instance.OldSceneDestroy(unSceneName);
            StartCoroutine(OnUnScene(unSceneName, action));
        }

        [LabelText("卸载逻辑")]
        IEnumerator OnUnScene(string unSceneName, Action action = null)
        {
            AsyncOperation unSceneAsyncOperation = SceneManager.UnloadSceneAsync(unSceneName);
            yield return unSceneAsyncOperation;
            action?.Invoke();
        }

        [LabelText("场景加载完毕")]
        public void AsyncSceneIsDone()
        {
            _tempSceneAsyncOperation.allowSceneActivation = true;
        }

        #endregion


        /// <summary>
        /// 退出程序
        /// </summary>
        public void SceneEsc()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
            }
            else
            {
                Application.Quit();
                Resources.UnloadUnusedAssets();
            }
        }
    }
}