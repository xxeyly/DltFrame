﻿using System;
using Sirenix.OdinInspector;

namespace XFramework
{
    public partial class SceneComponent
    {
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("视图名称")] [LabelWidth(50)]
        public string viewName;

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
    }
}