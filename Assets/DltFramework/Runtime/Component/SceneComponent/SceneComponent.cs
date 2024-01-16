using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
#if UNITY_EDITOR
    public abstract partial class SceneComponent : SerializedMonoBehaviour, ISceneComponent
#else
        public abstract partial class SceneComponent : MonoBehaviour, ISceneComponent
#endif
    {
        public abstract void StartComponent();
        public abstract void EndComponent();

        public void AddToSceneComponent()
        {
            if (GameRootStart.Instance.sceneComponents.Contains(this))
            {
                return;
            }

            GameRootStart.Instance.sceneComponents.Add(this);
            StartComponent();
        }
    }
}