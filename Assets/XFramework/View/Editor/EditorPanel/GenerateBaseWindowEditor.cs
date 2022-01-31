#if UNITY_EDITOR

using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XFramework
{
    public class GenerateBaseWindowEditor : BaseEditor
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

        private GenerateBaseWindowData _generateBaseWindowData;

        public override void OnDisable()
        {
            OnSaveConfig();
        }

        public override void OnCreateConfig()
        {
            _generateBaseWindowData =
                AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(General.generateBaseWindowPath);
            if (_generateBaseWindowData == null)
            {
                if (!Directory.Exists(General.assetRootPath))
                {
                    Directory.CreateDirectory(General.assetRootPath);
                }

                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GenerateBaseWindowData>(), General.generateBaseWindowPath);
                //读取数据
                _generateBaseWindowData = AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(General.generateBaseWindowPath);
            }
        }

        public override void OnSaveConfig()
        {
            _generateBaseWindowData.startUsing = startUsing;
            _generateBaseWindowData.endUsing = endUsing;
            _generateBaseWindowData.startUiVariable = startUiVariable;
            _generateBaseWindowData.endUiVariable = endUiVariable;
            _generateBaseWindowData.startVariableBindPath = startVariableBindPath;
            _generateBaseWindowData.endVariableBindPath = endVariableBindPath;
            _generateBaseWindowData.startVariableBindListener = startVariableBindListener;
            _generateBaseWindowData.endVariableBindListener = endVariableBindListener;
            _generateBaseWindowData.startVariableBindEvent = startVariableBindEvent;
            _generateBaseWindowData.endVariableBindEvent = endVariableBindEvent;
            _generateBaseWindowData.startCustomAttributesStart = startCustomAttributesStart;
            _generateBaseWindowData.endCustomAttributesStart = endCustomAttributesStart;
            //标记脏区
            EditorUtility.SetDirty(_generateBaseWindowData);
        }

        public override void OnLoadConfig()
        {
            startUsing = _generateBaseWindowData.startUsing;
            endUsing = _generateBaseWindowData.endUsing;
            startUiVariable = _generateBaseWindowData.startUiVariable;
            endUiVariable = _generateBaseWindowData.endUiVariable;
            startVariableBindPath = _generateBaseWindowData.startVariableBindPath;
            endVariableBindPath = _generateBaseWindowData.endVariableBindPath;
            startVariableBindListener = _generateBaseWindowData.startVariableBindListener;
            endVariableBindListener = _generateBaseWindowData.endVariableBindListener;
            startVariableBindEvent = _generateBaseWindowData.startVariableBindEvent;
            endVariableBindEvent = _generateBaseWindowData.endVariableBindEvent;
            startCustomAttributesStart = _generateBaseWindowData.startCustomAttributesStart;
            endCustomAttributesStart = _generateBaseWindowData.endCustomAttributesStart;
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}
#endif
