using System;
using System.Collections.Generic;

namespace XFramework
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
            return ViewComponent.Instance.GetViewState(view);
        }


        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        protected int GetCurrentActiveViewCount()
        {
            return ViewComponent.Instance.GetCurrentActiveViewCount();
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideThisView()
        {
            ViewComponent.Instance.HideView(viewType);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        protected void ShowView()
        {
            ViewComponent.Instance.ShowView(viewType);
        }

        #region 显示视图

        protected void ShowThisView()
        {
            ViewComponent.Instance.ShowView(viewType);
        }

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        protected void ShowView(Type type)
        {
            ViewComponent.Instance.ShowView(type);
        }

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types"></param>
        protected void ShowView(params Type[] types)
        {
            ViewComponent.Instance.ShowView(types);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        protected void ShowView(Type type, float time)
        {
            ViewComponent.Instance.ShowView(type, time);
        }

        /// <summary>
        /// 等待一段时间后,显示视图
        /// </summary>
        /// <param name="typeList"></param>
        /// <param name="time">切换所需时间</param>
        protected void ShowView(float time, params Type[] typeList)
        {
            ViewComponent.Instance.ShowView(time, typeList);
        }

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type"></param>
        protected void HideView(Type type)
        {
            ViewComponent.Instance.HideView(type);
        }

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types"></param>
        protected void HideView(params Type[] types)
        {
            ViewComponent.Instance.HideView(types);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="type">视图类型</param>
        /// <param name="time">切换所需时间</param>
        protected void HideView(Type type, float time)
        {
            ViewComponent.Instance.HideView(type, time);
        }

        /// <summary>
        /// 等待一段时间后,隐藏视图
        /// </summary>
        /// <param name="types"></param>
        /// <param name="time">切换所需时间</param>
        protected void HideView(float time, params Type[] types)
        {
            ViewComponent.Instance.HideView(time, types);
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideAllView()
        {
            ViewComponent.Instance.HideAllView();
        }

        /// <summary>
        /// 除其全部隐藏
        /// </summary>
        /// <param name="types"></param>
        public void ExceptForHideAllView(params Type[] types)
        {
            ViewComponent.Instance.ExceptForHideAllView(types);
        }

        /// <summary>
        /// 除其父类全部隐藏
        /// </summary>
        public void ExceptForParentHideAllView()
        {
            Type[] parentBaseWindowType = new Type[parentBaseWindow.Count];

            for (int i = 0; i < parentBaseWindow.Count; i++)
            {
                if (parentBaseWindow[i] != null)
                {
                    parentBaseWindowType[i] = parentBaseWindow[i].viewType;
                }
            }

            ViewComponent.Instance.ExceptForHideAllView(parentBaseWindowType);
        }

        #endregion
    }
}