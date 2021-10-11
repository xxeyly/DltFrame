using UnityEngine.UI;

//引入开始

//引入结束
namespace XFramework
{
    public class LoaderScene : BaseWindow
    {
        //变量声明开始
        private Slider _barSlider;
        private Text _title;

        private Text _loadingText;

        //变量声明结束
        public override void Init()
        {
        }

        protected override void InitView()
        {
            //变量查找开始
            BindUi(ref _barSlider, "BarSlider");
            BindUi(ref _title, "BarSlider/Title");
            BindUi(ref _loadingText, "BarSlider/LoadingText");
            //变量查找结束
        }

        protected override void InitListener()
        {
            //变量绑定开始

            //变量绑定结束
        }

        //变量方法开始

        //变量方法结束
    }
}