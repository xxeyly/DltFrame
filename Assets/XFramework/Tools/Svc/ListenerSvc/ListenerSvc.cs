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

        public delegate void CallBack<T, X>(T arg1, X arg2);

        public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);

        public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);

        public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);

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
        /// <param name="delegate"></param>
        private void AddDelegateToListenerEvent(string eventType, Delegate @delegate)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                List<Delegate> delegates = new List<Delegate>();
                delegates.Add(@delegate);
                listenerDic.Add(eventType, delegates);
            }
            else
            {
                List<Delegate> delegates = listenerDic[eventType];
                if (!delegates.Contains(@delegate))
                {
                    delegates.Add(@delegate);
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
                foreach (Delegate @delegate in listenerDic[eventType])
                {
                    try
                    {
                        ((CallBack) @delegate)?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
                foreach (Delegate @delegate in listenerDic[eventType])
                {
                    try
                    {
                        ((CallBack<T>) @delegate)?.Invoke(t);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
        private void ExecuteEvent<T, Y>(string eventType, T t, Y y)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate @delegate in listenerDic[eventType])
                {
                    try
                    {
                        ((CallBack<T, Y>) @delegate).Invoke(t, y);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
                foreach (Delegate @delegate in listenerDic[eventType])
                {
                    try
                    {
                        ((CallBack<T, X, Y>) @delegate).Invoke(t, x, y);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
        private void ExecuteEvent<T, Y, X, Z>(string eventType, T t, Y y, X x, Z z)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate @delegate in listenerDic[eventType])
                {
                    try
                    {
                        ((CallBack<T, Y, X, Z>) @delegate).Invoke(t, y, x, z);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
        private void ExecuteEvent<T, Y, X, Z, W>(string eventType, T t, Y y, X x, Z z, W w)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                foreach (Delegate @delegate in listenerDic[eventType])
                {
                    try
                    {
                        ((CallBack<T, Y, X, Z, W>) @delegate).Invoke(t, y, x, z, w);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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