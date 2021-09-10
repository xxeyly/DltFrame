using UnityEngine.Events;

namespace XFramework
{
    partial class BaseWindow
    {
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        public void AddListenerEvent(string eventType, ListenerSvc.CallBack unityAction)
        {
            ListenerSvc.Instance.AddListenerEvent(eventType, unityAction);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T>(string eventType, ListenerSvc.CallBack<T> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY>(string eventType, ListenerSvc.CallBack<T, TY> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY, TYX>(string eventType, ListenerSvc.CallBack<T, TY, TYX> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY, TYX, TYXZ>(string eventType, ListenerSvc.CallBack<T, TY, TYX, TYXZ> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY, TYX, TYXZ, TYXZW>(string eventType, ListenerSvc.CallBack<T, TY, TYX, TYXZ, TYXZW> callBack)
        {
            ListenerSvc.Instance.AddListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        public void DeleteListenerEvent(string eventType, UnityAction unityAction)
        {
            ListenerSvc.Instance.DeleteListenerEvent(eventType, unityAction);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        public void ExecuteEvent(string eventType)
        {
            ListenerSvc.Instance.ExecuteEvent(eventType);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        public void ExecuteEvent<T>(string eventType, T t)
        {
            ListenerSvc.Instance.ExecuteEvent(eventType, t);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        public void ExecuteEvent<T, TY>(string eventType, T t, TY y)
        {
            ListenerSvc.Instance.ExecuteEvent(eventType, t, y);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        public void ExecuteEvent<T, TY, TX>(string eventType, T t, TY y, TX x)
        {
            ListenerSvc.Instance.ExecuteEvent(eventType, t, y, x);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        public void ExecuteEvent<T, Y, X, Z>(string eventType, T t, Y y, X x, Z z)
        {
            ListenerSvc.Instance.ExecuteEvent(eventType, t, y, x, z);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        public void ExecuteEvent<T, Y, X, Z, W>(string eventType, T t, Y y, X x, Z z, W w)
        {
            ListenerSvc.Instance.ExecuteEvent(eventType, t, y, x, z, w);
        }
    }
}