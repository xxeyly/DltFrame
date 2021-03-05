using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 场景服务--用于场景的加载
    /// </summary>
    public class SceneSvc : SvcBase
    {
        public static SceneSvc Instance;

        private AsyncOperation _asyncOperation;
        private int _sceneLoadTimeTask;
        private int _sceneLoadOverTimeTask;
        private int _asyncSceneLoadProgressTimeTask;

        public override void StartSvc()
        {
            Instance = GetComponent<SceneSvc>();
        }

        public override void InitSvc()
        {
        }

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
            ViewSvc.Instance.AllViewDestroy();
            //删除所有计时任务
            TimeSvc.Instance.DeleteSwitchTask();
            TimeSvc.Instance.DeleteTimeTask();
            //音频提示播放
            AudioSvc.Instance.StopEffectAudio();
            AudioSvc.Instance.StopTipAndDialogAudio();
        }

        /// <summary>
        /// 初始化场景数据
        /// </summary>
        public void InitSceneData()
        {
            UpdateSceneNameOrIndex();
            ListenerSvc.Instance.InitSvc();
            InitSceneStartSingletons();
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
            ViewSvc.Instance.InitSvc();
            ViewSvc.Instance.FrozenInit();
        }

        /// <summary>
        /// 更新场景名字或索引
        /// </summary>
        public void UpdateSceneNameOrIndex()
        {
            PersistentDataSvc.Instance.sceneName = SceneManager.GetActiveScene().name;
            PersistentDataSvc.Instance.sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                Debug.Log("Quit");
            }
        }


        [DllImport("__Internal")]
        private static extern void Close();
    }
}