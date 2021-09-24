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
        [LabelText("场景服务")] [Searchable] public List<StartSingleton> sceneStartSingletons;
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
                gameRoot.GameRootInit(PersistentDataSvc.Instance.jump);
            }
        }

        private void OnDestroy()
        {
            foreach (SvcBase svcBase in activeSvcBase)
            {
                svcBase.EndSvc();
            }
        }

        protected void Awake()
        {
        }

        private void SvcStart()
        {
            foreach (SvcBase svcBase in activeSvcBase)
            {
                svcBase.StartSvc();
            }
        }

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
    }
}