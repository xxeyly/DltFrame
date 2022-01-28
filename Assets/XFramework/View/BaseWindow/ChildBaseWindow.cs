using Sirenix.OdinInspector;
namespace XFramework
{
    /// <summary>
    /// 局部UI界面
    /// </summary>
    public abstract class ChildBaseWindow : BaseWindow
    {
        [LabelText("索引")] public int itemIndex;
        public override void ViewStartInit()
        {
            window = gameObject;
            InitView();
            InitListener();
            OnlyOnceInit();
        }

        /// <summary>
        /// 选中
        /// </summary>
        public virtual void OnSelect()
        {
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        public virtual void OnUnSelect()
        {
        }
    }
}