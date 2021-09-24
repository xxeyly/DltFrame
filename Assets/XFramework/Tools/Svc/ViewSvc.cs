using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
namespace XFramework
{
    /// <summary>
    /// 视图服务
    /// </summary>
    public class ViewSvc : SvcBase
    {
        public static ViewSvc Instance;

        /// <summary>
        /// 所有的视图窗口
        /// </summary>
        [LabelText("所有的视图窗口")] [SerializeField] [Searchable]
        private List<BaseWindow> allViewWind;


        /// <summary>
        /// 视图类型与视图窗口的键值对
        /// </summary>
        private Dictionary<Type, BaseWindow> _activeViewDlc = new Dictionary<Type, BaseWindow>();

        /// <summary>
        /// 所有活动的视图
        /// </summary>
        [LabelText("所有活动的视图")] private List<Type> _allActiveView = new List<Type>();

        /// <summary>
        /// 视图计时任务ID
        /// </summary>
        private int _viewTimeTaskId;

        /// <summary>
        /// 获得当前场景中的Canvas
        /// </summary>
        [HideInInspector] public Canvas canvas;

        /// <summary>
        /// 版本信息加载完毕任务
        /// </summary>
        private int _checkVersionInfoLoadOverTaskTime;

        public delegate void DelegateShowView(Type type);

        public DelegateShowView onShowView;

        public delegate void DelegateHideView(Type type);

        public DelegateHideView onHideView;


        public override void StartSvc()
        {
            Instance = GetComponent<ViewSvc>();
        }

        /// <summary>
        /// 静态视图初始化
        /// </summary>
        public void StateViewInit()
        {
            foreach (BaseWindow window in allViewWind)
            {
                if (window.GetViewShowType() == ViewShowType.Static)
                {
                    window.Init();
                }
            }
        }

        public override void InitSvc()
        {
            // List<BaseWindow> tempSceneBaseWindow = new List<BaseWindow>(FindObjectsOfType<BaseWindow>());
            List<BaseWindow> tempSceneBaseWindow = DataSvc.GetAllObjectsInScene<BaseWindow>();
            allViewWind = new List<BaseWindow>();
            // allViewWind = new List<BaseWindow>(FindObjectsOfType<BaseWindow>());
            for (int i = 0; i < tempSceneBaseWindow.Count; i++)
            {
                if (!tempSceneBaseWindow[i].GetComponent<ChildBaseWindow>())
                {
                    allViewWind.Add(tempSceneBaseWindow[i]);
                }
            }

            _activeViewDlc = new Dictionary<Type, BaseWindow>();
            _allActiveView = new List<Type>();
            AddView();
            foreach (BaseWindow window in allViewWind)
            {
                window.ViewStartInit();
            }

            StateViewInit();

            //关闭全体禁止响应
            CloseNoAllResponse();
        }

        public override void EndSvc()
        {
        }

