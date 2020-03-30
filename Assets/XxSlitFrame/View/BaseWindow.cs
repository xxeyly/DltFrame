using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XxSlitFrame.Tools.Svc;
using UnityEngine.UI;
using XxSlitFrame.View.Button;

namespace XxSlitFrame.View
{
    public enum ViewShowType
    {
        /// <summary>
        /// 活动
        /// </summary>
        Activity,

        /// <summary>
        /// 冻结
        /// </summary>
        Frozen
    }

    public struct TimeTaskInfo
    {
        [Header("任务ID")] public int TimeTaskId;
        [Header("任务类型")] public TimeTaskList.TimeLoopType TimeLoopType;
        [Header("任务名称")] public string TimeTaskName;
    }

    /// <summary>
    /// 视图基类
    /// </summary>
    public abstract partial class BaseWindow : MonoBehaviour
    {
        protected GameObject Window;
        protected AudioSvc AudioSvc;
        protected ResSvc ResSvc;
        protected ViewSvc ViewSvc;
        protected SceneSvc SceneSvc;
        protected PersistentDataSvc PersistentDataSvc;
        protected TimeSvc TimeSvc;
        protected ListenerSvc ListenerSvc;
        protected MouseSvc MouseSvc;

        [SerializeField] [Header("组件事件监听")] protected Dictionary<string, UnityAction<BaseEventData>> uiListener = new Dictionary<string, UnityAction<BaseEventData>>();

        [SerializeField] [Header("计时任务列表")] protected List<TimeTaskInfo> timeTaskInfoList = new List<TimeTaskInfo>();

        protected BaseWindow()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            viewType = InitViewType();
            // ReSharper disable once VirtualMemberCallInConstructor
            InitViewShowType();
        }


