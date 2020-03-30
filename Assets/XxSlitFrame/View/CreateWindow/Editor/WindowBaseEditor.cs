using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;

namespace XxSlitFrame.View.CreateWindow.Editor
{
    public static class WindowBaseEditor
    {
        [MenuItem("GameObject/Create Empty WindowView", false, 0)]
        public static void OnCreateEmptyWindowView()
        {
            if (Object.FindObjectOfType<Canvas>())
            {
                //View 窗口根目录
                GameObject windowView = new GameObject("Empty WindowView");
                windowView.AddComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
                windowView.AddComponent<GenerationBaseWindow>().Init();
                //Window目录
                GameObject window = new GameObject("Window");
                window.AddComponent<RectTransform>();
                window.AddComponent<CanvasGroup>();
                //背景
                GameObject background = new GameObject("Background");
                background.AddComponent<Image>();
                //调整层级
                windowView.transform.SetParent(Object.FindObjectOfType<Canvas>().transform);
                window.transform.SetParent(windowView.transform);
                background.transform.SetParent(window.transform);
                //Transform 调整
                windowView.transform.localPosition = Vector3.zero;
                windowView.transform.localScale = Vector3.one;
                window.transform.localPosition = Vector3.zero;
                window.transform.localScale = Vector3.one;
                background.transform.localPosition = Vector3.zero;
                background.transform.localScale = Vector3.one;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Alt+T) 绑定Button  &t", false, 0)]
        public static void BindComponent()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;

            if (uiObj != null)
            {
                if (!uiObj.GetComponent<BindUiType>())
                {
                    uiObj.AddComponent<BindUiType>();
                }

                if (uiObj.GetComponent<UnityEngine.UI.Button>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Button;
                }
                else if (uiObj.GetComponent<Image>() && !uiObj.GetComponent<UnityEngine.UI.Button>() && !uiObj.GetComponent<Scrollbar>() &&
                         !uiObj.GetComponent<ScrollRect>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Image;
                }
                else if (uiObj.GetComponent<Text>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Text;
                }
                else if (uiObj.GetComponent<Toggle>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Toggle;
                }
                else if (uiObj.GetComponent<RawImage>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.RawImage;
                }
                else if (uiObj.GetComponent<Scrollbar>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Scrollbar;
                }
                else if (uiObj.GetComponent<Dropdown>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Dropdown;
                }
                else if (uiObj.GetComponent<InputField>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.InputField;
                }
                else if (uiObj.GetComponent<ScrollRect>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.ScrollRect;
                }
                else if (uiObj.GetComponent<VideoPlayer>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Text;
                }
                else
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.GameObject;
                }
            }
        }
    }
}