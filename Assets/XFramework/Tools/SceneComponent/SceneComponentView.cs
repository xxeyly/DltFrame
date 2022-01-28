using System;

namespace XFramework
{
    public partial class SceneComponent
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


        #region 显示视图

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

        #endregion
    }
}