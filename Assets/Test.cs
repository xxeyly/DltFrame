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
    private Toggle _tog;
    private Button _login;
    private Button _register;
    //变量声明结束
    public override void Init()
    {
    }

    protected override void InitView()
    {
        //变量查找开始
        BindUi(ref _image,"Image");
        BindUi(ref _tog,"Tog");
        BindUi(ref _login,"Login");
        BindUi(ref _register,"Register");
        //变量查找结束
    }

    protected override void InitListener()
    {
        //变量绑定开始
        _tog.onValueChanged.AddListener(OnTog);
        BindListener(_login,EventTriggerType.PointerClick,OnLoginClick);
        BindListener(_register,EventTriggerType.PointerClick,OnRegisterClick);
        //变量绑定结束
    }

    //变量方法开始
    private void OnTog(bool isOn)
    {
    }
    private void OnLoginClick(BaseEventData targetObj)
    {
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
        Debug.Log("1");
    }
    private void OnRegisterClick(BaseEventData targetObj)
    {
        Debug.Log("注册");
    }
    //变量方法结束
}










