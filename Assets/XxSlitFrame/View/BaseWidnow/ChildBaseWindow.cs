using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    /// <summary>
    /// 局部UI界面
    /// </summary>
    public abstract class ChildBaseWindow : MonoBehaviour
    {
        [HideInInspector] public Type viewType;

        [HorizontalGroup("命名")] [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [LabelText("类名称")]
        public string typeName;

        [HorizontalGroup("命名")]
        [Button(ButtonSizes.Small)]
        [LabelText("重命名")]
        [GUIColor(0, 1, 0)]
        public void GameNameSet()
        {
            gameObject.name = viewType.Name;

            typeName = viewType.Name;
        }

        protected ChildBaseWindow()
        {
            viewType = InitViewType();
        }

        public void Init()
        {
            InitView();
            InitListener();
            InitData();
        }

        /// <summary>
        /// UI类型设置
        /// </summary>
        protected Type InitViewType()
        {
            return GetType();
        }

        protected abstract void InitView();
        protected abstract void InitListener();
        protected abstract void InitData();

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params GameObject[] hideObjArray)
        {
            foreach (GameObject hideObj in hideObjArray)
            {
                hideObj.SetActive(false);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(true);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(display);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params MaskableGraphic[] showObjArray)
        {
            foreach (MaskableGraphic showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }

        #region UI 绑定

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">需要绑定的组件</param>
        /// <param name="path">当前组件的路径</param>
        protected void BindUi<T>(ref T viewType, string path)
        {
            viewType = transform.Find(path).GetComponent<T>();
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi<T>(ref List<T> viewType, string path)
        {
            viewType = new List<T>(transform.Find(path).GetComponentsInChildren<T>());
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi(ref GameObject viewType, string path)
        {
            viewType = transform.Find(path).GetComponent<Transform>().gameObject;
        }

        #endregion

        #region UI 事件绑定

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(UIBehaviour selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = selectable.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            UnityAction<BaseEventData> callback = action;
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventId};
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="buttonList">当前要操作的UI组件</param>
        /// <param name="eventID">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(List<Selectable> buttonList, EventTriggerType eventID, UnityAction<BaseEventData> action)
        {
            foreach (Selectable selectable in buttonList)
            {
                EventTrigger trigger = selectable.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = selectable.gameObject.AddComponent<EventTrigger>();
                }

                if (trigger.triggers.Count == 0)
                {
                    trigger.triggers = new List<EventTrigger.Entry>();
                }

                UnityAction<BaseEventData> callback = action;
                EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventID};
                entry.callback.AddListener(callback);
                trigger.triggers.Add(entry);
            }
        }

        protected List<Selectable> SelectableConverter<T>(List<T> selectableList) where T : Selectable
        {
            List<Selectable> SelectableList = new List<Selectable>();
            foreach (Selectable selectable in selectableList)
            {
                SelectableList.Add(selectable);
            }

            return SelectableList;
        }

        #endregion
    }
}