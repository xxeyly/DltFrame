using UnityEngine;

namespace DltFramework
{
    public abstract partial class SceneComponent : MonoBehaviour, ISceneComponent
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