using UnityEngine;

namespace XFramework
{
    public abstract partial class SceneComponent : MonoBehaviour, ISceneComponent
    {
        public abstract void StartSvc();
        public abstract void Init();
    }
}