using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DltFramework
{
    [InfoBox("框架开始")]
    public class GameRootStart : MonoBehaviour
    {
        public static GameRootStart Instance;
#pragma warning disable 649
        [LabelText("框架组件")] [Searchable] public List<FrameComponent> frameComponent = new List<FrameComponent>();
        [BoxGroup("场景加载")] [LabelText("跳转场景")] public bool initJump;

        [BoxGroup("场景加载")] [LabelText("初始化跳转场景名称")]
        public string initJumpSceneName;

        [BoxGroup("场景加载")] [LabelText("场景")] [HideInInspector]
        public Scene loadScene;

        [BoxGroup("卸载加载")] [LabelText("场景")] [HideInInspector]
        public Scene unScene;

        [BoxGroup("框架场景组件-不摧毁")] [LabelText("框架场景组件")] [Searchable]
        public List<SceneComponent> dontDestroyFrameSceneComponents = new List<SceneComponent>();

        [BoxGroup("框架场景组件-不摧毁")] [LabelText("框架场景初始化组件")] [Searchable]
        public List<SceneComponentInit> frameSceneInitStartSingletons = new List<SceneComponentInit>();

        [BoxGroup("实时场景组件")] [LabelText("场景组件")] [Searchable]
        public List<SceneComponent> sceneComponents = new List<SceneComponent>();

        [BoxGroup("实时场景组件")] [LabelText("场景初始化组件")] [Searchable]
        public List<SceneComponentInit> sceneInitStartSingletons = new List<SceneComponentInit>();

        [LabelText("热更加载")] [BoxGroup] public bool hotFixLoad;
        [LabelText("禁止摧毁")] [BoxGroup] public bool dontDestroyOnLoad;


        [LabelText("框架加载日志")] [BoxGroup] public bool frameLoadLog;


        private async void OnEnable()
        {
            //场景中只有一个GameRootStart
            if (DataFrameComponent.Hierarchy_GetAllObjectsInScene<GameRootStart>().Count == 1)
            {
                if (DataFrameComponent.Hierarchy_GetAllObjectsInScene<GameRootStart>()[0].dontDestroyOnLoad)
                {
                    return;
                }
            }
            //多余GameRootStart销毁
            else
            {
                foreach (GameRootStart gameRootStart in DataFrameComponent.Hierarchy_GetAllObjectsInScene<GameRootStart>())
                {
                    if (!gameRootStart.dontDestroyOnLoad)
                    {
                        DestroyImmediate(gameRootStart.gameObject);
                    }
                }

                return;
            }

            DontDestroyOnLoad(this.gameObject);
            dontDestroyOnLoad = true;
            Instance = GetComponent<GameRootStart>();
            Debug.Log("框架初始化");
            frameComponent = DataFrameComponent.Hierarchy_GetAllObjectsInScene<FrameComponent>("DontDestroyOnLoad");

            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].SetFrameInitIndex();
            }

            //frameComponent冒泡排序
            //定义总和值
            for (int i = 0; i < frameComponent.Count - 1; i++)
                //需要比较的次数，即减去i本身
            {
                for (int j = 0; j < frameComponent.Count - 1 - i; j++)
                    //比较的次数，即减去第一个i本身以及比较过的次数i
                {
                    if (frameComponent[j].frameInitIndex > frameComponent[j + 1].frameInitIndex)
                    {
                        (frameComponent[j], frameComponent[j + 1]) = (frameComponent[j + 1], frameComponent[j]);
                        //交换元素位置
                    }
                }
            }


            for (int i = 0; i < frameComponent.Count; i++)
            {
                frameComponent[i].FrameInitComponent();
            }

            Debug.Log("框架初始化完毕");
            //框架组件开启
            dontDestroyFrameSceneComponents = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponent>("DontDestroyOnLoad");
            if (dontDestroyFrameSceneComponents.Count > 0)
            {
                Debug.Log("不摧毁的SceneComponent加载完毕");

                for (int i = 0; i < dontDestroyFrameSceneComponents.Count; i++)
                {
                    dontDestroyFrameSceneComponents[i].StartComponent();
                }
            }

            if (frameSceneInitStartSingletons.Count > 0)
            {
                frameSceneInitStartSingletons = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponentInit>("DontDestroyOnLoad");
                for (int i = 0; i < frameSceneInitStartSingletons.Count; i++)
                {
                    frameSceneInitStartSingletons[i].InitComponent();
                }

                Debug.Log("不摧毁的SceneComponentInit加载完毕");
            }

            if (initJump)
            {
                Debug.Log("初始场景跳转");
                await SceneLoadFrameComponent.Instance.SceneLoad(initJumpSceneName);
                DestroyImmediate(GetComponent<AudioListener>());
            }

            SceneManager.sceneLoaded += SceneLoadOverCallBack;
        }

        /// <summary>
        /// 场景加载完毕回调
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneType"></param>
        private void SceneLoadOverCallBack(Scene scene, LoadSceneMode sceneType)
        {
            if (scene.name == unScene.name)
            {
                unScene = new Scene();
                return;
            }

            loadScene = scene;
            InitSceneStartSingletons(scene);
        }

        /// <summary>
        /// 加载场景初始化单例
        /// 加载顺序 框架组件-场景工具
        /// </summary>
        private void InitSceneStartSingletons(Scene scene)
        {
            Debug.Log(scene.name + "场景加载完毕");
            FrameComponentSceneInit();
            // Debug.Log(scene.name + "框架场景初始化");
            SceneComponentStart(scene);
            SceneComponentInitStart(scene);
            // Debug.Log(scene.name + ":" + "场景初始化完毕");
        }


        //旧场景摧毁
        public void OldSceneDestroy(string destroySceneName)
        {
            List<SceneComponent> destroySceneComponent = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponent>(destroySceneName);
            //场景组件结束
            foreach (SceneComponent sceneComponent in destroySceneComponent)
            {
                sceneComponent.EndComponent();
            }

            //场景组件移除
            foreach (SceneComponent sceneComponent in destroySceneComponent)
            {
                if (sceneComponents.Contains(sceneComponent))
                {
                    sceneComponents.Remove(sceneComponent);
                }
            }

            //场景初始化组件移除
            List<SceneComponentInit> destroySceneComponentInit = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponentInit>(destroySceneName);
            foreach (SceneComponentInit sceneComponentInit in destroySceneComponentInit)
            {
                if (sceneInitStartSingletons.Contains(sceneComponentInit))
                {
                    sceneInitStartSingletons.Remove(sceneComponentInit);
                }
            }

            //框架组件场景结束
            foreach (FrameComponent component in frameComponent)
            {
                component.FrameSceneEndComponent();
            }
        }

        private void OnApplicationQuit()
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
        private void FrameComponentInit()
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
        public void SceneComponentStart(Scene scene)
        {
            sceneComponents = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponent>(scene.name);
            for (int i = 0; i < sceneComponents.Count; i++)
            {
                sceneComponents[i].StartComponent();
            }
        }

        [LabelText("开启场景初始化组件")]
        public void SceneComponentInitStart(Scene scene)
        {
            sceneInitStartSingletons = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponentInit>(scene.name);
            for (int i = 0; i < sceneInitStartSingletons.Count; i++)
            {
                sceneInitStartSingletons[i].InitComponent();
            }
        }

        //场景组件结束
        [LabelText("结束场景组件")]
        private void SceneComponentEnd()
        {
            List<SceneComponent> tempSceneComponent = DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponent>();
            for (int i = 0; i < tempSceneComponent.Count; i++)
            {
                tempSceneComponent[i].EndComponent();
            }
        }
    }
}