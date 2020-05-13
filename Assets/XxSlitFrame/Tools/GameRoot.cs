using System;
using CameraTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools
{
    /// <summary>
    /// 逻辑的根路径
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        private ResSvc _resSvc;

        private AudioSvc _audioSvc;

        private SceneSvc _sceneSvc;

        private PersistentDataSvc _persistentDataSvc;

        private TimeSvc _timeSvc;

        private ListenerSvc _listenerSvc;

        private ViewSvc _viewSvc;

        private MouseSvc _mouseSvc;


        public void GameRootInit()
        {
            _resSvc = GetComponent<ResSvc>();
            _audioSvc = GetComponent<AudioSvc>();
            _sceneSvc = GetComponent<SceneSvc>();
            _persistentDataSvc = GetComponent<PersistentDataSvc>();
            _timeSvc = GetComponent<TimeSvc>();
            _listenerSvc = GetComponent<ListenerSvc>();
            _viewSvc = GetComponent<ViewSvc>();
            _mouseSvc = GetComponent<MouseSvc>();

            _persistentDataSvc.StartSvc();
            _audioSvc.StartSvc();
            _resSvc.StartSvc();
            _sceneSvc.StartSvc();
            _timeSvc.StartSvc();
            _listenerSvc.StartSvc();
            _mouseSvc.StartSvc();
            _viewSvc.StartSvc();

            _persistentDataSvc.InitSvc();
            _listenerSvc.InitSvc();
            _audioSvc.InitSvc();
            _resSvc.InitSvc();
            _sceneSvc.InitSvc();
            _timeSvc.InitSvc();
            _mouseSvc.InitSvc();
            DontDestroyOnLoad(this);
            //下载配置文件
            _resSvc.StartDownProjectConfig();
            //开启场景实时检测
            _timeSvc.AddImmortalTimeTask(() =>
            {
                switch (_persistentDataSvc.sceneLoadType)
                {
                    case SceneLoadType.Normal:
                        break;
                    //同步
                    case SceneLoadType.SceneName:
                        //新场景了
                        if (_persistentDataSvc.sceneName != SceneManager.GetActiveScene().name)
                        {
                            _persistentDataSvc.sceneName = SceneManager.GetActiveScene().name;
                            _persistentDataSvc.sceneIndex = SceneManager.GetActiveScene().buildIndex;

                            _listenerSvc.InitSvc();
                            _viewSvc.InitSvc();
                        }

                        break;
                    //异步
                    case SceneLoadType.SceneIndex:
                        //新场景了
                        if (_persistentDataSvc.sceneIndex != SceneManager.GetActiveScene().buildIndex)
                        {
                            _persistentDataSvc.sceneName = SceneManager.GetActiveScene().name;
                            _persistentDataSvc.sceneIndex = SceneManager.GetActiveScene().buildIndex;
                            _listenerSvc.InitSvc();
                            _viewSvc.InitSvc();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }, "开启场景实时检测", 0.01f, 0);

            //开启场景实时检测
            _timeSvc.AddImmortalTimeTask(() =>
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    CameraControl.Instance.SetCurrentCameraPosInfo();
                }
            }, "开启相机位置信息实时记录", 0.01f, 0);
            if (_persistentDataSvc.jump)
            {
                _persistentDataSvc.sceneName = SceneManager.GetActiveScene().name;
                _persistentDataSvc.sceneIndex = SceneManager.GetActiveScene().buildIndex;
                _sceneSvc.SceneLoad(_persistentDataSvc.jumpSceneName);
                Destroy(GetComponent<AudioListener>());
            }
            else
            {
                _viewSvc.InitSvc();
                _persistentDataSvc.sceneLoadType = SceneLoadType.SceneName;
                _persistentDataSvc.sceneName = SceneManager.GetActiveScene().name;
                _persistentDataSvc.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            }
        }
    }
}