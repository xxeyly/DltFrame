using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public partial class SceneComponent
    {
        #region 无返回方法

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent(string eventType, ListenerFrameComponent.CallBack callBack)
        {
            ListenerFrameComponent.Instance.AddListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T>(string eventType, ListenerFrameComponent.CallBack<T> callBack)
        {
            ListenerFrameComponent.Instance.AddListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X>(string eventType, ListenerFrameComponent.CallBack<T, X> callBack)
        {
            ListenerFrameComponent.Instance.AddListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y>(string eventType, ListenerFrameComponent.CallBack<T, X, Y> callBack)
        {
            ListenerFrameComponent.Instance.AddListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y, Z>(string eventType, ListenerFrameComponent.CallBack<T, X, Y, Z> callBack)
        {
            ListenerFrameComponent.Instance.AddListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y, Z, W>(string eventType, ListenerFrameComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerFrameComponent.Instance.AddListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        #endregion

        #region 返回方法

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<R>(string eventType, ListenerFrameComponent.ReturnCallBack<R> returnCallBack)
        {
            ListenerFrameComponent.Instance.AddReturnListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, R>(string eventType, ListenerFrameComponent.ReturnCallBack<T, R> returnCallBack)
        {
            ListenerFrameComponent.Instance.AddReturnListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, R>(string eventType, ListenerFrameComponent.ReturnCallBack<T, X, R> returnCallBack)
        {
            ListenerFrameComponent.Instance.AddReturnListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, R>(string eventType, ListenerFrameComponent.ReturnCallBack<T, X, Y, R> returnCallBack)
        {
            ListenerFrameComponent.Instance.AddReturnListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, R>(string eventType, ListenerFrameComponent.ReturnCallBack<T, X, Y, Z, R> returnCallBack)
        {
            ListenerFrameComponent.Instance.AddReturnListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, W, R>(string eventType, ListenerFrameComponent.ReturnCallBack<T, X, Y, Z, W, R> returnCallBack)
        {
            ListenerFrameComponent.Instance.AddReturnListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), returnCallBack);
        }

        #endregion

        #region 删除事件

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void RemoveListenerEvent(string eventType, ListenerFrameComponent.CallBack callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T>(string eventType, ListenerFrameComponent.CallBack<T> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X>(string eventType, ListenerFrameComponent.CallBack<T, X> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y>(string eventType, ListenerFrameComponent.CallBack<T, X, Y> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y, Z>(string eventType, ListenerFrameComponent.CallBack<T, X, Y, Z> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void RemoveListenerEvent<T, X, Y, Z, W>(string eventType, ListenerFrameComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerFrameComponent.Instance.RemoveListenerEvent(DataFrameComponent.StringBuilderString(GetType().ToString(), "-", eventType), callBack);

        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        private void RemoveListenerEvent(string eventType)
        {
            ListenerFrameComponent.Instance.RemoveDelegateToListenerEvent(eventType);
        }

        #endregion
    }
}