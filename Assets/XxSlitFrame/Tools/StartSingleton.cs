using UnityEngine;

namespace XxSlitFrame.Tools
{
    public class StartSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        /// <summary>
        /// 开启服务
        /// </summary>
        public virtual void StartSvc()
        {
            Instance = gameObject.GetComponent<T>();
            Init();
        }

        public virtual void Init()
        {
            
        }

     
    }
}