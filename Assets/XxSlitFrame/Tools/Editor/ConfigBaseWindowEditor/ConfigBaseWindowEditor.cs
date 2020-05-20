using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.View;

namespace XxSlitFrame.Tools.Editor.ConfigBaseWindowEditor
{
    [CustomEditor(typeof(AutoBindUIData))]
    public class ConfigBaseWindowEditor : UnityEditor.Editor
    {
        private AutoBindUIData _autoBindUiData;
        private bool _viewDeclarationUi;
        private bool _viewBindUi;
        private bool _viewBindListener;
        private bool _viewStatementListener;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _autoBindUiData = (AutoBindUIData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("显示界面"))
            {
                _autoBindUiData.ShowView();
            }

            if (GUILayout.Button("隐藏界面"))
            {
                _autoBindUiData.HideView();
            }

            if (GUILayout.Button("UI变量声明"))
            {
                _viewDeclarationUi = true;
                _viewBindUi = false;
                _viewBindListener = false;
                _viewStatementListener = false;
                _autoBindUiData.OneClickDeclarationUi();
            }

            if (GUILayout.Button("UI绑定位置"))
            {
                _viewDeclarationUi = false;
                _viewBindUi = true;
                _viewBindListener = false;
                _viewStatementListener = false;
                _autoBindUiData.OneClickBindUi();
            }

            if (GUILayout.Button("UI绑定事件"))
            {
                _viewDeclarationUi = false;
                _viewBindUi = false;
                _viewBindListener = true;
                _viewStatementListener = false;
                _autoBindUiData.OneClickBindListener();
            }

            if (GUILayout.Button("UI事件声明"))
            {
                _viewDeclarationUi = false;
                _viewBindUi = false;
                _viewBindListener = false;
                _viewStatementListener = true;
                _autoBindUiData.OneClickStatementListener();
            }

            EditorGUILayout.EndHorizontal();

            if (_viewDeclarationUi)
            {
                for (int i = 0; i < _autoBindUiData.allUiVariableName.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(new GUIStyle("ObjectPickerBackground"));
                    EditorGUILayout.LabelField(_autoBindUiData.allUiVariableName[i]);
                    if (GUILayout.Button("复制", GUILayout.MaxWidth(40)))
                    {
                        TextEditor textEditor = new TextEditor
                        {
                            text = _autoBindUiData.allUiVariableName[i]
                        };
                        textEditor.OnFocus();
                        textEditor.Copy();
                    }

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(40)))
                    {
                        _autoBindUiData.allUiVariableName.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("复制全部"))
                {
                    string currentContent = "";
                    for (int i = 0; i < _autoBindUiData.allUiVariableName.Count; i++)
                    {
                        currentContent += _autoBindUiData.allUiVariableName[i];
                    }

                    TextEditor textEditor = new TextEditor
                    {
                        text = currentContent
                    };
                    textEditor.OnFocus();
                    textEditor.Copy();
                }
            }

            if (_viewBindUi)
            {
                for (int i = 0; i < _autoBindUiData.allUiVariableBind.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(new GUIStyle("ObjectPickerBackground"));
                    EditorGUILayout.LabelField(_autoBindUiData.allUiVariableBind[i]);
                    if (GUILayout.Button("复制", GUILayout.MaxWidth(40)))
                    {
                        TextEditor textEditor = new TextEditor
                        {
                            text = _autoBindUiData.allUiVariableBind[i]
                        };
                        textEditor.OnFocus();
                        textEditor.Copy();
                    }

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(40)))
                    {
                        _autoBindUiData.allUiVariableBind.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("复制全部"))
                {
                    string currentContent = "";
                    for (int i = 0; i < _autoBindUiData.allUiVariableBind.Count; i++)
                    {
                        currentContent += _autoBindUiData.allUiVariableBind[i];
                    }

                    TextEditor textEditor = new TextEditor
                    {
                        text = currentContent
                    };
                    textEditor.OnFocus();
                    textEditor.Copy();
                }
            }

            if (_viewBindListener)
            {
                for (int i = 0; i < _autoBindUiData.allUiVariableBindListener.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(new GUIStyle("ObjectPickerBackground"));
                    EditorGUILayout.LabelField(_autoBindUiData.allUiVariableBindListener[i]);
                    if (GUILayout.Button("复制", GUILayout.MaxWidth(40)))
                    {
                        TextEditor textEditor = new TextEditor
                        {
                            text = _autoBindUiData.allUiVariableBindListener[i]
                        };
                        textEditor.OnFocus();
                        textEditor.Copy();
                    }

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(40)))
                    {
                        _autoBindUiData.allUiVariableBindListener.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("复制全部"))
                {
                    string currentContent = "";
                    for (int i = 0; i < _autoBindUiData.allUiVariableBindListener.Count; i++)
                    {
                        currentContent += _autoBindUiData.allUiVariableBindListener[i];
                    }

                    TextEditor textEditor = new TextEditor
                    {
                        text = currentContent
                    };
                    textEditor.OnFocus();
                    textEditor.Copy();
                }
            }

            if (_viewStatementListener)
            {
                for (int i = 0; i < _autoBindUiData.allUiVariableBindListenerEvent.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(new GUIStyle("ObjectPickerBackground"));
                    EditorGUILayout.LabelField(_autoBindUiData.allUiVariableBindListenerEvent[i]);
                    if (GUILayout.Button("复制", GUILayout.MaxWidth(40)))
                    {
                        TextEditor textEditor = new TextEditor
                        {
                            text = _autoBindUiData.allUiVariableBindListenerEvent[i]
                        };
                        textEditor.OnFocus();
                        textEditor.Copy();
                    }

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(40)))
                    {
                        _autoBindUiData.allUiVariableBindListenerEvent.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("复制全部"))
                {
                    string currentContent = "";
                    for (int i = 0; i < _autoBindUiData.allUiVariableBindListenerEvent.Count; i++)
                    {
                        currentContent += _autoBindUiData.allUiVariableBindListenerEvent[i];
                    }

                    TextEditor textEditor = new TextEditor
                    {
                        text = currentContent
                    };
                    textEditor.OnFocus();
                    textEditor.Copy();
                }
            }
        }
    }
}