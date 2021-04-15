using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel.OdinEditor
{
    public class GenerateBaseWindowEditor : BaseEditor
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
                //创建数据
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GenerateBaseWindowData>(),
                    General.generateBaseWindowPath);
                //读取数据
                _generateBaseWindowData =
                    AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(
                        General.generateBaseWindowPath);
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
            //标记脏区
            EditorUtility.SetDirty(_generateBaseWindowData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
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
        }

        public override void OnInit()
        {
            OnCreateConfig();
            OnLoadConfig();
        }
    }
}