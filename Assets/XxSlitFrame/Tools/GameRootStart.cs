using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.Tools.Svc.BaseSvc;
using Object = UnityEngine.Object;

namespace XxSlitFrame.Tools
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
                //服务初始化
                SvcInit();
                Debug.Log("服务开启");
                GameRoot gameRoot = gameObject.AddComponent<GameRoot>();
                gameRoot.GameRootInit(dontDestroyOnLoad);
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