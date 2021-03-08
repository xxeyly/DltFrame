using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    public class BaseWindowGenerateScripts : MonoBehaviour
    {
        [TabGroup("UI", "引用")] [LabelText("UI变量引用")] [ReadOnly]
        public List<string> allUiVariableUsing;

        [TabGroup("UI", "名称")] [LabelText("UI变量名称")] [ReadOnly]
        public List<string> allUiVariableName;

        [TabGroup("UI", "查找")] [LabelText("UI变量绑定")] [ReadOnly]
        public List<string> allUiVariableBind;

        [TabGroup("UI", "绑定")] [LabelText("UI变量绑定事件")] [ReadOnly]
        public List<string> allUiVariableBindListener;

        [TabGroup("UI", "事件")] [LabelText("UI变量绑定事件声明")] [ReadOnly]
        public List<string> allUiVariableBindListenerEvent;

        private GenerateBaseWindowData _generateBaseWindowData;
        private CustomScriptableObject _customScriptableObject;
        private string _currentScriptsContent;
        [LabelText("分号")] private string _semicolon = ";";
        [LabelText("换行")] private string _lineFeed = "\n";

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("代码生成")]
        public void Generate()
        {
            //清除空格
            if (_customScriptableObject == null)
            {
                _customScriptableObject = new CustomScriptableObject();
            }

            if (_generateBaseWindowData == null)
            {
                _generateBaseWindowData =
                    AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(
                        _customScriptableObject.generateBaseWindowPath);
            }

            OneClickDeclarationUi();
            OneClickBindUi();
            OneClickBindListener();
            OneClickStatementListener();

            for (int i = 0; i < allUiVariableUsing.Count - 1; i++)
            {
                allUiVariableUsing[i] += _lineFeed;
            }

            for (int i = 0; i < allUiVariableName.Count - 1; i++)
            {
                allUiVariableName[i] += _lineFeed;
            }

            for (int i = 0; i < allUiVariableBind.Count - 1; i++)
            {
                allUiVariableBind[i] += _lineFeed;
            }

            for (int i = 0; i < allUiVariableBindListener.Count - 1; i++)
            {
                allUiVariableBindListener[i] += _lineFeed;
            }

            for (int i = 0; i < allUiVariableBindListenerEvent.Count - 1; i++)
            {
                allUiVariableBindListenerEvent[i] += _lineFeed;
            }

            _currentScriptsContent = GetOldScriptsContent();
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableUsing,
                "//" + _generateBaseWindowData.startUsing, 
                "//" + _generateBaseWindowData.endUsing);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableName,
                "//" + _generateBaseWindowData.startUiVariable, 
                "//" + _generateBaseWindowData.endUiVariable);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableBind,
                "//" + _generateBaseWindowData.startVariableBindPath,
                "//" + _generateBaseWindowData.endVariableBindPath);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableBindListener,
                "//" + _generateBaseWindowData.startVariableBindListener,
                "//" + _generateBaseWindowData.endVariableBindListener);
            _currentScriptsContent = ReplaceScriptContent(_currentScriptsContent, allUiVariableBindListenerEvent,
                "//" + _generateBaseWindowData.startVariableBindEvent,
                "//" + _generateBaseWindowData.endVariableBindEvent);
            ResSvc.FileOperation.SaveTextToLoad(GetScriptsPath(), _currentScriptsContent);
        }

        /// <summary>
        /// 缩进
        /// </summary>
        /// <returns></returns>
        private string Indents(int number)
        {
            string temp = String.Empty;
            for (int i = 0; i < number; i++)
            {
                temp += " ";
            }

            return temp;
        }

        /// <summary>
        /// 获得Window界面 LocalBaseWindow从跟目录开始索引
        /// </summary>
        /// <returns></returns>
        protected virtual Transform GetWindow()
        {
            return transform.Find("Window").transform;
        }

        /// <summary>
        /// 获得UI组件是否包含LocalBaseWindow路径
        /// </summary>
        /// <returns></returns>
        private bool GetUiComponentContainLocalBaseWindow(Transform uiTr)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != GetWindow().name)
            {
                hierarchy++;
                uiTr = uiTr.parent;
            }

            if (hierarchy == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i <= hierarchy; i++)
                {
                    if (GetParentByHierarchy(defaultUiTr, i).GetComponent<LocalBaseWindow>())
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        /// <summary>
        /// 根据UI层级获得父物体
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        private static Transform GetParentByHierarchy(Transform uiTr, int hierarchy)
        {
            for (int i = 0; i < hierarchy; i++)
            {
                uiTr = uiTr.parent;
            }

            return uiTr;
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        /// <returns></returns>
        private string GetUiComponentPath(Transform uiTr, string uiPath)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != GetWindow().name)
            {
                hierarchy++;
                uiTr = uiTr.parent;
            }

            for (int i = 1; i <= hierarchy; i++)
            {
                uiTr = GetParentByHierarchy(defaultUiTr, i);
                uiPath = uiTr.name + "/" + uiPath;
            }

            return uiPath + defaultUiTr.name;
        }

        /// <summary>
        /// 替换内容
        /// </summary>
        /// <param name="scriptsContent"></param>
        /// <param name="insertContent"></param>
        /// <param name="insertStartMark"></param>
        /// <param name="insertEndMark"></param>
        /// <returns></returns>
        private string ReplaceScriptContent(string scriptsContent, List<string> insertContent,
            string insertStartMark, string insertEndMark)
        {
            //开始位置 
            int usingStartIndex = scriptsContent.IndexOf(insertStartMark, StringComparison.Ordinal);
            //结束位置
            int usingEndIndex = scriptsContent.IndexOf(insertEndMark, StringComparison.Ordinal);
            //移除多余空格
            while (scriptsContent[usingEndIndex - 1] == ' ')
            {
                usingEndIndex -= 1;
            }

            //查找要被替换的内容
            string scriptUsingContent = String.Empty;
            for (int i = 0; i < scriptsContent.Length; i++)
            {
                if (i >= usingStartIndex && i < usingEndIndex)
                {
                    scriptUsingContent += scriptsContent[i];
                }
            }

            string tempInsertContent = String.Empty;
            for (int i = 0; i < insertContent.Count; i++)
            {
                tempInsertContent += insertContent[i];
            }

            tempInsertContent = insertStartMark + "\n" + tempInsertContent + "\n";
            //替换新内容
            return scriptsContent.Replace(scriptUsingContent, tempInsertContent);
        }

        /// <summary>
        /// 获得脚本路径
        /// </summary>
        /// <returns></returns>
        private string GetScriptsPath()
        {
            Type scriptType = GetComponent<BaseWindow>().GetType();
            string[] scriptNameSplit = scriptType.ToString().Split(new char[] {'.'});
            string scriptName = scriptNameSplit[scriptNameSplit.Length - 1];
            string scriptPath = GetPath(scriptName);
            return scriptPath;
        }

        /// <summary>
        /// 获得旧的脚本内容
        /// </summary>
        private string GetOldScriptsContent()
        {
            string oldScriptContent = ResSvc.FileOperation.GetTextToLoad(GetScriptsPath());
            return oldScriptContent;
        }

        /// <summary>
        /// 添加Using引用
        /// </summary>
        /// <param name="usingContent"></param>
        private void AddUsing(string usingContent)
        {
            if (!allUiVariableUsing.Contains(usingContent))
            {
                allUiVariableUsing.Add(usingContent);
            }
        }

        /// <summary>
        /// 获得脚本路径
        /// </summary>
        /// <param name="_scriptName"></param>
        /// <returns></returns>
        static string GetPath(string _scriptName)
        {
            string[] path = UnityEditor.AssetDatabase.FindAssets(_scriptName);

            for (int i = 0; i < path.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(path[i]).Contains("Assets") &&
                    AssetDatabase.GUIDToAssetPath(path[i]).Contains(_scriptName + ".cs"))
                {
                    return AssetDatabase.GUIDToAssetPath(path[i]);
                }
            }

            return null;
        }

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void OneClickDeclarationUi()
        {
            Transform window = GetWindow();
            allUiVariableName = new List<string>();
            allUiVariableUsing = new List<string>();
            //首行缩进
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    switch (child.GetComponent<BindUiType>().type)
                    {
                        case BindUiType.UiType.GameObject:

                            allUiVariableName.Add(Indents(4) + "private GameObject _" +
                                                  DataSvc.FirstCharToLower(child.name) + _semicolon);
                            AddUsing("using UnityEngine;");
                            break;
                        case BindUiType.UiType.Button:

                            allUiVariableName.Add(
                                Indents(4) + "private Button _" + DataSvc.FirstCharToLower(child.name) + _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.Image:
                            allUiVariableName.Add(Indents(4) + "private Image _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.Text:
                            allUiVariableName.Add(Indents(4) + "private Text _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon);
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.Toggle:
                            allUiVariableName.Add(
                                Indents(4) + "private Toggle _" + DataSvc.FirstCharToLower(child.name) +
                                _semicolon);
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.RawImage:
                            allUiVariableName.Add(Indents(4) + "private RawImage _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.Scrollbar:
                            allUiVariableName.Add(Indents(4) + "private Scrollbar _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.ScrollRect:
                            allUiVariableName.Add(Indents(4) + "private ScrollRect _" +
                                                  DataSvc.FirstCharToLower(child.name) + _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.InputField:

                            allUiVariableName.Add(Indents(4) + "private InputField _" +
                                                  DataSvc.FirstCharToLower(child.name) + _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.Dropdown:
                            allUiVariableName.Add(Indents(4) + "private Dropdown _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.Slider:
                            allUiVariableName.Add(Indents(4) + "private Slider _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;
                        case BindUiType.UiType.VideoPlayer:
                            allUiVariableName.Add(Indents(4) + "private VideoPlayer _" +
                                                  DataSvc.FirstCharToLower(child.name) +
                                                  _semicolon
                            );
                            AddUsing("using UnityEngine.UI;");
                            break;

                        case BindUiType.UiType.ChildList:

                            string childTypeName;
                            Type childType;
                            if (child.GetComponent<BindUiType>().childType != null)
                            {
                                childType = child.GetComponent<BindUiType>().childType.GetType();
                                if (childType == typeof(LocalUIBaseWindow))
                                {
                                    childTypeName = child.GetComponentInChildren<LocalUIBaseWindow>().uiType.ToString();
                                    AddUsing("using System.Collections.Generic;");
                                }
                                else
                                {
                                    childTypeName = child.GetComponent<BindUiType>().childType.GetType().ToString();
                                    AddUsing("using System.Collections.Generic;");
                                }

                                childTypeName = childTypeName.Split('.')[childTypeName.Split('.').Length - 1];

                                allUiVariableName.Add(Indents(4) + "private List<" + childTypeName + "> _" +
                                                      DataSvc.FirstCharToLower(child.name) + _semicolon);
                            }

                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        public void OneClickBindUi()
        {
            Transform window = GetWindow();
            allUiVariableBind = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    allUiVariableBind.Add(Indents(8) + "BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                          GetUiComponentPath(child, "") + "\");");
                }
            }
        }

        /// <summary>
        /// 一键绑定UI事件
        /// </summary>
        /// <returns></returns>
        public void OneClickBindListener()
        {
            Transform window = GetWindow();
            allUiVariableBindListener = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allUiVariableBindListener.Add(Indents(8) + "BindListener(_" +
                                                      DataSvc.FirstCharToLower(child.name) + "," +
                                                      "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                                      ");");
                        AddUsing("using UnityEngine.EventSystems;");
                    }
                    else
                    {
                        // allUiVariableBindListener.Add(string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        public void OneClickStatementListener()
        {
            Transform window = GetWindow();

            allUiVariableBindListenerEvent = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allUiVariableBindListenerEvent.Add(Indents(4) + "private void On" + child.name +
                                                           "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" +
                                                           "\n" + Indents(4) + "}");
                    }
                    else
                    {
                        // allUiVariableBindListenerEvent.Add(string.Empty);
                    }
                }
            }
        }
    }
}