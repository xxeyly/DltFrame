using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    [Serializable]
    public enum ViewShowType
    {
        [LabelText("活动")] Activity,
        [LabelText("静态")] Static
    }

    public enum ShowType
    {
        [LabelText("直接")] Direct,
        [LabelText("渐隐")] Curve
    }

    [Serializable]
    public struct TimeTaskInfo
    {
        [HideLabel] [HorizontalGroup("任务ID")] public int timeTaskId;
        [HideLabel] [HorizontalGroup("任务名称")] public string timeTaskName;
        [HideLabel] [HorizontalGroup("任务类型")] public TimeTaskList.TimeLoopType timeLoopType;
    }

    /// <summary>
    /// 视图基类
    /// </summary>
    public abstract partial class BaseWindow : MonoBehaviour
    {
        protected GameObject window;
        protected CanvasGroup canvasGroup;

        [BoxGroup("属性")] [LabelText("视图类型")] [SerializeField] [EnumToggleButtons]
        protected ViewShowType viewShowType = ViewShowType.Activity;

        [BoxGroup("属性")] [LabelText("显示类型")] [SerializeField] [EnumToggleButtons]
        protected ShowType showType = ShowType.Direct;

        [BoxGroup("属性")] [LabelText("显示时间")] [Range(0.1f, 9)] [SerializeField] [ShowIf("showType", ShowType.Curve)]
        protected float showTime = 1;

        [HideInInspector] public Type viewType;

        [BoxGroup("命名")] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("视图名称")]
        public string viewName;

        [BoxGroup("命名")] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("类名称")]
        public string typeName;


        [BoxGroup("属性")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("日志输出")]
        public bool isLog;

        [BoxGroup("属性")] [TableList(AlwaysExpanded = true, DrawScrollView = false)] [Searchable] [SerializeField] [LabelText("计时任务列表")]
        protected List<TimeTaskInfo> timeTaskInfoList = new List<TimeTaskInfo>();

        [BoxGroup("命名")]
        [Button(ButtonSizes.Medium)]
        [LabelText("重命名")]
        [GUIColor(0, 1, 0)]
        public void GameNameSet()
        {
            gameObject.name = viewType.Name;
            typeName = viewType.Name;
        }

        /// <summary>
        /// 视图显示任务
        /// </summary>
        private int _viewShowTimeTask;

        protected BaseWindow()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            viewType = InitViewType();
            // ReSharper disable once VirtualMemberCallInConstructor
            InitViewShowType();
        }

        /// <summary>
        /// 返回当前视图的活动类型
        /// </summary>
        /// <returns></returns>
        public ViewShowType GetViewShowType()
        {
            return viewShowType;
        }

        /// <summary>
        /// 视图初始化
        /// </summary>
        public virtual void ViewStartInit()
        {
            window = transform.Find("Window").gameObject;
            canvasGroup = window.GetComponent<CanvasGroup>();
            SvcInit();
            InitView();
            InitListener();
            OnlyOnceInit();
        }


        public abstract void Init();

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
        /// 视图摧毁
        /// </summary>
        public void OnViewDestroy()
        {
            ViewDestroy();
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideThisView()
        {
            ViewSvc.Instance.HideView(viewType);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        protected void ShowView()
        {
            ViewSvc.Instance.ShowView(viewType);
        }

        /// <summary>
        /// 获得当前视图的显示状态
        /// </summary>
        /// <returns></returns>
        public bool GetDisplay()
        {
            return window.activeInHierarchy;
        }
    }
}