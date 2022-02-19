namespace XFramework
{
    public interface IComponent
    {
        /// <summary>
        /// 开启组件
        /// </summary>
        void FrameInitComponent();

        /// <summary>
        /// 组件初始化
        /// </summary>
        void SceneInitComponent();
    }
}