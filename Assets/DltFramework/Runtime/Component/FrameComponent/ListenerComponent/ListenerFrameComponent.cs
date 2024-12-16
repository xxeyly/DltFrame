using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = System.Object;


// ReSharper disable once CheckNamespace
// ReSharper disable once TypeParameterCanBeVariant

namespace DltFramework
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public partial class ListenerFrameComponent : FrameComponent
    {
        public static ListenerFrameComponent Instance;

        public delegate void CallBack();

        public delegate void CallBack<T>(T t);

        public delegate void CallBack<T, T1>(T t, T1 t1);

        public delegate void CallBack<T, T1, T2>(T t, T1 t1, T2 t2);

        public delegate void CallBack<T, T1, T2, T3>(T t, T1 t1, T2 t2, T3 t3);

        public delegate void CallBack<T, T1, T2, T3, T4>(T t, T1 t1, T2 t2, T3 t3, T4 t4);

        public delegate void CallBack<T, T1, T2, T3, T4, T5>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12);

        public delegate void
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12,
            T13 t13, T14 t14);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15,
            T16 t16);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14, T15 t15,
            T16 t16, T17 t17);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14,
            T15 t15, T16 t16, T17 t17, T18 t18);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14,
            T15 t15, T16 t16, T17 t17, T18 t18, T19 t19);

        public delegate void CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13,
            T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20);


        public delegate R ReturnCallBack<R>();

        public delegate R ReturnCallBack<T, R>(T t);

        public delegate R ReturnCallBack<T, T1, R>(T t, T1 x);

        public delegate R ReturnCallBack<T, T1, T2, R>(T t, T1 t1, T2 t2);

        public delegate R ReturnCallBack<T, T1, T2, T3, R>(T t, T1 t1, T2 t2, T3 t3);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12,
            T13 t13);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14, T15 t15,
            T16 t16);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14,
            T15 t15, T16 t16, T17 t17);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14,
            T15 t15, T16 t16, T17 t17, T18 t18);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13,
            T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19);

        public delegate R ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, R>(T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8,
            T9 t9, T10 t10, T11 t11, T12 t12,
            T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20);


        [LabelText("所有触发事件")] public Dictionary<string, List<Delegate>> allListener = new Dictionary<string, List<Delegate>>();


        public override void SetFrameInitIndex()
        {
            frameInitIndex = 0;
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<ListenerFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
            GetAllAddListenerEvent();
        }

        public override void FrameSceneEndComponent()
        {
            allListener.Clear();
        }

        public override void FrameEndComponent()
        {
        }

        #region 不带返回值添加监听

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
            if (!allListener.ContainsKey(eventType))
            {
                allListener.Add(eventType, new List<Delegate>());
            }

            List<Delegate> currentDelegateList = allListener[eventType];
            if (!currentDelegateList.Contains(customDelegate))
            {
                currentDelegateList.Add(customDelegate);
            }
            else
            {
                Debug.LogWarning(eventType + "该事件已经被绑定了");
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
        public void AddListenerEvent<T, T1>(string eventType, CallBack<T, T1> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, T1, T2>(string eventType, CallBack<T, T1, T2> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, T1, T2, T3>(string eventType, CallBack<T, T1, T2, T3> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddListenerEvent<T, T1, T2, T3, T4>(string eventType, CallBack<T, T1, T2, T3, T4> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5>(string eventType, CallBack<T, T1, T2, T3, T4, T5> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        #endregion

        #region 带返回值添加监听

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        /// <typeparam name="R"></typeparam>
        public void AddReturnListenerEvent<R>(string eventType, ReturnCallBack<R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, R>(string eventType, ReturnCallBack<T, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, T1, R>(string eventType, ReturnCallBack<T, T1, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, T1, T2, R>(string eventType, ReturnCallBack<T, T1, T2, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, T1, T2, T3, R>(string eventType, ReturnCallBack<T, T1, T2, T3, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void AddReturnListenerEvent<T, T1, T2, T3, T4, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(string eventType, ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        public void AddReturnListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, R>(string eventType,
            ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, R> callBack)
        {
            AddDelegateToListenerEvent(eventType, callBack);
        }

        #endregion

        #region 移除监听

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        public void RemoveListenerEvent(string eventType)
        {
            RemoveDelegateToListenerEvent(eventType);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent(string eventType, CallBack callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T>(string eventType, CallBack<T> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, T1>(string eventType, CallBack<T, T1> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, T1, T2>(string eventType, CallBack<T, T1, T2> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, T1, T2, T3>(string eventType, CallBack<T, T1, T2, T3> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        public void RemoveListenerEvent<T, T1, T2, T3, T4>(string eventType, CallBack<T, T1, T2, T3, T4> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5>(string eventType, CallBack<T, T1, T2, T3, T4, T5> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }


        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string eventType, CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }

        public void RemoveListenerEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(string eventType,
            CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> callBack)
        {
            RemoveDelegateToListenerEvent(eventType, callBack);
        }


        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="eventType"></param>
        public void RemoveDelegateToListenerEvent(string eventType)
        {
            if (allListener.ContainsKey(eventType))
            {
                allListener.Remove(eventType);
            }
            else
            {
                Debug.Log(eventType + "没有被绑定过");
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="customDelegate"></param>
        private void RemoveDelegateToListenerEvent(string eventType, Delegate customDelegate)
        {
            if (!allListener.ContainsKey(eventType))
            {
                Debug.Log(eventType + "没有被绑定过");
            }
            else
            {
                List<Delegate> delegates = allListener[eventType];
                if (delegates.Contains(customDelegate))
                {
                    delegates.Remove(customDelegate);
                }
                else
                {
                    Debug.Log(eventType + "没有被绑定过");
                }

                if (delegates.Count == 0)
                {
                    allListener.Remove(eventType);
                }
            }
        }

        #endregion

        #region 执行无返回值监听

        public void ExecuteEvent(string eventType, string delegateType)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 0 && customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack)customDelegate)();
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T>(string eventType, string delegateType, T t)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (t == null)
                    {
                        return;
                    }

                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T>)customDelegate)(t);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1>(string eventType, string delegateType, T t, T1 t1)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1>)customDelegate)(t, t1);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2>(string eventType, string delegateType, T t, T1 t1, T2 t2)
        {
            if (allListener.ContainsKey(eventType))

            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2>)customDelegate)(t, t1, t2);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3)
        {
            if (allListener.ContainsKey(eventType))

            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3>)customDelegate)(t, t1, t2, t3);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4>)customDelegate)(t, t1, t2, t3, t4);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 6 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5>)customDelegate)(t, t1, t2, t3, t4, t5);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 7 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6>)customDelegate)(t, t1, t2, t3, t4, t5, t6);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 8 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 9 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 10 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 11 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 12 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 13 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType
                );
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 14 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType
                );
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8,
            T9 t9, T10 t10, T11 t11, T12 t12, T13 t13,
            T14 t14)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 15 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            T8 t8, T9 t9, T10 t10, T11 t11, T12 t12,
            T13 t13, T14 t14, T15 t15)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 16 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 17 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,
            T7 t7, T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 18 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15,
                            t16, t17);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,
            T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 19 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.GetParameters()[18].ParameterType == t18.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14,
                            t15, t16, t17, t18);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,
            T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 20 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.GetParameters()[18].ParameterType == t18.GetType() &&
                        customDelegate.Method.GetParameters()[19].ParameterType == t19.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13,
                            t14, t15, t16, t17, t18, t19);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }


        public void ExecuteEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4,
            T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 21 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.GetParameters()[18].ParameterType == t18.GetType() &&
                        customDelegate.Method.GetParameters()[19].ParameterType == t19.GetType() &&
                        customDelegate.Method.GetParameters()[20].ParameterType == t20.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        ((CallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12,
                            t13, t14, t15, t16, t17, t18, t19, t20);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }
        }

        #endregion

        #region 执行有返回值监听

        public R ExecuteReturnEvent<R>(string eventType, string delegateType)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 0 &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<R>)customDelegate)();
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, R>(string eventType, string delegateType, T t)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 1 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, R>)customDelegate)(t);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, R>(string eventType, string delegateType, T t, T1 t1)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 2 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, R>)customDelegate)(t, t1);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, R>(string eventType, string delegateType, T t, T1 t1, T2 t2)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 3 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, R>)customDelegate)(t, t1, t2);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 4 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, R>)customDelegate)(t, t1, t2, t3);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 5 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, R>)customDelegate)(t, t1, t2, t3, t4);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 6 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, R>)customDelegate)(t, t1, t2, t3, t4, t5);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 7 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 8 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 9 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 10 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 11 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 12 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 13 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8,
            T9 t9, T10 t10, T11 t11, T12 t12,
            T13 t13)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 14 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            T8 t8, T9 t9, T10 t10, T11 t11, T12 t12,
            T13 t13, T14 t14)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 15 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7,
            T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 16 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14,
                            t15);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6,
            T7 t7, T8 t8, T9 t9, T10 t10, T11 t11,
            T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 17 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13,
                            t14, t15, t16);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5,
            T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 18 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12,
                            t13, t14, t15, t16, t17);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4,
            T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10,
            T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 19 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.GetParameters()[18].ParameterType == t18.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11,
                            t12, t13, t14, t15, t16, t17, t18);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3, T4 t4,
            T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 20 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.GetParameters()[18].ParameterType == t18.GetType() &&
                        customDelegate.Method.GetParameters()[19].ParameterType == t19.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10,
                            t11, t12, t13, t14, t15, t16, t17, t18,
                            t19);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        public R ExecuteReturnEvent<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, R>(string eventType, string delegateType, T t, T1 t1, T2 t2, T3 t3,
            T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, T19 t19, T20 t20)
        {
            if (allListener.ContainsKey(eventType))
            {
                foreach (Delegate customDelegate in allListener[eventType])
                {
                    if (customDelegate.Method.GetParameters().Length == 21 &&
                        customDelegate.Method.GetParameters()[0].ParameterType == t.GetType() &&
                        customDelegate.Method.GetParameters()[1].ParameterType == t1.GetType() &&
                        customDelegate.Method.GetParameters()[2].ParameterType == t2.GetType() &&
                        customDelegate.Method.GetParameters()[3].ParameterType == t3.GetType() &&
                        customDelegate.Method.GetParameters()[4].ParameterType == t4.GetType() &&
                        customDelegate.Method.GetParameters()[5].ParameterType == t5.GetType() &&
                        customDelegate.Method.GetParameters()[6].ParameterType == t6.GetType() &&
                        customDelegate.Method.GetParameters()[7].ParameterType == t7.GetType() &&
                        customDelegate.Method.GetParameters()[8].ParameterType == t8.GetType() &&
                        customDelegate.Method.GetParameters()[9].ParameterType == t9.GetType() &&
                        customDelegate.Method.GetParameters()[10].ParameterType == t10.GetType() &&
                        customDelegate.Method.GetParameters()[11].ParameterType == t11.GetType() &&
                        customDelegate.Method.GetParameters()[12].ParameterType == t12.GetType() &&
                        customDelegate.Method.GetParameters()[13].ParameterType == t13.GetType() &&
                        customDelegate.Method.GetParameters()[14].ParameterType == t14.GetType() &&
                        customDelegate.Method.GetParameters()[15].ParameterType == t15.GetType() &&
                        customDelegate.Method.GetParameters()[16].ParameterType == t16.GetType() &&
                        customDelegate.Method.GetParameters()[17].ParameterType == t17.GetType() &&
                        customDelegate.Method.GetParameters()[18].ParameterType == t18.GetType() &&
                        customDelegate.Method.GetParameters()[19].ParameterType == t19.GetType() &&
                        customDelegate.Method.GetParameters()[20].ParameterType == t20.GetType() &&
                        customDelegate.Method.Name == delegateType)
                    {
                        return ((ReturnCallBack<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, R>)customDelegate)(t, t1, t2, t3, t4, t5, t6, t7, t8, t9,
                            t10, t11, t12, t13, t14, t15, t16, t17,
                            t18, t19, t20);
                    }
                }
            }
            else
            {
                Debug.LogWarning("该事件没有被绑定过:" + eventType);
            }

            return default(R);
        }

        #endregion

        public void GetAllAddListenerEvent()
        {
            foreach (SceneComponent sceneComponent in DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponent>())
            {
                ReflexBinEventListener(sceneComponent);
            }

            foreach (SceneComponentInit sceneComponentInit in DataFrameComponent.Hierarchy_GetAllObjectsInScene<SceneComponentInit>())
            {
                ReflexBinEventListener(sceneComponentInit);
            }

            foreach (BaseWindow baseWindow in DataFrameComponent.Hierarchy_GetAllObjectsInScene<BaseWindow>())
            {
                if (baseWindow.GetComponent<ChildBaseWindow>())
                {
                    continue;
                }

                ReflexBinEventListener(baseWindow);
            }
        }

        private void ReflexBinEventListener(Object targetObject)
        {
            //反射所有Type中的方法
            foreach (MethodInfo methodInfo in targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                //获得该方法所有使用的特性
                foreach (Attribute customAttribute in methodInfo.GetCustomAttributes())
                {
                    //当前特性是监听事件
                    if (customAttribute is AddListenerEventAttribute)
                    {
                        Type[] parameterInfo;
                        if (methodInfo.ReturnType == typeof(void))
                        {
                            parameterInfo = new Type[methodInfo.GetParameters().Length];
                            for (int i = 0; i < methodInfo.GetParameters().Length; i++)
                            {
                                parameterInfo[i] = methodInfo.GetParameters()[i].ParameterType;
                            }
                        }
                        else
                        {
                            parameterInfo = new Type[methodInfo.GetParameters().Length + 1];
                            for (int i = 0; i < methodInfo.GetParameters().Length; i++)
                            {
                                parameterInfo[i] = methodInfo.GetParameters()[i].ParameterType;
                            }

                            parameterInfo[methodInfo.GetParameters().Length] = methodInfo.ReturnType;
                        }


                        Type delegateType = null;
                        if (methodInfo.ReturnType != typeof(void))
                        {
                            if (parameterInfo.Length == 1)
                            {
                                delegateType = typeof(ReturnCallBack<>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 2)
                            {
                                delegateType = typeof(ReturnCallBack<,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 3)
                            {
                                delegateType = typeof(ReturnCallBack<,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 4)
                            {
                                delegateType = typeof(ReturnCallBack<,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 5)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 6)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 7)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 8)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 9)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 10)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 11)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 12)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 13)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 14)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 15)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 16)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 17)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 18)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 19)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 20)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                            else if (parameterInfo.Length == 21)
                            {
                                delegateType = typeof(ReturnCallBack<,,,,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                            }
                        }
                        else
                        {
                            if (methodInfo.GetParameters().Length == 0)
                            {
                                delegateType = typeof(CallBack);
                            }
                            else
                            {
                                if (parameterInfo.Length == 1)
                                {
                                    delegateType = typeof(CallBack<>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 2)
                                {
                                    delegateType = typeof(CallBack<,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 3)
                                {
                                    delegateType = typeof(CallBack<,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 4)
                                {
                                    delegateType = typeof(CallBack<,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 5)
                                {
                                    delegateType = typeof(CallBack<,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 6)
                                {
                                    delegateType = typeof(CallBack<,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 7)
                                {
                                    delegateType = typeof(CallBack<,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 8)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 9)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 10)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 11)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 12)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 13)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 14)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 15)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 16)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 17)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 18)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 19)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 20)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                                else if (parameterInfo.Length == 21)
                                {
                                    delegateType = typeof(CallBack<,,,,,,,,,,,,,,,,,,,,>).MakeGenericType(parameterInfo);
                                }
                            }
                        }

                        if (delegateType != null)
                        {
                            Delegate tempDelegate = Delegate.CreateDelegate(delegateType, targetObject, methodInfo);
                            AddDelegateToListenerEvent(targetObject.GetType().Name, tempDelegate);
                        }
                    }
                }
            }
        }
    }
}