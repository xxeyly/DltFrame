using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View.Button;

namespace XxSlitFrame.View
{
    /// <summary>
    /// 局部UI界面
    /// </summary>
    public abstract class LocalBaseWindow : MonoBehaviour
    {
        [TextArea(5, 5)] public string viewDeclarationUi;

        [TextArea(5, 5)] public string viewBindUi;

        [TextArea(5, 5)] public string viewBindListener;

        [TextArea(5, 5)] public string viewStatementListener;

        public void Init()
        {
            InitView();
            InitListener();
            InitData();
        }

        #region 编辑器界面操作

        [XButton("一键获得UI")]
        protected void OneGenerateAllView()
        {
            ViewDeclarationUi();
            ViewBindUi();
            ViewBindListener();
            ViewStatementListener();
        }

        #endregion

        #region UI绑定

        protected virtual void ViewDeclarationUi()
        {
            viewDeclarationUi = OneClickDeclarationUi();
        }

        protected virtual void ViewBindUi()
        {
            viewBindUi = OneClickBindUi();
        }

        protected virtual void ViewBindListener()
        {
            viewBindListener = OneClickBindListener();
        }

        protected virtual void ViewStatementListener()
        {
            viewStatementListener = OneClickStatementListener();
        }

        #endregion

        #region UI绑定

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string OneClickDeclarationUi()
        {
            Transform window = transform;
            string allUiName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    switch (child.GetComponent<BindUiType>().type)
                    {
                        case BindUiType.UiType.GameObject:
                            allUiName += "private GameObject _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Button:
                            allUiName += "private Button _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Image:
                            allUiName += "private Image _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Text:
                            allUiName += "private Text _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Toggle:
                            allUiName += "private Toggle _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.RawImage:
                            allUiName += "private RawImage _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Scrollbar:
                            allUiName += "private Scrollbar _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.ScrollRect:
                            allUiName += "private ScrollRect _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.InputField:
                            allUiName += "private InputField _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Dropdown:
                            allUiName += "private Dropdown _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allUiName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allUiName;
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        private string OneClickBindUi()
        {
            Transform window = transform;

            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    allBindName += "BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                   GetUiComponentPath(child, "") + "\");" + "\n";
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allBindName;
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        /// <returns></returns>
        private string GetUiComponentPath(Transform uiTr, string uiPath)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != gameObject.name)
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
        /// 根据UI层级获得父物体
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        private Transform GetParentByHierarchy(Transform uiTr, int hierarchy)
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
        private string OneClickBindListener()
        {
            Transform window = transform;

            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," + "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                       ");" + "\n";
                    }
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allBindName;
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        private string OneClickStatementListener()
        {
            Transform window = transform;
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "private void On" + child.name + "(BaseEventData targetObj)" + "\n" + "{" + "\n" + "}" + "\n";
                    }
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allBindName;
        }

        #endregion

        protected abstract void InitView();
        protected abstract void InitListener();
        protected abstract void InitData();

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params GameObject[] hideObjArray)
        {
            foreach (GameObject hideObj in hideObjArray)
            {
                hideObj.SetActive(false);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(true);
            }
        }

        #region UI 绑定

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">需要绑定的组件</param>
        /// <param name="path">当前组件的路径</param>
        protected void BindUi<T>(ref T viewType, string path)
        {
            viewType = transform.Find(path).GetComponent<T>();
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi<T>(ref List<T> viewType, string path)
        {
            viewType = new List<T>(transform.Find(path).GetComponentsInChildren<T>());
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi(ref GameObject viewType, string path)
        {
            viewType = transform.Find(path).GetComponent<Transform>().gameObject;
        }

        #endregion

        #region UI 事件绑定

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(UIBehaviour selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = selectable.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            UnityAction<BaseEventData> callback = action;
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventId};
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="buttonList">当前要操作的UI组件</param>
        /// <param name="eventID">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(List<Selectable> buttonList, EventTriggerType eventID, UnityAction<BaseEventData> action)
        {
            foreach (Selectable selectable in buttonList)
            {
                EventTrigger trigger = selectable.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = selectable.gameObject.AddComponent<EventTrigger>();
                }

                if (trigger.triggers.Count == 0)
                {
                    trigger.triggers = new List<EventTrigger.Entry>();
                }

                UnityAction<BaseEventData> callback = action;
                EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventID};
                entry.callback.AddListener(callback);
                trigger.triggers.Add(entry);
            }
        }

        protected List<Selectable> SelectableConverter<T>(List<T> selectableList) where T : Selectable
        {
            List<Selectable> SelectableList = new List<Selectable>();
            foreach (Selectable selectable in selectableList)
            {
                SelectableList.Add(selectable);
            }

            return SelectableList;
        }

        #endregion
    }
}