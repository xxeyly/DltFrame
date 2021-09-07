using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
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
    public abstract class ChildBaseWindow : BaseWindow
    {
        [LabelText("索引")] public int itemIndex;
        public override void ViewStartInit()
        {
            window = gameObject;
            SvcInit();
            InitView();
            InitListener();
            OnlyOnceInit();
        }

        /// <summary>
        /// 选中
        /// </summary>
        public virtual void OnSelect()
        {
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        public virtual void OnUnSelect()
        {
        }
    }
}