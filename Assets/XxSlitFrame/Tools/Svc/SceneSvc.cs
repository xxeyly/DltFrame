using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.Svc.BaseSvc;
using XxSlitFrame.View.InitView;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 场景服务--用于场景的加载
    /// </summary>
    public class SceneSvc : SvcBase<SceneSvc>
    {
        private AsyncOperation _asyncOperation;
        private int _sceneLoadTimeTask;
        private int _sceneLoadOverTimeTask;
        private int _asyncSceneLoadProgressTimeTask;


        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void SceneLoad(int sceneIndex)
        {
            PersistentDataSvc.Instance.sceneLoadType = SceneLoadType.SceneIndex;
            SceneLoadBeforeInit();
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void SceneLoad(string sceneName)
        {
            PersistentDataSvc.Instance.sceneLoadType = SceneLoadType.SceneName;
            SceneLoadBeforeInit();
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 场景跳转之前的初始化操作
        /// </summary>
        private void SceneLoadBeforeInit()
        {
            //全局禁止响应,不能继续点击
            ViewSvc.Instance.NoAllResponse();
            //关闭除"不死"类型的所有任务
            TimeSvc.Instance.DeleteTimeTask();
            TimeSvc.Instance.DeleteSwitchTask();
            //音频提示播放
            AudioSvc.Instance.StopEffectAudio();
            AudioSvc.Instance.StopTipAndDialogAudio();
        }

        /// <summary>
        /// 异步加载场景
        /// 加载场景完毕后,视图服务重新启动
        /// </summary>
        /// <param name="sceneIndex">跳转场景的索引</param>
        public void AsyncSceneLoad(int sceneIndex)
        {
            PersistentDataSvc.Instance.sceneLoadType = SceneLoadType.SceneIndex;

            //全局禁止响应
            ViewSvc.Instance.NoAllResponse();
            SceneLoadBeforeInit();
            if (PersistentDataSvc.Instance.versionInfo.sceneProgress)
            {
                StartUpdateSceneLoadProgress();
            }

            StartCoroutine(LoadAsyncScene(sceneIndex));
            StartAsyncSceneLoadTimeTask();
        }

        /// <summary>
        /// 异步加载场景
        /// 加载场景完毕后,视图服务重新启动
        /// </summary>
        public void AsyncSceneLoad(string sceneName)
        {
            PersistentDataSvc.Instance.sceneLoadType = SceneLoadType.SceneName;
            //全局禁止响应
            ViewSvc.Instance.NoAllResponse();
            SceneLoadBeforeInit();
            if (PersistentDataSvc.Instance.versionInfo.sceneProgress)
            {
                //开始更新异步加载场景进度
                StartUpdateSceneLoadProgress();
            }

            StartCoroutine(LoadAsyncScene(sceneName));
            StartAsyncSceneLoadTimeTask();
        }

        /// <summary>
        /// 开始异步加载场景任务
        /// </summary>
        private void StartAsyncSceneLoadTimeTask()
        {
            _sceneLoadTimeTask = TimeSvc.Instance.AddTimeTask(() =>
            {
                if (_asyncOperation != null && _asyncOperation.progress >= 0.9f)
                {
                    _asyncOperation.allowSceneActivation = true;
                    _sceneLoadOverTimeTask = TimeSvc.Instance.AddTimeTask(() =>
                    {
                        if (_asyncOperation.isDone)
                        {
                            TimeSvc.Instance.DeleteTimeTask(_sceneLoadOverTimeTask);
                            if (PersistentDataSvc.Instance.versionInfo.sceneProgress)
                            {
                                ViewSvc.Instance.HideView(typeof(ExcessiveScene));
                                TimeSvc.Instance.DeleteTimeTask(_asyncSceneLoadProgressTimeTask);
                            }
                        }
                    }, "延迟加载视图", 0.001f, 0);
                    TimeSvc.Instance.DeleteTimeTask(_sceneLoadTimeTask);
                }
            }, "加载场景", 0.01f, 0);
        }

        /// <summary>
        /// 加载场景协程
        /// </summary>
        /// <param name="sceneIndex">跳转场景的索引</param>
        /// <returns></returns>
        IEnumerator LoadAsyncScene(int sceneIndex)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            _asyncOperation.allowSceneActivation = false;
            yield return _asyncOperation;
        }

        /// <summary>
        /// 加载场景协程
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadAsyncScene(string sceneName)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            _asyncOperation.allowSceneActivation = false;
            yield return _asyncOperation;
        }

        /// <summary>
        /// 开始更新异步加载场景进度
        /// </summary>
        private void StartUpdateSceneLoadProgress()
        {
            ViewSvc.Instance.ShowView(typeof(ExcessiveScene));
            _asyncSceneLoadProgressTimeTask = TimeSvc.Instance.AddTimeTask(() =>
            {
                if (_asyncOperation != null)
                {
                    if (_asyncOperation.progress >= 0.9f)
                    {
                        ExcessiveScene.Instance.UpdateAsyncLoadProgress(1);
                    }
                    else
                    {
                        ExcessiveScene.Instance.UpdateAsyncLoadProgress(_asyncOperation.progress);
                    }
                }
            }, "更新异步加载场景进度", 0.01f, 0);
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
                // Application.ExternalCall("close", "close");
                Close();
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.Log("Quit");
            }
        }

        public override void InitSvc()
        {
        }

        [DllImport("__Internal")]
        private static extern void Close();
    }
}