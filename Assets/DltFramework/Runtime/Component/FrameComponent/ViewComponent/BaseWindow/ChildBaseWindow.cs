#define SFRAMEWORK
using Sirenix.OdinInspector;

namespace DltFramework
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

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="itemIndexValue"></param>
        public virtual void InitData(int itemIndexValue)
        {
            this.itemIndex = itemIndexValue;
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="itemIndexValue"></param>
        /// <param name="content"></param>
        public virtual void InitData(int itemIndexValue, string content)
        {
            this.itemIndex = itemIndexValue;
        }
    }
}