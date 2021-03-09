using UnityEngine;
using XxSlitFrame.View;

//引入开始
using UnityEngine.UI;
using UnityEngine.EventSystems;
//引入结束
public class Test : BaseWindow
{
    //变量声明开始
    private Image _image;
    private Button _login;
    //变量声明结束
    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _image,"Image");
        BindUi(ref _login,"Login");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        BindListener(_login,EventTriggerType.PointerClick,OnLoginClick);
        BindListener(_login,EventTriggerType.PointerEnter,OnLoginEnter);
        BindListener(_login,EventTriggerType.PointerExit,OnLoginExit);
        BindListener(_login,EventTriggerType.PointerDown,OnLoginDown);
        BindListener(_login,EventTriggerType.PointerUp,OnLoginUp);
        BindListener(_login,EventTriggerType.Drag,OnLoginDrag);
        //变量绑定结束
    }

    //变量方法开始
    private void OnLoginClick(BaseEventData targetObj)
    {
        Debug.Log("点击");
    }
    private void OnLoginEnter(BaseEventData targetObj)
    {
        Debug.Log("进入");
    }
    private void OnLoginExit(BaseEventData targetObj)
    {
        Debug.Log("退出");
    }
    private void OnLoginDown(BaseEventData targetObj)
    {
        Debug.Log("按下");
    }
    private void OnLoginUp(BaseEventData targetObj)
    {
        Debug.Log("抬起");
    }
    private void OnLoginDrag(BaseEventData targetObj)
    {
        Debug.Log("拖拽");
    }
    //变量方法结束
}
