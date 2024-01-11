using System;
using UnityEngine;

namespace DltFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        protected bool GetViewState(Type view)
        {
            return ViewFrameComponent.Instance.GetViewState(view);
        }

        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        protected int GetCurrentActiveViewCount()
        {
            return ViewFrameComponent.Instance.GetCurrentActiveViewCount();
        }

        #region 显示视图

        protected void ShowThisView()
        {
            ViewFrameComponent.Instance.ShowView(viewType);
        }

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        protected void ShowView(Type type)
        {
            ViewFrameComponent.Instance.ShowView(type);
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        protected void ShowView(params Type[] types)
        {
            ViewFrameComponent.Instance.ShowView(types);
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        protected void HideView(Type type)
        {
            ViewFrameComponent.Instance.HideView(type);
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        protected void HideView(params Type[] types)
        {
            ViewFrameComponent.Instance.HideView(types);
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideAllView()
        {
            ViewFrameComponent.Instance.HideAllView();
        }

        #endregion

        public virtual void OnViewHide()
        {
        }

        /// <summary>
        /// 界面摧毁
        /// </summary>
        public virtual void OnViewDestroy()
        {
        }
    }
}