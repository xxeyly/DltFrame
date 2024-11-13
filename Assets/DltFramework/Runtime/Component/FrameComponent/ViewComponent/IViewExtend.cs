using System;
using UnityEngine;

namespace DltFramework
{
    public interface IViewExtend
    {
        /// <summary>
        /// 视图实例化
        /// </summary>
        /// <param name="instantiate">要实例化的GameObject</param>
        /// <param name="parent">父节点</param>
        /// <param name="world">是否在世界坐标系下</param>
        /// <returns></returns>
        public GameObject E_Instantiate(GameObject instantiate, Transform parent, bool world);

        /// <summary>
        /// 销毁视图实例
        /// </summary>
        /// <param name="view">视图类型</param>
        /// <returns></returns>
        public bool V_GetViewExistence(Type view);

        /// <summary>
        /// 获得某个视图的显示状态
        /// </summary>
        /// <param name="view">视图类型</param>
        /// <returns></returns>
        public bool V_GetViewState(Type view);

        /// <summary>
        /// 获得某个视图的实例
        /// </summary>
        /// <param name="view">视图类型</param>
        /// <returns></returns>
        public BaseWindow V_GetView(Type view);

        /// <summary>
        /// 获得当前活动的视图数量
        /// </summary>
        /// <returns></returns>
        public int V_GetCurrentActiveViewCount();


        /// <summary>
        /// 获得当前场景中视图的数量
        /// </summary>
        public int V_GetCurrentSceneViewCount();

        #region 显示视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type">视图类型</param>
        public void V_ShowView(Type type);

        /// <summary>
        /// 显示一些视图
        /// </summary>
        /// <param name="types">视图类型数组</param>
        public void V_ShowView(params Type[] types);

        #endregion

        #region 隐藏视图

        /// <summary>
        /// 显示单一视图类型
        /// </summary>
        /// <param name="type">视图类型</param>
        public void V_HideView(Type type);

        /// <summary>
        /// 隐藏一些视图
        /// </summary>
        /// <param name="types">视图类型数组</param>
        public void V_HideView(params Type[] types);

        /// <summary>
        /// 隐藏所有视图
        /// </summary>
        public void V_HideAllView();

        #endregion
    }
}