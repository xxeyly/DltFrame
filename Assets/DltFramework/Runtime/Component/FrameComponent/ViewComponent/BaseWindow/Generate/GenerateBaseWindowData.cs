using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public class GenerateBaseWindowData 
    {
        [LabelText("Using开始")] public static string startUsing = "#region 引入";
        [LabelText("Using开始")] public static string endUsing = "#endregion 引入";

        [LabelText("变量声明")] public static string startUiVariable = "#region 变量声明";
        [LabelText("变量声明")] public static string endUiVariable = "#endregion 变量声明";


        [LabelText("变量位置绑定开始")] public static string startVariableBindPath = "#region 变量查找";
        [LabelText("变量位置绑定结束")] public static string endVariableBindPath = "#endregion 变量查找";


        [LabelText("变量事件绑定开始")] public static string startVariableBindListener = "#region 变量绑定";
        [LabelText("变量事件绑定结束")] public static string endVariableBindListener = "#endregion 变量绑定";

        [LabelText("变量方法")] public static string startVariableBindEvent = "#region 变量方法";
        [LabelText("变量方法")] public static string endVariableBindEvent = "#endregion 变量方法";

        [LabelText("自定义属性开始")] public static string startCustomAttributesStart = "#region 自定义属性";
        [LabelText("自定义属性结束")] public static string endCustomAttributesStart = "#endregion 自定义属性";
    }
}