namespace DltFramework
{
    /// <summary>
    /// 框架基类接口
    /// </summary>
    public interface IFrameComponent
    {
        /// <summary>
        /// 框架初始化
        /// </summary>
        void FrameInitComponent();

        /// <summary>
        /// 框架场景初始化
        /// </summary>
        void FrameSceneInitComponent();
    }
}