        /// <summary>
        /// 获得视图是否存在
        /// </summary>
        /// <returns></returns>
        private bool GetViewExistence(Type view)
        {
            if (_activeViewDlc != null && _activeViewDlc.ContainsKey(view))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public bool GetViewState(Type view)
        {
            if (_activeViewDlc != null && _activeViewDlc.ContainsKey(view))
            {
                return _activeViewDlc[view].GetDisplay();
            }

            return false;
        }

        /// <summary>
        /// 所有视图初始化
        /// </summary>
        public void AllViewWindInit()
        {
            foreach (KeyValuePair<Type, BaseWindow> pair in _activeViewDlc)
            {
                pair.Value.GetComponent<BaseWindow>().Init();
            }
        }

        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        public int GetCurrentActiveViewCount()
        {
            int currentActiveViewCount = 0;
            return _allActiveView.Count - currentActiveViewCount;
        }

        /// <summary>
        /// 获得当前场景中视图的数量
        /// </summary>
        public int GetCurrentSceneViewCount()
        {
            return allViewWind.Count;
        }

        /// <summary>
        /// 添加视图类型,视图类型不为Normal的都添加到视图键值对中
        /// </summary>
        private void AddView()
        {
            foreach (BaseWindow baseWindow in allViewWind)
            {
                if (!_activeViewDlc.ContainsKey(baseWindow.viewType))
                {
                    _activeViewDlc.Add(baseWindow.viewType, baseWindow);
                }
                else
                {
                    Debug.LogError("当前视图:" + baseWindow.viewType + "场景中存在多个");
                }
            }
        }

        /// <summary>
        ///  删除所有视图
        /// </summary>
        public void DeleteAllView()
        {
            allViewWind.Clear();
            _activeViewDlc.Clear();
            _allActiveView.Clear();
        }

        #region 显示视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void ShowView(Type type)
        {
            _activeViewDlc[type].DisPlay(true);
            _activeViewDlc[type].Init();
            if (!_allActiveView.Contains(type))
            {
                _allActiveView.Add(type);
            }

            onShowView.Invoke(type);
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        public void ShowView(params Type[] types)
        {
            foreach (Type viewType in types)
            {
                ShowView(viewType);
            }
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        public void ShowView(Type type, float time)
        {
            StopViewTimeTask();
            ViewTimeTask(ShowView, type, time);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="typeList"></param>
        /// <param name="time">切换所需时间</param>
        public void ShowView(float time, params Type[] typeList)
        {
            StopViewTimeTask();
            ViewTimeTask(ShowView, typeList, time);
        }

        /// <summary>
        /// 消融View
        /// </summary>
        public void AblationView(Type viewType)
        {
            _activeViewDlc[viewType].DisPlay(true);
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void HideView(Type type)
        {
            BaseWindow baseWindow = _activeViewDlc[type];
            if (baseWindow != null) baseWindow.DisPlay(false);
            if (_allActiveView.Contains(type))
            {
                _allActiveView.Remove(type);
            }

            onHideView.Invoke(type);
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        public void HideView(params Type[] types)
        {
            foreach (Type viewType in types)
            {
                HideView(viewType);
            }
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        public void HideView(Type type, float time)
        {
            StopViewTimeTask();
            ViewTimeTask(HideView, type, time);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="types"></param>
        /// <param name="time">切换所需时间</param>
        public void HideView(float time, params Type[] types)
        {
            StopViewTimeTask();
            ViewTimeTask(HideView, types, time);
        }

        public void HideAllView()
        {
            foreach (KeyValuePair<Type, BaseWindow> pair in _activeViewDlc)
            {
                if (pair.Value.GetViewShowType() == ViewShowType.Activity)
                {
                    HideView(pair.Key);
                }
            }
        }

        /// <summary>
        /// 全局禁止响应
        /// </summary>
        public void NoAllResponse()
        {
            //TODO 未开发
        }

        /// <summary>
        /// 关闭全局禁止响应
        /// </summary>
        public void CloseNoAllResponse()
        {
            //TODO 未开发
        }

        /// <summary>
        /// 冻结视图
        /// </summary>
        public void FrozenView(Type viewType)
        {
            _activeViewDlc[viewType].DisPlay(false);
        }

        #endregion

        #region 视图任务

        /// <summary>
        /// 间隔一段时间后执行视图任务
        /// </summary>
        /// <typeparam name="T">视图类型</typeparam>
        /// <param name="viewAction">要执行的操作</param>
        /// <param name="viewType">视图类型</param>
        /// <param name="time">多长时间后执行</param>
        private void ViewTimeTask<T>(UnityAction<T> viewAction, T viewType, float time)
        {
            _viewTimeTaskId = TimeSvc.Instance.AddTimeTask(() => { viewAction.Invoke(viewType); }, "视图任务", time);
        }

        /// <summary>
        /// 结束视图计时任务
        /// </summary>
        private void StopViewTimeTask()
        {
            TimeSvc.Instance.DeleteTimeTask(_viewTimeTaskId);
        }

        #endregion

        /// <summary>
        /// 所有视图的隐藏摧毁任务
        /// </summary>
        public void AllViewDestroy()
        {
            foreach (BaseWindow window in allViewWind)
            {
                window.OnViewDestroy();
            }
        }
    }
}