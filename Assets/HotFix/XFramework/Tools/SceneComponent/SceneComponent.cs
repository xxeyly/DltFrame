using UnityEngine;

namespace XFramework
{
    public abstract partial class SceneComponent : MonoBehaviour, ISceneComponent
    {
        public abstract void StartComponent();
        public abstract void InitComponent();
        public abstract void EndComponent();
    }
}