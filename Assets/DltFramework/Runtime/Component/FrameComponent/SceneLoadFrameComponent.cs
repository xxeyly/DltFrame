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
        private bool _asyncLoad;

        #region 异步加载场景

        public delegate void AsyncLoadSceneProgressDelegate(float progress, bool over);

        [LabelText("异步加载进度委托")] public AsyncLoadSceneProgressDelegate asyncLoadSceneProgress;

        #endregion

        private AsyncOperation tempSceneAsyncOperation;


        public override void FrameInitComponent()
        {
            Instance = this;
        }

        public override void FrameSceneInitComponent()
        {
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
            if (tempSceneAsyncOperation.isDone)
            {
                return 1;
            }
            else
            {
                return tempSceneAsyncOperation.progress;
            }
        }

        private void Update()
        {
            if (tempSceneAsyncOperation != null)
            {
                if (tempSceneAsyncOperation.progress >= 1)
                {
                    asyncLoadSceneProgress?.Invoke(1, true);
                }
                else
                {
                    asyncLoadSceneProgress?.Invoke(1, false);
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
                //加载热更配置表
                await HotFixFrameComponent.Instance.LoadHotFixSceneConfig(sceneName);
                //加载场景AssetBundle
                await HotFixFrameComponent.Instance.LoadAssetBundleSceneToSystem(sceneName);
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
                //加载场景AssetBundle
                await HotFixFrameComponent.Instance.LoadAssetBundleSceneToSystem(sceneName);
            }

            tempSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            tempSceneAsyncOperation.allowSceneActivation = false;
            await tempSceneAsyncOperation;
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
            tempSceneAsyncOperation.allowSceneActivation = true;
        }

        #endregion


        /// <summary>
        /// 退出程序
        /// </summary>
        public void SceneEsc()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.Quit();
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}