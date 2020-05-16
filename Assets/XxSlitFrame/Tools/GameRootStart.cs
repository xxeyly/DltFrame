using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools
{
    /// <summary>
    /// GameRoot开始
    /// </summary>
    public class GameRootStart : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject gameRootCanvas;
        public List<SvcBase> activeSvcBase;

        private void Start()
        {
            //如果场景中有GameRoot,摧毁当前物体
            if (FindObjectOfType<GameRoot>())
            {
                Destroy(gameObject);
            }
            else
            {
                if (gameRootCanvas != null)
                {
                    //服务开启
                    SvcStart();
                    //服务初始化
                    SvcInit();
                    Debug.Log("服务开启");
                    //开启
                    GameObject cloneGameRootCanvas = Instantiate(this.gameRootCanvas, transform, true);
                    cloneGameRootCanvas.transform.localPosition = Vector3.zero;
                    cloneGameRootCanvas.transform.localScale = Vector3.one;
                    cloneGameRootCanvas.name = "RootCanvas(" + SceneManager.GetActiveScene().name + ")";
                    GameRoot gameRoot = gameObject.AddComponent<GameRoot>();
                    gameRoot.GameRootInit();
                }
                else
                {
                    Debug.Log("_gameRootCanvas为空");
                }
            }
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
                svcBase.InitSvc();
            }
        }
    }
}