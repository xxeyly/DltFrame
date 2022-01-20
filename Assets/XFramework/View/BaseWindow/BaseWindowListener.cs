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
        protected void AddListenerEvent(string eventType, ListenerSvc.CallBack unityAction)
        {
            ListenerSvc.Instance.AddListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T>(string eventType, ListenerSvc.CallBack<T> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X>(string eventType, ListenerSvc.CallBack<T, X> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y>(string eventType, ListenerSvc.CallBack<T, X, Y> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y, Z>(string eventType,
            ListenerSvc.CallBack<T, X, Y, Z> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        protected void AddListenerEvent<T, X, Y, Z, W>(string eventType,
            ListenerSvc.CallBack<T, X, Y, Z, W> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(GetType() + "_" + eventType, callBack);
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void DeleteListenerEvent(string eventType, UnityAction unityAction)
        {
            ListenerSvc.Instance.DeleteListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        #endregion

        #region 返回方法

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<R>(string eventType, ListenerSvc.ReturnCallBack<R> returnCallBack)
        {
            ListenerSvc.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, R>(string eventType, ListenerSvc.ReturnCallBack<T, R> returnCallBack)
        {
            ListenerSvc.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, R>(string eventType, ListenerSvc.ReturnCallBack<T, X, R> returnCallBack)
        {
            ListenerSvc.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, R>(string eventType, ListenerSvc.ReturnCallBack<T, X, Y, R> returnCallBack)
        {
            ListenerSvc.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, R>(string eventType, ListenerSvc.ReturnCallBack<T, X, Y, Z, R> returnCallBack)
        {
            ListenerSvc.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="returnCallBack"></param>
        protected void AddReturnListenerEvent<T, X, Y, Z, W, R>(string eventType, ListenerSvc.ReturnCallBack<T, X, Y, Z, W, R> returnCallBack)
        {
            ListenerSvc.Instance.AddReturnListenerEvent(GetType() + "_" + eventType, returnCallBack);
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        protected void DeleteReturnListenerEvent(string eventType, UnityAction unityAction)
        {
            ListenerSvc.Instance.DeleteReturnListenerEvent(GetType() + "_" + eventType, unityAction);
        }

        #endregion
    }
}