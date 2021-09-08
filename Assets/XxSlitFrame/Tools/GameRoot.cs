using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools
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