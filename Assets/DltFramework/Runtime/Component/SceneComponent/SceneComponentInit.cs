using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [RequireComponent(typeof(HotFixAssetPathConfig))]
    public abstract partial class SceneComponentInit : ExtendMonoBehaviour, ISceneComponent
    {
        [GUIColor(0.3f, 0.8f, 0.8f)] [LabelText("视图名称")] [LabelWidth(50)]
        public string viewName;

        public virtual void StartComponent()
        {
        }

        public abstract void InitComponent();
    }
}