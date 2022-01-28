namespace XFramework
{
    public interface IComponent
    {
        /// <summary>
        /// 开启组件
        /// </summary>
        void StartComponent();

        /// <summary>
        /// 组件初始化
        /// </summary>
        /// <param name="onTimeAddTimeTask"></param>
        void InitComponent();
    }
}