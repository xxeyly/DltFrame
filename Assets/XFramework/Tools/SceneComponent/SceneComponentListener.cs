﻿using UnityEngine.Events;

namespace XFramework
{
    public partial class SceneComponent
    {
        #region 增加事件

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void AddListenerEvent(string eventType, ListenerComponent.CallBack unityAction)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T>(string eventType, ListenerComponent.CallBack<T> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X>(string eventType, ListenerComponent.CallBack<T, X> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y>(string eventType, ListenerComponent.CallBack<T, X, Y> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z>(string eventType,
            ListenerComponent.CallBack<T, X, Y, Z> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z, W>(string eventType,
            ListenerComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        #endregion

        #region 删除事件

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void RemoveListenerEvent(string eventType, ListenerComponent.CallBack unityAction)
        {
            ListenerComponent.Instance.RemoveListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T>(string eventType, ListenerComponent.CallBack<T> callBack)
        {
            ListenerComponent.Instance.RemoveListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X>(string eventType, ListenerComponent.CallBack<T, X> callBack)
        {
            ListenerComponent.Instance.RemoveListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y>(string eventType, ListenerComponent.CallBack<T, X, Y> callBack)
        {
            ListenerComponent.Instance.RemoveListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y, Z>(string eventType,
            ListenerComponent.CallBack<T, X, Y, Z> callBack)
        {
            ListenerComponent.Instance.RemoveListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y, Z, W>(string eventType,
            ListenerComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerComponent.Instance.RemoveListenerEvent(GetType() + "_" + eventType, callBack);
        }

        #endregion
    }
}