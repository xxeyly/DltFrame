using UnityEngine;

namespace XFramework
{
    public abstract class StartSingleton : MonoBehaviour, IStartSingleton
    {
        public abstract void StartSvc();
        public abstract void Init();
    }
}