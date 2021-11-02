using UnityEngine;
namespace XFramework
{
    /// <summary>
    /// 逻辑的根路径
    /// </summary>
    public class GameRoot : MonoBehaviour
    {
        public void GameRootInit(bool dontDestroyOnLoad)
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }

            if (RuntimeDataSvc.Instance.jump)
            {
                Debug.Log("初始场景跳转");
                SceneSvc.Instance.SceneLoad(RuntimeDataSvc.Instance.jumpSceneName);
                Destroy(GetComponent<AudioListener>());
            }
        }
        
    }
}