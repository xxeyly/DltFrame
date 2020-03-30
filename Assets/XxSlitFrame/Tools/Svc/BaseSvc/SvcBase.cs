using UnityEngine;

namespace XxSlitFrame.Tools.Svc.BaseSvc
{
    public abstract class SvcBase<T> : MonoBehaviour
    {
        public static T Instance;

        /// <summary>
        /// 开启服务
        /// </summary>
        public virtual void StartSvc()
        {
            Instance = gameObject.GetComponent<T>();
        }

        public abstract void InitSvc();
    }
}