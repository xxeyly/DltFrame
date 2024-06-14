using Sirenix.OdinInspector;
using UnityEngine;
namespace DltFramework
{
    /// <summary>
    /// 框架基类
    /// </summary>
    public abstract class FrameComponent : MonoBehaviour, IFrameComponent
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