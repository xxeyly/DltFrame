using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public abstract class ComponentBase : MonoBehaviour, IComponent
    {
        [BoxGroup] [LabelText("框架初始化")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public bool frameInit;

        [BoxGroup] [LabelText("场景初始化")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public bool sceneInit;

        [BoxGroup] [LabelText("组件索引")] public int componentIndex;

        public abstract void StartComponent();


        public abstract void InitComponent();
        public abstract void EndComponent();
    }
}