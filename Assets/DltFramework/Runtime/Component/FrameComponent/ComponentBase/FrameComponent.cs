using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace DltFramework
{
#if UNITY_EDITOR
    public abstract class FrameComponent : SerializedMonoBehaviour, IFrameComponent
#else
        public abstract class FrameComponent : MonoBehaviour, IFrameComponent
#endif
    {
        //框架组件初始化
        public abstract void FrameInitComponent();

        //框架组件结束
        public abstract void FrameEndComponent();

        //框架组件场景初始化
        public abstract void FrameSceneInitComponent();

        //框架组件场景结束
        public abstract void FrameSceneEndComponent();
    }
}