        protected virtual void Update()
        {
            if (timeTaskInfoList.Count >= 1)
            {
                foreach (TimeTaskInfo timeTaskInfo in timeTaskInfoList)
                {
                    if (!TimeSvc.GetAllTimeTaskId().Contains(timeTaskInfo.TimeTaskId))
                    {
                        timeTaskInfoList.Remove(timeTaskInfo);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 返回当前视图的活动类型
        /// </summary>
        /// <returns></returns>
        public ViewShowType GetViewShowType()
        {
            return ViewShowType;
        }

        /// <summary>
        /// 视图初始化
        /// </summary>
        public void ViewStartInit()
        {
            Window = transform.Find("Window").gameObject;
            AudioSvc = AudioSvc.Instance;
            ResSvc = ResSvc.Instance;
            ViewSvc = ViewSvc.Instance;
            SceneSvc = SceneSvc.Instance;
            TimeSvc = TimeSvc.Instance;
            PersistentDataSvc = PersistentDataSvc.Instance;
            ListenerSvc = ListenerSvc.Instance;
            MouseSvc = MouseSvc.Instance;
            InitView();
            InitListener();
            OnlyOnceInit();
        }


        public abstract void Init();

        /// <summary>
        /// 首先加载的步骤
        /// </summary>
        public virtual void First()
        {
        }

        /// <summary>
        /// 初始化视图的显示类型
        /// </summary>
        protected virtual void InitViewShowType()
        {
        }

        /// <summary>
        /// UI类型设置
        /// </summary>
        protected Type InitViewType()
        {
            return GetType();
        }

        /// <summary>
        /// UI绑定
        /// </summary>
        protected abstract void InitView();

        /// <summary>
        /// 事件监听
        /// </summary>
        protected abstract void InitListener();

        /// <summary>
        /// 仅仅初始化一次
        /// </summary>
        protected virtual void OnlyOnceInit()
        {
        }

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
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params Selectable[] hideObjArray)
        {
            foreach (Selectable hideObj in hideObjArray)
            {
                hideObj.gameObject.SetActive(false);
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

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(display);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 视图摧毁
        /// </summary>
        public void OnViewDestroy()
        {
            ViewDestroy();
        }

        /// <summary>
        /// 界面摧毁
        /// </summary>
        protected virtual void ViewDestroy()
        {
            for (int i = 0; i < timeTaskInfoList.Count; i++)
            {
                switch (timeTaskInfoList[i].TimeLoopType)
                {
                    case TimeTaskList.TimeLoopType.Once:
                        DeleteTimeTask(timeTaskInfoList[i].TimeTaskId);
                        break;
                    case TimeTaskList.TimeLoopType.Loop:
                        DeleteSwitchTask(timeTaskInfoList[i].TimeTaskId);
                        break;
                }
            }
            timeTaskInfoList.Clear();
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideView()
        {
            ViewSvc.HideView(viewType);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        protected void ShowView()
        {
            ViewSvc.ShowView(viewType);
        }

        /// <summary>
        /// 获得当前视图的显示状态
        /// </summary>
        /// <returns></returns>
        public bool GetDisplay()
        {
            return Window.activeInHierarchy;
        }

        /// <summary>
        /// 设置当前视图关闭或者隐藏
        /// </summary>
        /// <param name="display"></param>
        public void DisPlay(bool display)
        {
            if (display)
            {
                ShowObj(Window);
            }
            else
            {
                HideObj(Window);
                ViewDestroy();
            }
        }

        #region UI 绑定

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">需要绑定的组件</param>
        /// <param name="path">当前组件的路径</param>
        // ReSharper disable once VirtualMemberCallInConstructor
        protected void BindUi<T>(ref T viewType, string path)
        {
            viewType = transform.Find("Window/" + path).GetComponent<T>();
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi<T>(ref List<T> viewType, string path)
        {
            viewType = new List<T>(transform.Find("Window/" + path).GetComponentsInChildren<T>(true));
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">视图类型</param>
        /// <param name="path">路径</param>
        protected void BindUi(ref GameObject viewType, string path)
        {
            viewType = transform.Find("Window/" + path).GetComponent<Transform>().gameObject;
        }

        #endregion

        #region UI 事件绑定

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="button"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(UnityEngine.UI.Button button, EventTriggerType eventId, UnityAction action)
        {
            button.onClick.AddListener(action);
        }


        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(Selectable selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            /*if (!uiListener.ContainsKey(selectable.name))
            {
                uiListener.Add(selectable.name, action);
            }*/

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
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(GameObject selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            if (!uiListener.ContainsKey(selectable.name))
            {
                uiListener.Add(selectable.name, action);
            }

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
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(List<Selectable> buttonList, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            foreach (Selectable selectable in buttonList)
            {
                if (!uiListener.ContainsKey(selectable.name))
                {
                    uiListener.Add(selectable.name, action);
                }

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
        }

        protected List<Selectable> SelectableConverter<T>(List<T> selectableList) where T : Selectable
        {
            List<Selectable> returnSelectableList = new List<Selectable>();
            foreach (T selectable in selectableList)
            {
                returnSelectableList.Add(selectable);
            }

            return returnSelectableList;
        }

        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            int timeTaskId = TimeSvc.AddTimeTask(callback, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo() {TimeTaskId = timeTaskId, TimeLoopType = TimeTaskList.TimeLoopType.Once, TimeTaskName = taskName});
            return timeTaskId;
        }

        /// <summary>
        /// 增加循环计时任务
        /// </summary>
        /// <param name="callbackList"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            int timeTaskId = TimeSvc.AddSwitchTask(callbackList, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo() {TimeTaskId = timeTaskId, TimeLoopType = TimeTaskList.TimeLoopType.Loop, TimeTaskName = taskName});
            return timeTaskId;
        }

        /// <summary>
        /// 增加不摧毁任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddImmortalTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            int timeTaskId = TimeSvc.AddImmortalTimeTask(callback, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo() {TimeTaskId = timeTaskId, TimeLoopType = TimeTaskList.TimeLoopType.Once, TimeTaskName = taskName});
            TimeSvc.DeleteTimeTask();
            return timeTaskId;
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteTimeTask(int timeTaskId)
        {
            TimeSvc.DeleteTimeTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteSwitchTask(int timeTaskId)
        {
            TimeSvc.DeleteSwitchTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteImmortalTimeTask(int timeTaskId)
        {
            TimeSvc.DeleteImmortalTimeTask(timeTaskId);
            DeleteTimeTaskById(timeTaskId);
        }

        /// <summary>
        /// 根据任务ID删除任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        private void DeleteTimeTaskById(int timeTaskId)
        {
            for (int i = 0; i < timeTaskInfoList.Count; i++)
            {
                if (timeTaskInfoList[i].TimeTaskId == timeTaskId)
                {
                    timeTaskInfoList.Remove(timeTaskInfoList[i]);
                    return;
                }
            }
        }

        /// <summary>
        /// 暂停时事件
        /// </summary>
        public virtual void ViewPause()
        {
        }

        /// <summary>
        /// 继续时事件
        /// </summary>
        public virtual void ViewContinue()
        {
        }

        #endregion
    }

    /// <summary>
    /// UI自动生成部分
    /// </summary>
    public abstract partial class BaseWindow : MonoBehaviour
    {
        #region 编辑器界面操作

        [XButton("显示界面")]
        protected virtual void ShowWindow()
        {
            Window = transform.Find("Window").gameObject;
            ShowObj(Window);
        }

        [XButton("隐藏界面")]
        protected virtual void HideWindow()
        {
            Window = transform.Find("Window").gameObject;
            HideObj(Window);
        }

        [XButton("初始化界面")]
        protected virtual void DeclarationUi()
        {
            InitView();
            Init();
        }

        [XButton("UI一键生成")]
        protected void OneGenerateAllView()
        {
            ViewDeclarationUi();
            ViewBindUi();
            ViewBindListener();
            ViewStatementListener();
        }

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

        #endregion

        #region UI自动生成属性

        [TextArea(5, 5)] public string viewDeclarationUi;

        [TextArea(5, 5)] public string viewBindUi;

        [TextArea(5, 5)] public string viewBindListener;

        [TextArea(5, 5)] public string viewStatementListener;

        [HideInInspector] public Type viewType;

        [Header("视图类型")] [SerializeField] protected ViewShowType ViewShowType = ViewShowType.Activity;

        #endregion

        #region UI自动生成代码

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string OneClickDeclarationUi()
        {
            Transform window = transform.Find("Window").transform;
            string allUiName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    switch (child.GetComponent<BindUiType>().type)
                    {
                        case BindUiType.UiType.GameObject:
                            allUiName += "private GameObject _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LGameObject:
                            allUiName += "private List<GameObject> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
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
                        case BindUiType.UiType.Input:
                            allUiName += "private InputField _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.DropDown:
                            allUiName += "private DropDown _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";

                            break;
                        case BindUiType.UiType.RawImage:
                            allUiName += "private RawImage _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Slider:
                            allUiName += "private Slider _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Scrollbar:
                            allUiName += "private Scrollbar _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.ScrollRect:
                            allUiName += "private ScrollRect _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LButton:
                            allUiName += "private List<Button> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LImage:
                            allUiName += "private List<Image> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LText:
                            allUiName += "private List<Text> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LToggle:
                            allUiName += "private List<Toggle> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LInput:
                            allUiName += "private List<InputField> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LDropDown:
                            allUiName += "private List<DropDown> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LRawImage:
                            allUiName += "private List<RawImage> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LSlider:
                            allUiName += "private List<Slider> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LScrollbar:
                            allUiName += "private List<Scrollbar> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.LScrollView:
                            allUiName += "private List<ScrollView> _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return allUiName;
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        private string OneClickBindUi()
        {
            Transform window = transform.Find("Window").transform;
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    allBindName += "BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                   GetUiComponentPath(child, "") + "\");" + "\n";
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
        private string OneClickBindListener()
        {
            Transform window = transform.Find("Window").transform;
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," + "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                       ");" + "\n";
                    }

                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.LButton)
                    {
                        allBindName += "BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," + "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                       ");" + "\n";
                    }
                }
            }


            return allBindName;
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        private string OneClickStatementListener()
        {
            Transform window = transform.Find("Window").transform;

            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "private void On" + child.name + "(BaseEventData targetObj)" + "\n" + "{" + "\n" + "}" + "\n";
                    }
                    else if (child.GetComponent<BindUiType>().type == BindUiType.UiType.LButton)
                    {
                        allBindName += "private void On" + child.name + "(BaseEventData targetObj)" + "\n" + "{" + "\n" + "}" + "\n";
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

        #endregion
    }
}