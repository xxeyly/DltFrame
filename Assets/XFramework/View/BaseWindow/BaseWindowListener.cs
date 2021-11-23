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
    }
}