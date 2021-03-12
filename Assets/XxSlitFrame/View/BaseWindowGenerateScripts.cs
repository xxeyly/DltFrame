using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
#if UNITY_EDITOR

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
        private string _currentScriptsContent;
        [LabelText("分号")] private string _semicolon = ";";
        [LabelText("换行")] private string _lineFeed = "\n";
        [LabelText("监听事件列表")] private Dictionary<string, string> _listenerActionList;

        [Button(ButtonSizes.Large)]
        [GUIColor(0, 1, 0)]
        [LabelText("代码生成")]
        public void Generate()
        {

            if (_generateBaseWindowData == null)
            {
#if UNITY_EDITOR

                _generateBaseWindowData =
                    AssetDatabase.LoadAssetAtPath<GenerateBaseWindowData>(
                        General.generateBaseWindowPath);
#endif
            }

            _listenerActionList = new Dictionary<string, string>();

            OneClickDeclarationUi();
            OneClickBindUi();
            OneClickBindListener();
            OneClickGetOldAction();
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
        protected bool GetUiComponentContainLocalBaseWindow(Transform uiTr)
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
                    if (GetParentByHierarchy(defaultUiTr, i).GetComponent<ChildBaseWindow>())
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
        protected virtual string GetScriptsPath()
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
        protected static string GetPath(string _scriptName)
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
                BindUiType bindUiType = child.GetComponent<BindUiType>();
                if (bindUiType && !GetUiComponentContainLocalBaseWindow(child))
                {
                    switch (bindUiType.type)
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
                            if (bindUiType.childType != null)
                            {
                                childType = bindUiType.childType.GetType();
                                if (childType == typeof(ChildUiBaseWindow))
                                {
                                    childTypeName = child.GetComponentInChildren<ChildUiBaseWindow>().uiType.ToString();
                                    AddUsing("using System.Collections.Generic;");
                                }
                                else
                                {
                                    childTypeName = bindUiType.childType.GetType().ToString();
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
                BindUiType bindUiType = child.GetComponent<BindUiType>();

                if (bindUiType && !GetUiComponentContainLocalBaseWindow(child))
                {
                    string bindStr = string.Empty;
                    if (bindUiType.type == BindUiType.UiType.Button)
                    {
                        BindUiType.UIEventTriggerType uiEventTriggerType =
                            child.GetComponent<BindUiType>().eventTriggerType;

                        if ((BindUiType.UIEventTriggerType.PointerClick & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerClick)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerClick" + "," + "On" + child.name + "Click" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerEnter & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerEnter)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerEnter" + "," + "On" + child.name + "Enter" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerExit & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerExit)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerExit" + "," + "On" + child.name + "Exit" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerDown & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerDown)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerDown" + "," + "On" + child.name + "Down" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerUp & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerUp)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.PointerUp" + "," + "On" + child.name + "Up" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.Drag & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.Drag)
                        {
                            bindStr = Indents(8) + "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                      "EventTriggerType.Drag" + "," + "On" + child.name + "Drag" + ");";
                            allUiVariableBindListener.Add(bindStr);
                        }

                        AddUsing("using UnityEngine.EventSystems;");
                    }
                    else if (bindUiType.type == BindUiType.UiType.Toggle)
                    {
                        bindStr = Indents(8) + "_" + DataSvc.FirstCharToLower(child.name) +
                                  ".onValueChanged.AddListener(" +
                                  "On" +
                                  child.name + ");";
                        allUiVariableBindListener.Add(bindStr);
                        AddUsing("using UnityEngine.EventSystems;");
                    }
                }
            }
        }


        public void OneClickGetOldAction()
        {
            string insertStartMark = "//" + _generateBaseWindowData.startVariableBindEvent;
            string insertEndMark = "//" + _generateBaseWindowData.endVariableBindEvent;
            string currentScript = GetOldScriptsContent();
            //开始位置 
            int usingStartIndex =
                currentScript.IndexOf(insertStartMark, StringComparison.Ordinal) + insertStartMark.Length;
            //结束位置
            int usingEndIndex = currentScript.IndexOf(insertEndMark, StringComparison.Ordinal);
            //移除多余空格
            while (currentScript[usingEndIndex - 1] == ' ')
            {
                usingEndIndex -= 1;
            }

            //查找要被替换的内容
            string scriptContent = String.Empty;
            for (int i = 0; i < currentScript.Length; i++)
            {
                if (i >= usingStartIndex && i < usingEndIndex)
                {
                    scriptContent += currentScript[i];
                }
            }

            List<string> actionNameList = new List<string>();

            Transform window = GetWindow();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                BindUiType bindUiType = child.GetComponent<BindUiType>();

                if (bindUiType && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (bindUiType.type == BindUiType.UiType.Button)
                    {
                        BindUiType.UIEventTriggerType uiEventTriggerType =
                            child.GetComponent<BindUiType>().eventTriggerType;

                        if ((BindUiType.UIEventTriggerType.PointerClick & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerClick)
                        {
                            actionNameList.Add(FindActionNameKey(
                                "On" + child.name + "Click", scriptContent));
                        }

                        if ((BindUiType.UIEventTriggerType.PointerEnter & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerEnter)
                        {
                            actionNameList.Add(FindActionNameKey(
                                "On" + child.name + "Enter", scriptContent));
                        }

                        if ((BindUiType.UIEventTriggerType.PointerExit & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerExit)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Exit",
                                    scriptContent));
                        }

                        if ((BindUiType.UIEventTriggerType.PointerDown & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerDown)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Down",
                                    scriptContent));
                        }

                        if ((BindUiType.UIEventTriggerType.PointerUp & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerUp)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Up",
                                    scriptContent));
                        }

                        if ((BindUiType.UIEventTriggerType.Drag & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.Drag)
                        {
                            actionNameList.Add(
                                FindActionNameKey("On" + child.name + "Drag",
                                    scriptContent));
                        }
                    }
                    else if (bindUiType.type == BindUiType.UiType.Toggle)
                    {
                        // actionNameList.Add("On" + child.name + "(bool isOn)" + "\n" + Indents(4) + "{");
                        actionNameList.Add(FindActionNameKey("On" + child.name + "(bool isOn)",
                            scriptContent));
                    }
                }
            }

            foreach (string actionName in actionNameList)
            {
                if (scriptContent.Contains(actionName))
                {
                    if (_listenerActionList.ContainsKey(actionName))
                    {
                        _listenerActionList[actionName] = FindActionContent(actionName, scriptContent);
                    }
                    else
                    {
                        _listenerActionList.Add(actionName, FindActionContent(actionName, scriptContent));
                    }
                }
            }
        }

        /// <summary>
        /// 查找方法关键Key
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private string FindActionNameKey(string actionName, string scriptsContent)
        {
            string actionContent = "";
            //开始位置 
            int startIndex =
                scriptsContent.IndexOf(actionName, StringComparison.Ordinal) + actionName.Length;
            int endIndex = 0;

            for (int i = startIndex; i < scriptsContent.Length; i++)
            {
                if (scriptsContent[i] != '(')
                {
                    endIndex += 1;
                    break;
                }
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                actionContent += scriptsContent[startIndex + i];
            }

            return actionName + actionContent;
        }

        private string FindActionContent(string startMark, string scriptContent)
        {
            string action = String.Empty;
            //开始位置 
            int startIndex =
                scriptContent.IndexOf(startMark, StringComparison.Ordinal) + startMark.Length;
            for (int i = startIndex; i < scriptContent.Length; i++)
            {
                // Debug.Log(scriptContent[i]);
                startIndex += 1;
                if (scriptContent[i] == '{')
                {
                    break;
                }
            }

            int endIndex = 0;
            int leftBrackets = 0;
            for (int i = startIndex; i < scriptContent.Length; i++)
            {
                if (scriptContent[i] == '{')
                {
                    leftBrackets += 1;
                }

                if (scriptContent[i] == '}')
                {
                    if (leftBrackets == 0)
                    {
                        endIndex = i;
                        break;
                    }

                    leftBrackets -= 1;
                }
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                action += scriptContent[i];
            }

            return action;
        }

        /// <summary>
        /// 查找旧的函数内容
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private string FindOldActionContent(string actionName)
        {
            if (_listenerActionList.ContainsKey(actionName))
            {
                return _listenerActionList[actionName];
            }

            return "\n" + Indents(4);
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
                        string bindStr = String.Empty;
                        BindUiType.UIEventTriggerType uiEventTriggerType =
                            child.GetComponent<BindUiType>().eventTriggerType;
                        if ((BindUiType.UIEventTriggerType.PointerClick & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerClick)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Click");
                            bindStr = Indents(4) + "private void On" + child.name + "Click" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerEnter & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerEnter)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Enter");

                            bindStr = Indents(4) + "private void On" + child.name + "Enter" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerExit & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerExit)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Exit");
                            bindStr = Indents(4) + "private void On" + child.name + "Exit" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerDown & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerDown)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Down");
                            bindStr = Indents(4) + "private void On" + child.name + "Down" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.PointerUp & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.PointerUp)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Up");
                            bindStr = Indents(4) + "private void On" + child.name + "Up" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }

                        if ((BindUiType.UIEventTriggerType.Drag & uiEventTriggerType) ==
                            BindUiType.UIEventTriggerType.Drag)
                        {
                            string oldContent = FindOldActionContent(
                                "On" + child.name + "Drag");
                            bindStr = Indents(4) + "private void On" + child.name + "Drag" +
                                      "(BaseEventData targetObj)" + "\n" + Indents(4) + "{" + oldContent +
                                      "}";
                            allUiVariableBindListenerEvent.Add(bindStr);
                        }
                    }
                    else if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Toggle)
                    {
                        string oldContent = FindOldActionContent(
                            "On" + child.name + "(bool isOn)" + "\n" + Indents(4) + "{");
                        allUiVariableBindListenerEvent.Add(Indents(4) + "private void On" + child.name +
                                                           "(bool isOn)" + "\n" + Indents(4) + "{" + oldContent + "}");
                    }
                }
            }
        }
    }
#endif
}