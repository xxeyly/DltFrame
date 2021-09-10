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

            if (PersistentDataSvc.Instance.jump)
            {
                SceneSvc.Instance.SceneLoad(PersistentDataSvc.Instance.jumpSceneName);
                Destroy(GetComponent<AudioListener>());
            }
        }
    }
}