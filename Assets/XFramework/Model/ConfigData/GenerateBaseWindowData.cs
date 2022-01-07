using Sirenix.OdinInspector;
using UnityEngine;

namespace XFramework
{
    public class GenerateBaseWindowData : ScriptableObject
    {
        [LabelText("Using开始")] public string startUsing = "引入开始";
        [LabelText("Using开始")] public string endUsing = "引入结束";

        [LabelText("变量声明开始")] public string startUiVariable = "变量声明开始";
        [LabelText("变量声明结束")] public string endUiVariable = "变量声明结束";


        [LabelText("变量位置绑定开始")] public string startVariableBindPath = "变量查找开始";
        [LabelText("变量位置绑定结束")] public string endVariableBindPath = "变量查找结束";


        [LabelText("变量事件绑定开始")] public string startVariableBindListener = "变量绑定开始";
        [LabelText("变量事件绑定结束")] public string endVariableBindListener = "变量绑定结束";

        [LabelText("变量方法开始")] public string startVariableBindEvent = "变量方法开始";
        [LabelText("变量方法结束")] public string endVariableBindEvent = "变量方法结束";

        [LabelText("自定义属性开始")] public string startCustomAttributesStart = "自定义属性开始";
        [LabelText("自定义属性结束")] public string endCustomAttributesStart = "自定义属性结束";
        
    }
}