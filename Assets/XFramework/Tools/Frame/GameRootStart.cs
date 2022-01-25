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
        [LabelText("激活的服务")] [Searchable] public List<SvcBase> activeSvcBase;
        [LabelText("场景服务")] [Searchable] public List<SceneComponent> sceneStartSingletons;
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
                //服务排序
                SvcSort();
                //服务开启
                SvcStart();
                Debug.Log("服务开启");
                //服务初始化
                SvcInit();
                Debug.Log("框架服务加载完毕");
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
                foreach (SvcBase svcBase in activeSvcBase)
                {
                    svcBase.EndSvc();
                }
            }
        }

        protected void Awake()
        {
        }

        /// <summary>
        /// 服务排序
        /// </summary>
        private void SvcSort()
        {
            for (int i = 0; i < activeSvcBase.Count; i++)
            {
                for (int j = 0; j < activeSvcBase.Count; j++)
                {
                    if (activeSvcBase[i].svcIndex <= activeSvcBase[j].svcIndex)
                    {
                        SvcBase tempSvcBase = activeSvcBase[i];
                        activeSvcBase[i] = activeSvcBase[j];
                        activeSvcBase[j] = tempSvcBase;
                    }
                }
            }
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        private void SvcStart()
        {
            foreach (SvcBase svcBase in activeSvcBase)
            {
                svcBase.StartSvc();
            }
        }

        /// <summary>
        /// 服务初始化
        /// </summary>
        private void SvcInit()
        {
            foreach (SvcBase svcBase in activeSvcBase)
            {
                if (svcBase.frameInit)
                {
                    svcBase.InitSvc();
                }
            }
        }

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("ListenerSvc代码生成")]
        public void ListenerSvcGenerateData()
        {
            GetComponentInChildren<ListenerSvcGenerateData>()?.OnGenerate();
        }

        [LabelText("场景道具初始化")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        public void EntityInit()
        {
            GetComponentInChildren<EntitySvc>()?.EntityInit();
        }
    }
}