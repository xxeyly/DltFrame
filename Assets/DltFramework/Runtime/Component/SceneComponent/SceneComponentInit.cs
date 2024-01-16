using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [RequireComponent(typeof(HotFixAssetPathConfig))]
#if UNITY_EDITOR
    public abstract partial class SceneComponentInit : SerializedMonoBehaviour, ISceneComponent
#else
        public abstract partial class SceneComponentInit : MonoBehaviour, ISceneComponent
#endif
    {
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("视图名称")] [LabelWidth(50)]
        public string viewName;

        public virtual void StartComponent()
        {
        }

        public abstract void InitComponent();
    }
}