﻿using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
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

        [SerializeField] private Dictionary<string, Delegate> listenerDic;

        public override void StartSvc()
        {
            Instance = GetComponent<ListenerSvc>();
        }

        public override void InitSvc()
        {
            listenerDic = new Dictionary<string, Delegate>();
        }

        public override void EndSvc()
        {
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        public void AddListenerEvent(string eventType, CallBack unityAction)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                listenerDic.Add(eventType, unityAction);
            }
            else
            {
                Debug.LogError(eventType + "该事件已经被绑定了");
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T>(string eventType, CallBack<T> callBack)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                listenerDic.Add(eventType, callBack);
            }
            else
            {
//                Debug.LogError("该事件已经被绑定了");
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY>(string eventType, CallBack<T, TY> callBack)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                listenerDic.Add(eventType, callBack);
            }
            else
            {
                Debug.LogError("该事件已经被绑定了");
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY, TYX>(string eventType, CallBack<T, TY, TYX> callBack)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                listenerDic.Add(eventType, callBack);
            }
            else
            {
                Debug.LogError("该事件已经被绑定了");
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY, TYX, TYXZ>(string eventType, CallBack<T, TY, TYX, TYXZ> callBack)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                listenerDic.Add(eventType, callBack);
            }
            else
            {
                Debug.LogError("该事件已经被绑定了");
            }
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, TY, TYX, TYXZ, TYXZW>(string eventType,
            CallBack<T, TY, TYX, TYXZ, TYXZW> callBack)
        {
            if (!listenerDic.ContainsKey(eventType))
            {
                listenerDic.Add(eventType, callBack);
            }
            else
            {
                Debug.LogError("该事件已经被绑定了");
            }
        }


        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        public void DeleteListenerEvent(string eventType, UnityAction unityAction)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                listenerDic.Remove(eventType);
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
        public void ExecuteEvent(string eventType)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack) listenerDic[eventType]).Invoke();
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
        public void ExecuteEvent<T>(string eventType, T t)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T>) listenerDic[eventType]).Invoke(t);
            }
            else
            {
                Debug.LogError(eventType + "该事件没有被绑定过");
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        public void ExecuteEvent<T, TY>(string eventType, T t, TY y)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T, TY>) listenerDic[eventType]).Invoke(t, y);
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
        public void ExecuteEvent<T, TY, TX>(string eventType, T t, TY y, TX x)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T, TY, TX>) listenerDic[eventType]).Invoke(t, y, x);
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
        public void ExecuteEvent<T, Y, X, Z>(string eventType, T t, Y y, X x, Z z)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T, Y, X, Z>) listenerDic[eventType]).Invoke(t, y, x, z);
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
        public void ExecuteEvent<T, Y, X, Z, W>(string eventType, T t, Y y, X x, Z z, W w)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T, Y, X, Z, W>) listenerDic[eventType]).Invoke(t, y, x, z, w);
            }
            else
            {
                Debug.LogError("该事件没有被绑定过:" + eventType);
            }
        }


        /// <summary>
        /// 获得事件类型
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public CallBack<T> GetEvent<T>(string eventType)
        {
            return (CallBack<T>) listenerDic[eventType];
        }
    }
}