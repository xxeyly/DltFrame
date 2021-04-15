using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace XxSlitFrame.View
{
    public class BindUiType : MonoBehaviour
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
            ChildList,
            VideoPlayer
        }

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

            All = PointerEnter | PointerExit | PointerDown | PointerUp | PointerClick | Drag | Drop |
                  Scroll | UpdateSelected | Select | Deselect | Move | InitializePotentialDrag | BeginDrag |
                  EndDrag | Submit | Cancel
        }

        [LabelText("UI组件类型")] public UiType type;

        // [EnableIf("@type == UiType.ChildList")]
        [ShowIf("@type == UiType.ChildList")] public ChildBaseWindow childType;

        [ShowIf("@type == UiType.Button")] [LabelText("UI触发事件类型")]
        public UIEventTriggerType eventTriggerType;
    }
}