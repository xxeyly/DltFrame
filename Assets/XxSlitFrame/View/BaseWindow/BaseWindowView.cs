using System;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    partial class BaseWindow
    {
        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public bool GetViewState(Type view)
        {
            return ViewSvc.Instance.GetViewState(view);
        }


        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        public int GetCurrentActiveViewCount()
        {
            return ViewSvc.Instance.GetCurrentActiveViewCount();
        }


        #region 显示视图

        public void ShowThisView()
        {
            ViewSvc.Instance.ShowView(viewType);
        }

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void ShowView(Type type)
        {
            ViewSvc.Instance.ShowView(type);
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        public void ShowView(params Type[] types)
        {
            ViewSvc.Instance.ShowView(types);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        public void ShowView(Type type, float time)
        {
            ViewSvc.Instance.ShowView(type, time);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="typeList"></param>
        /// <param name="time">切换所需时间</param>
        public void ShowView(float time, params Type[] typeList)
        {
            ViewSvc.Instance.ShowView(time, typeList);
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        public void HideView(Type type)
        {
            ViewSvc.Instance.HideView(type);
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        public void HideView(params Type[] types)
        {
            ViewSvc.Instance.HideView(types);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        public void HideView(Type type, float time)
        {
            ViewSvc.Instance.HideView(type, time);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="types"></param>
        /// <param name="time">切换所需时间</param>
        public void HideView(float time, params Type[] types)
        {
            ViewSvc.Instance.HideView(time, types);
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        public void HideAllView()
        {
            ViewSvc.Instance.HideAllView();
        }

        #endregion
    }
}