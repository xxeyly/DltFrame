using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [Tooltip("DltFramework Runtime")]
    public abstract partial class SceneComponent : ExtendMonoBehaviour, ISceneComponent
    {
        public abstract void StartComponent();
        public abstract void EndComponent();
        [LabelText("视图名称")] public string viewName;

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