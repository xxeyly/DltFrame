using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    /// <summary>
    /// GameRoot开始
    /// </summary>
    public class GameRootStart : MonoBehaviour
    {
        public static GameRootStart Instance;
#pragma warning disable 649
        [LabelText("激活的组件")] [Searchable] public List<ComponentBase> activeComponentBase;
        [LabelText("场景组件")] [Searchable] public List<SceneComponent> sceneStartSingletons;
        [LabelText("禁止摧毁")] [BoxGroup] public bool dontDestroyOnLoad;

        private void OnEnable()
        {
            //如果场景中有GameRoot,摧毁当前物体
            if (FindObjectOfType<GameRoot>())
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = GetComponent<GameRootStart>();
                dontDestroyOnLoad = true;
                //组件排序
                ComponentSort();
                //组件开启
                ComponentStart();
                Debug.Log("组件开启");
                //组件初始化
                ComponentInit();
                Debug.Log("框架组件加载完毕");
            }
        }

        /// <summary>
        /// 框架加载结束
        /// </summary>
        public void FrameSceneLoadEnd()
        {
            if (!GetComponent<GameRoot>())
            {
                GameRoot gameRoot = gameObject.AddComponent<GameRoot>();
                gameRoot.GameRootInit(dontDestroyOnLoad);
            }
        }

        private void OnDestroy()
        {
            if (dontDestroyOnLoad)
            {
                foreach (ComponentBase componentBase in activeComponentBase)
                {
                    componentBase.EndComponent();
                }
            }
        }

        protected void Awake()
        {
        }
      
        /// <summary>
        /// 组件排序
        /// </summary>
        private void ComponentSort()
        {
            for (int i = 0; i < activeComponentBase.Count; i++)
            {
                for (int j = 0; j < activeComponentBase.Count; j++)
                {
                    if (activeComponentBase[i].componentIndex <= activeComponentBase[j].componentIndex)
                    {
                        ComponentBase tempComponentBase = activeComponentBase[i];
                        activeComponentBase[i] = activeComponentBase[j];
                        activeComponentBase[j] = tempComponentBase;
                    }
                }
            }
        }

        /// <summary>
        /// 开启组件
        /// </summary>
        private void ComponentStart()
        {
            for (int i = 0; i < activeComponentBase.Count; i++)
            {
                activeComponentBase[i].FrameInitComponent();
            }
        }

        /// <summary>
        /// 组件初始化
        /// </summary>
        private void ComponentInit()
        {
            for (int i = 0; i < activeComponentBase.Count; i++)
            {
                if (activeComponentBase[i].frameInit)
                {
                    activeComponentBase[i].SceneInitComponent();
                }
            }
        }

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("ListenerComponent代码生成")]
        public void ListenerComponentGenerateData()
        {
            GetComponentInChildren<ListenerComponentGenerateData>()?.OnGenerate();
        }

        [LabelText("场景道具初始化")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            GetComponentInChildren<EntityComponent>()?.EntityInit();
        }
    }
}