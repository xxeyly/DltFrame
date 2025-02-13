﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DltFramework
{
    public enum UiType
    {
        Button,
        Image,
        Text,
        Toggle,
        RawImage,
        Scrollbar,
        Dropdown,
        InputField,
        ScrollRect,
        GameObject,
        Slider,
        VideoPlayer,

        // ReSharper disable once InconsistentNaming
        TextMeshProUGUI,

        // ReSharper disable once InconsistentNaming
        TMP_Dropdown,

        // ReSharper disable once InconsistentNaming
        TMP_InputField,
        ChildList,
        Null,
    }

    /// <summary>
    /// 事件触发类型
    /// </summary>
    [System.Flags]
    public enum UIEventTriggerType
    {
        PointerClick = 1 << 1,
        PointerEnter = 1 << 2,
        PointerExit = 1 << 3,
        PointerDown = 1 << 4,
        PointerUp = 1 << 5,
        Drag = 1 << 6,
        Drop = 1 << 7,
        Scroll = 1 << 8,
        UpdateSelected = 1 << 9,
        Select = 1 << 10,
        Deselect = 1 << 11,
        Move = 1 << 12,
        InitializePotentialDrag = 1 << 13,
        BeginDrag = 1 << 14,
        EndDrag = 1 << 15,
        Submit = 1 << 16,
        Cancel = 1 << 17,

        All = PointerEnter | PointerExit | PointerDown | PointerUp | PointerClick | Drag | Drop | Scroll | UpdateSelected | Select | Deselect | Move | InitializePotentialDrag | BeginDrag |
              EndDrag | Submit | Cancel
    }

    public class BindUiType : MonoBehaviour
    {
        [LabelText("描述名称")] public string descriptionName;
        [LabelText("UI组件类型")] public UiType type;

        [LabelText("子级类型")] [ShowIf("@type ==UiType.ChildList")] [ValueDropdown("GetListOfChildBaseWindow")]
        public string childType;

        [ShowIf("@type == UiType.Button")] [LabelText("UI触发事件类型")]
        public UIEventTriggerType eventTriggerType;


        [LabelText("UI组件扩展类型")] [ValueDropdown("GetListOfMonoBehaviours")]
        public List<string> expansionType = new List<string>();

        private IEnumerable<string> GetListOfMonoBehaviours()
        {
            List<Object> all = new List<Object>(transform.GetComponents<Component>());
            List<string> selfObj = new List<string>();
            foreach (Object obj in all)
            {
                if (obj.GetType() != typeof(BindUiType) && !expansionType.Contains(obj.GetType().Name) && obj.GetType().Name != type.ToString())
                {
                    selfObj.Add(obj.GetType().Name);
                }
            }

            return selfObj;
        }

        private IEnumerable<string> GetListOfChildBaseWindow()
        {
            List<string> selfObj = new List<string>();
            List<Type> all = DataFrameComponent.List_GetSubclasses(typeof(ChildBaseWindow));
            foreach (Type childType in all)
            {
                if (childType != typeof(ChildBaseWindowTemplate) && childType != typeof(ChildUiBaseWindow))
                {
                    selfObj.Add(childType.ToString());
                }
            }

            return selfObj;
        }
    }
}