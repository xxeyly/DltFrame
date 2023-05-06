using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    [InfoBox("框架开始")]
    public class GameRootStart : MonoBehaviour
    {
        public static GameRootStart Instance;
#pragma warning disable 649
        [LabelText("框架组件")] [Searchable] public List<FrameComponent> frameComponent = new List<FrameComponent>();
        [LabelText("场景组件")] [Searchable] public List<SceneComponent> sceneComponents = new List<SceneComponent>();
        [LabelText("场景初始化组件")] [Searchable] public List<SceneComponentInit> sceneInitStartSingletons = new List<SceneComponentInit>();
        [LabelText("禁止摧毁")] [BoxGroup] public bool dontDestroyOnLoad;

        private void OnEnable()
        {
            //场景中只有一个GameRootStart
            if (DataFrameComponent.GetAllObjectsInScene<GameRootStart>().Count == 1)
            {
                if (DataFrameComponent.GetAllObjectsInScene<GameRootStart>()[0].dontDestroyOnLoad)
                {
                    return;
                }
            }
            //多余GameRootStart销毁
            else
            {
                foreach (GameRootStart gameRootStart in DataFrameComponent.GetAllObjectsInScene<GameRootStart>())
                {
                    if (!gameRootStart.dontDestroyOnLoad)
                    {
                        Destroy(gameRootStart.gameObject);
                    }
                }

                return;
            }


            DontDestroyOnLoad(this);
            Instance = GetComponent<GameRootStart>();

            List<FrameComponent> frameComponents = DataFrameComponent.GetAllObjectsInScene<FrameComponent>();
            foreach (Type type in General.frameComponentType)
            {
                foreach (FrameComponent component in frameComponents)
                {
                    if (component.GetType() == type)
                    {
                        frameComponent.Add(component);
                        break;
                    }
                }
            }

            FrameComponentStart();
            SceneManager.sceneLoaded += SceneLoadOverCallBack;
            dontDestroyOnLoad = true;
            //框架组件开启
            if (RuntimeDataFrameComponent.Instance.jump)
            {
                Debug.Log("初始场景跳转");
                SceneLoadFrameComponent.Instance.SceneLoad(RuntimeDataFrameComponent.Instance.jumpSceneName);
                Destroy(GetComponent<AudioListener>());
            }
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

        /// <summary>
        /// 加载场景初始化单例
        /// 加载顺序 框架组件-场景工具-View静态界面
        /// </summary>
        private void InitSceneStartSingletons()
        {
            Debug.Log("框架初始化");
            FrameComponentSceneInit();
            Debug.Log("场景初始化");
            SceneComponentStart();
            SceneComponentInitStart();
            Debug.Log(SceneManager.GetActiveScene().name + ":" + "场景初始化完毕");
        }


        //场景加载前准备
        public void SceneBeforeLoadPrepare()
        {
            SceneComponentEnd();
            ViewFrameComponent.Instance.AllViewDestroy();
            TimeFrameComponent.Instance.FrameSceneInitComponent();
            ListenerFrameComponent.Instance.FrameSceneInitComponent();
        }


        private void OnDestroy()
        {
            if (dontDestroyOnLoad)
            {
                foreach (FrameComponent componentBase in frameComponent)
                {
                    componentBase.FrameEndComponent();
                }

                SceneComponentEnd();
            }
        }


        [LabelText("开启框架组件")]
        private void FrameComponentStart()
        {
            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].FrameInitComponent();
            }
        }

        [LabelText("框架组件场景初始化")]
        private void FrameComponentSceneInit()
        {
            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].FrameSceneInitComponent();
            }
        }

        [LabelText("开启场景组件")]
        private void SceneComponentStart()
        {
            sceneComponents = DataFrameComponent.GetAllObjectsInScene<SceneComponent>();
            for (int i = 0; i < sceneComponents.Count; i++)
            {
                sceneComponents[i].StartComponent();
            }
        }

        [LabelText("开启场景初始化组件")]
        private void SceneComponentInitStart()
        {
            sceneInitStartSingletons = DataFrameComponent.GetAllObjectsInScene<SceneComponentInit>();
            for (int i = 0; i < sceneInitStartSingletons.Count; i++)
            {
                sceneInitStartSingletons[i].InitComponent();
            }
        }

        //场景组件结束
        [LabelText("结束场景组件")]
        public void SceneComponentEnd()
        {
            for (int i = 0; i < sceneComponents.Count; i++)
            {
                sceneComponents[i].EndComponent();
            }
        }
    }
}