using System;
using System.Collections.Generic;
using FlyingWormConsole3.LiteNetLib;
using UnityEngine;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View.Button;

namespace XxSlitFrame.View
{
    public class AutoBindUIData : MonoBehaviour
    {
        /// <summary>
        /// 所有UI变量名称
        /// </summary>
        [HideInInspector] public List<string> allUiVariableName;

        /// <summary>
        /// 所有UI变量绑定
        /// </summary>
        [HideInInspector] public List<string> allUiVariableBind;

        /// <summary>
        /// 所有UI变量绑定事件
        /// </summary>
        [HideInInspector] public List<string> allUiVariableBindListener;

        /// <summary>
        /// 所有UI变量绑定事件声明
        /// </summary>
        [HideInInspector] public List<string> allUiVariableBindListenerEvent;

        #region UI自动生成代码

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string OneClickDeclarationUi()
        {
            Transform window = transform.Find("Window").transform;
            string allUiName = "";
            allUiVariableName = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    switch (child.GetComponent<BindUiType>().type)
                    {
                        case BindUiType.UiType.GameObject:
                            allUiName += "private GameObject _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private GameObject _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                        case BindUiType.UiType.Button:
                            allUiName += "private Button _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add(
                                "private Button _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n");


                            break;
                        case BindUiType.UiType.Image:
                            allUiName += "private Image _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private Image _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                        case BindUiType.UiType.Text:
                            allUiName += "private Text _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private Text _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n");

                            break;
                        case BindUiType.UiType.Toggle:
                            allUiName += "private Toggle _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add(
                                "private Toggle _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n");

                            break;
                        case BindUiType.UiType.RawImage:
                            allUiName += "private RawImage _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private RawImage _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                        case BindUiType.UiType.Scrollbar:
                            allUiName += "private Scrollbar _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private Scrollbar _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                        case BindUiType.UiType.ScrollRect:
                            allUiName += "private ScrollRect _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private ScrollRect _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                        case BindUiType.UiType.InputField:
                            allUiName += "private InputField _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private InputField _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                        case BindUiType.UiType.Dropdown:
                            allUiName += "private Dropdown _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("private Dropdown _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");

                            break;
                    }
                }
            }

            return allUiName;
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        public string OneClickBindUi()
        {
            Transform window = transform.Find("Window").transform;
            allUiVariableBind = new List<string>();
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    allBindName += "BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                   GetUiComponentPath(child, "") + "\");" + "\n";
                    allUiVariableBind.Add("BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                          GetUiComponentPath(child, "") + "\");" + "\n");
                }
            }


            return allBindName;
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        /// <returns></returns>
        private static string GetUiComponentPath(Transform uiTr, string uiPath)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != "Window")
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
        /// 获得UI组件是否包含LocalBaseWindnow路径
        /// </summary>
        /// <returns></returns>
        private static bool GetUiComponentContainLocalBaseWindow(Transform uiTr)
        {
            bool isContainLocalBaseWindow = false;
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != "Window")
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
                        isContainLocalBaseWindow = true;
                        return isContainLocalBaseWindow;
                    }
                }
            }


            return isContainLocalBaseWindow;
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
        /// 一键绑定UI事件
        /// </summary>
        /// <returns></returns>
        public string OneClickBindListener()
        {
            Transform window = transform.Find("Window").transform;
            string allBindName = "";
            allUiVariableBindListener = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                       "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                       ");" + "\n";
                        allUiVariableBindListener.Add("BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                                      "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                                      ");" + "\n");
                    }
                }
            }


            return allBindName;
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        public string OneClickStatementListener()
        {
            Transform window = transform.Find("Window").transform;

            string allBindName = "";
            allUiVariableBindListenerEvent = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "private void On" + child.name + "(BaseEventData targetObj)" + "\n" + "{" +
                                       "\n" + "}" + "\n";
                        allUiVariableBindListenerEvent.Add("private void On" + child.name +
                                                           "(BaseEventData targetObj)" + "\n" + "{" +
                                                           "\n" + "}" + "\n");
                    }
                }
            }

            /*TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();*/
            return allBindName;
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public void HideView()
        {
            transform.Find("Window").gameObject.SetActive(false);
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public void ShowView()
        {
            transform.Find("Window").gameObject.SetActive(true);
        }

        #endregion
    }
}