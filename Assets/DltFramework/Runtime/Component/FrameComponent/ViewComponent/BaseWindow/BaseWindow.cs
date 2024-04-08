#define SFRAMEWORK

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    [Serializable]
    public enum ViewShowType
    {
        [LabelText("活动")] Activity,
        [LabelText("静态")] Static
    }

    [Serializable]
    public struct TimeTaskInfo
    {
        [HideLabel] [HorizontalGroup("任务ID")] public int timeTaskId;
        [HideLabel] [HorizontalGroup("任务名称")] public string timeTaskName;
    }

    /// <summary>
    /// 视图基类
    /// </summary>
    public abstract partial class BaseWindow : MonoBehaviour
    {
        protected GameObject window;
        protected CanvasGroup canvasGroup;

        [HorizontalGroup("标签")] [BoxGroup("标签/属性")] [LabelText("视图类型")] [SerializeField] [EnumToggleButtons] [LabelWidth(50)] [Tooltip("静态模式不会影响全局视图全局操作,单独指定事件会被影响")]
        protected ViewShowType viewShowType = ViewShowType.Activity;

        [BoxGroup("标签/属性")] [LabelText("UI层级")] [LabelWidth(100)] [Tooltip("ViewFrame勾选层级排序会根据索引进行排序")]
        public int sceneLayerIndex;

        [BoxGroup("标签/属性")] [LabelText("初始化")] [LabelWidth(50)] [Tooltip("该属性影响是否一开始执行Init操作")]
        public bool viewInit;

        [BoxGroup("调试")] [ToggleLeft] [GUIColor(0.3f, 0.8f, 0.8f)] [LabelText("日志输出")]
        public bool isLog;

        [BoxGroup("调试")] [TableList(AlwaysExpanded = true, DrawScrollView = false)] [Searchable] [SerializeField] [LabelText("计时任务列表")]
        protected List<TimeTaskInfo> timeTaskInfoList = new List<TimeTaskInfo>();

        public Type viewType;

        [BoxGroup("标签/命名")] [GUIColor(0.3f, 0.8f, 0.8f)] [LabelText("视图名称")] [LabelWidth(50)]
        public string viewName;

        [BoxGroup("标签/命名")] [GUIColor(0.3f, 0.8f, 0.8f)] [LabelText("类名称")] [LabelWidth(50)]
        public string typeName;

        [BoxGroup("标签/命名")]
        [Button("重命名", ButtonSizes.Medium)]
        [GUIColor(0, 1, 0)]
        public void GameNameSet()
        {
            gameObject.name = viewType.Name;
            typeName = viewType.Name;
        }

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
            InitView();
            InitListener();
            OnlyOnceInit();
            if (viewInit)
            {
                Init();
            }
        }

        public void SetSetSiblingIndex()
        {
            transform.SetSiblingIndex(sceneLayerIndex);
        }

        public int GetSceneLayerIndex()
        {
            return sceneLayerIndex;
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
        protected virtual void InitView()
        {
        }

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
        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideThisView()
        {
            ViewFrameComponent.Instance.HideView(viewType);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        protected void ShowView()
        {
            ViewFrameComponent.Instance.ShowView(viewType);
        }


        /// <summary>
        /// 获得当前视图的显示状态
        /// </summary>
        /// <returns></returns>
        public bool GetDisplay()
        {
            return window.activeSelf;
        }
    }
}