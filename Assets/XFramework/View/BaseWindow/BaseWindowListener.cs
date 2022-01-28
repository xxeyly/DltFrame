using UnityEngine.Events;

namespace XFramework
{
    partial class BaseWindow
    {
        #region 无返回方法

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
        protected void AddListenerEvent<T, X, Y, Z>(string eventType,
            ListenerComponent.CallBack<T, X, Y, Z> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y, Z, W>(string eventType,
            ListenerComponent.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerComponent.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void DeleteListenerEvent(string eventType, UnityAction unityAction)
        {
            ListenerComponent.Instance.DeleteListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        #endregion

        #region 返回方法

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<R>(string eventType, ListenerComponent.ReturnCallBack<R> returnCallBack)
        {
            ListenerComponent.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, R>(string eventType, ListenerComponent.ReturnCallBack<T, R> returnCallBack)
        {
            ListenerComponent.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, R>(string eventType, ListenerComponent.ReturnCallBack<T, X, R> returnCallBack)
        {
            ListenerComponent.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, R>(string eventType, ListenerComponent.ReturnCallBack<T, X, Y, R> returnCallBack)
        {
            ListenerComponent.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, R>(string eventType, ListenerComponent.ReturnCallBack<T, X, Y, Z, R> returnCallBack)
        {
            ListenerComponent.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, W, R>(string eventType, ListenerComponent.ReturnCallBack<T, X, Y, Z, W, R> returnCallBack)
        {
            ListenerComponent.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void DeleteReturnListenerEvent(string eventType, UnityAction unityAction)
        {
            ListenerComponent.Instance.DeleteReturnListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        #endregion
    }
}