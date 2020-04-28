using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XxSlitFrame.Tools
{
    /// <summary>
    /// GameRoot开始
    /// </summary>
    public class GameRootStart : MonoBehaviour
    {
#pragma warning disable 649
       [SerializeField]
        private GameObject gameRootCanvas;
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
    }
}