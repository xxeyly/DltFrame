using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace XFramework
{
    [RequireComponent(typeof(ListenerSvcGenerateData))]
    public partial class ListenerSvc : SvcBase
    {
        public static ListenerSvc Instance;

        public delegate void CallBack();

        public delegate void CallBack<T>(T t);

        public delegate void CallBack<T, X>(T t, X x);

        public delegate void CallBack<T, X, Y>(T t, X x, Y y);

        public delegate void CallBack<T, X, Y, Z>(T t, X x, Y y, Z z);

        public delegate void CallBack<T, X, Y, Z, W>(T t, X x, Y y, Z z, W w);

        [SerializeField] private Dictionary<string, List<Delegate>> listenerDic;

        public override void StartSvc()
        {
            Instance = GetComponent<ListenerSvc>();
        }

        public override void InitSvc()
        {
            listenerDic = new Dictionary<string, List<Delegate>>();
        }

        public override void EndSvc()
        {
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent(string eventType, CallBack callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加委托
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        private void AddDelegateToListenerEvent(string eventType, Delegate customDelegate)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                List<Delegate> delegates = new List<Delegate> {customDelegate};
                listenerDic.Add(eventType, delegates);
            }
            else
            {
                List<Delegate> delegates = listenerDic[eventType];
                if (!delegates.Contains(customDelegate))
                {
                    delegates.Add(customDelegate);
                }
                else
                {
                    Debug.LogError(eventType + "该事件已经被绑定了");
                }
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T>(string eventType, CallBack<T> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X>(string eventType, CallBack<T, X> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y>(string eventType, CallBack<T, X, Y> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z>(string eventType, CallBack<T, X, Y, Z> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, X, Y, Z, W>(string eventType,
            CallBack<T, X, Y, Z, W> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void DeleteListenerEvent(string eventType, UnityAction callBack)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                listenerDic[eventType].Remove(callBack);
            }
            else
            {
                Debug.LogError("该事件没有被绑定过");
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        private void ExecuteEvent(string eventType)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in listenerDic[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 0)
                    {
                        ((CallBack) customDelegate)();
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        private void ExecuteEvent<T>(string eventType, T t)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in listenerDic[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType())
                    {
                        ((CallBack<T>) customDelegate)(t);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="x"></param>
        private void ExecuteEvent<T, X>(string eventType, T t, X x)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in listenerDic[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType())
                    {
                        ((CallBack<T, X>) customDelegate)(t, x);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        private void ExecuteEvent<T, X, Y>(string eventType, T t, X x, Y y)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in listenerDic[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType())
                    {
                        ((CallBack<T, X, Y>) customDelegate)(t, x, y);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        private void ExecuteEvent<T, X, Y, Z>(string eventType, T t, X x, Y y, Z z)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in listenerDic[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType())
                    {
                        ((CallBack<T, X, Y, Z>) customDelegate)(t, x, y, z);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>r
        /// <param name="t"></param>
        /// <param name="y"></param>
        private void ExecuteEvent<T, X, Y, Z, W>(string eventType, T t, X x, Y y, Z z, W w)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in listenerDic[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == x.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == y.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == z.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == w.GetType())
                    {
                        ((CallBack<T, X, Y, Z, W>) customDelegate)(t, x, y, z, w);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }
    }
}