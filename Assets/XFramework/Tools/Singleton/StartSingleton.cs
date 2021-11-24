using UnityEngine;

namespace XFramework
{
    public abstract partial class StartSingleton : MonoBehaviour, IStartSingleton
    {
        public abstract void StartSvc();
        public abstract void Init();
    }
}