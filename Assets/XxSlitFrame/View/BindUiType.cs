using UnityEngine;
using UnityEngine.Serialization;

namespace XxSlitFrame.View
{
    public class BindUiType : MonoBehaviour
    {
        public enum UiType
        {
            GameObject,
            LGameObject,
            Button,
            Image,
            Text,
            Toggle,
            Input,
            DropDown,
            RawImage,
            Slider,
            Scrollbar,
            ScrollRect,
            LButton,
            LImage,
            LText,
            LToggle,
            LInput,
            LDropDown,
            LRawImage,
            LSlider,
            LScrollbar,
            LScrollView,
        }

        [FormerlySerializedAs("Type")] 
        public UiType type;
    }
}