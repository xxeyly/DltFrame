using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools.Svc.BaseSvc;

namespace XxSlitFrame.Tools.Svc
{
    public class ListenerSvc : SvcBase<ListenerSvc>
    {
        public delegate void CallBack();

        public delegate void CallBack<T>(T t);

        public delegate void CallBack<T, X>(T arg1, X arg2);

        public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);

        public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);

        public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);

        [Header("事件监听")] [SerializeField] private Dictionary<EventType, Delegate> listenerDic;

        /// <summary>
        /// 事件类型
        /// </summary>
        public enum EventType
        {
            Normal,

            /// <summary>
            /// 根据步骤索引执行事件
            /// </summary>
            InvokeEventByStepIndex,

            /// <summary>
            /// 跳转下一步骤
            /// </summary>
            SkipToNext,

            /// <summary>
            /// 相机移动到目标位置
            /// </summary>
            CameraMoveToTargetPos,

            /// <summary>
            /// 道具初始化
            /// </summary>
            PropInit,

            /// <summary>
            /// 打开世界操作点
            /// </summary>
            OpenWorldPoint,

            /// <summary>
            /// 关闭世界操作点
            /// </summary>
            CloseWorldPoint,

            /// <summary>
            /// 小步骤知识点
            /// </summary>
            SetSmallKnowledgePointsContent,

            /// <summary>
            /// 显示深度提示
            /// </summary>
            IntermittentNegativePressureBackPumpingShowDepth,

            /// <summary>
            /// 增加用物选择
            /// </summary>
            AddMaterialPreparationItem
        }

        public override void InitSvc()
        {
            listenerDic = new Dictionary<EventType, Delegate>();
        }

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="unityAction"></param>
        public void AddListenerEvent(EventType eventType, CallBack unityAction)
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
        public void AddListenerEvent<T>(EventType eventType, CallBack<T> callBack)
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
        public void AddListenerEvent<T, TY>(EventType eventType, CallBack<T, TY> callBack)
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
        public void AddListenerEvent<T, TY, TYX, TYXZ>(EventType eventType, CallBack<T, TY, TYX, TYXZ> callBack)
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
        public void DeleteListenerEvent(EventType eventType, UnityAction unityAction)
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
        public void ImplementListenerEvent(EventType eventType)
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
        public void ImplementListenerEvent<T>(EventType eventType, T t)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T>) listenerDic[eventType]).Invoke(t);
            }
            else
            {
                Debug.LogError(eventType + "该事件已经没有被绑定过");
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="t"></param>
        /// <param name="y"></param>
        public void ImplementListenerEvent<T, Y>(EventType eventType, T t, Y y)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T, Y>) listenerDic[eventType]).Invoke(t, y);
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
        public void ImplementListenerEvent<T, Y, X>(EventType eventType, T t, Y y, X x)
        {
            if (listenerDic.ContainsKey(eventType))
            {
                ((CallBack<T, Y, X>) listenerDic[eventType]).Invoke(t, y, x);
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
        public void ImplementListenerEvent<T, Y, X, Z>(EventType eventType, T t, Y y, X x, Z z)
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
        /// 获得事件类型
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public CallBack<T> GetEvent<T>(EventType eventType)
        {
            return (CallBack<T>) listenerDic[eventType];
        }
    }
}