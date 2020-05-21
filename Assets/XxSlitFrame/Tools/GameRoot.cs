using System;
using CameraTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools
{
    /// <summary>
    /// 逻辑的根路径
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        public void GameRootInit()
        {
            DontDestroyOnLoad(this);
#if UNITY_EDITOR
            ResSvc.Instance.StartDownProjectConfig();
#endif
            //跟新场景信息
            SceneSvc.Instance.UpdateSceneNameOrIndex();
            //开启场景实时检测
            TimeSvc.Instance.AddImmortalTimeTask(() =>
            {
                switch (PersistentDataSvc.Instance.sceneLoadType)
                {
                    case SceneLoadType.Normal:
                        break;
                    //同步
                    case SceneLoadType.SceneName:
                        //新场景了
                        if (PersistentDataSvc.Instance.sceneName != SceneManager.GetActiveScene().name)
                        {
                            SceneSvc.Instance.InitSceneData();
                        }

                        break;
                    //异步
                    case SceneLoadType.SceneIndex:
                        //新场景了
                        if (PersistentDataSvc.Instance.sceneIndex != SceneManager.GetActiveScene().buildIndex)
                        {
                            SceneSvc.Instance.InitSceneData();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }, "开启场景实时检测", 0.01f, 0);

            //开启场景实时检测
            TimeSvc.Instance.AddImmortalTimeTask(() =>
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    CameraControl.Instance.SetCurrentCameraPosInfo();
                }
            }, "开启相机位置信息实时记录", 0.01f, 0);
            if (PersistentDataSvc.Instance.jump)
            {
                SceneSvc.Instance.UpdateSceneNameOrIndex();
                SceneSvc.Instance.SceneLoad(PersistentDataSvc.Instance.jumpSceneName);
                Destroy(GetComponent<AudioListener>());
            }
            else
            {
                PersistentDataSvc.Instance.sceneLoadType = SceneLoadType.SceneName;
                SceneSvc.Instance.UpdateSceneNameOrIndex();
            }
        }
    }
}