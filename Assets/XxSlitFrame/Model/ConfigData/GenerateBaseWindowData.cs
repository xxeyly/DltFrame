using Sirenix.OdinInspector;
using UnityEngine;

namespace XxSlitFrame.Model.ConfigData
{
    public class GenerateBaseWindowData : ScriptableObject
    {
        [LabelText("Using开始")] public string startUsing;
        [LabelText("Using开始")] public string endUsing;

        [LabelText("变量声明开始")] public string startUiVariable;
        [LabelText("变量声明结束")] public string endUiVariable;


        [LabelText("变量位置绑定开始")] public string startVariableBindPath;
        [LabelText("变量位置绑定结束")] public string endVariableBindPath;


        [LabelText("变量事件绑定开始")] public string startVariableBindListener;
        [LabelText("变量事件绑定结束")] public string endVariableBindListener;

        [LabelText("变量方法开始")] public string startVariableBindEvent;
        [LabelText("变量方法结束")] public string endVariableBindEvent;
        
        [LabelText("自定义属性开始")] public string startCustomAttributesStart;
        [LabelText("自定义属性结束")] public string endCustomAttributesStart;
    }
}