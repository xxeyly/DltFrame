#if UNITY_EDITOR
using System.Linq;
using TMPro;

#if UNITY_2019_1_OR_NEWER
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using Object = UnityEngine.Object;

namespace DltFramework
{
    public static class WindowBaseEditor
    {
        [MenuItem("GameObject/Create Empty WindowView", false, 0)]
        public static void OnCreateEmptyWindowView()
        {
            Canvas[] allCanvas = Object.FindObjectsOfType<Canvas>();

            Canvas uiCanvas = null;
            foreach (Canvas canvas in allCanvas)
            {
                if (canvas.sortingOrder == 0)
                {
                    uiCanvas = canvas;
                    break;
                }
            }

            if (uiCanvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                uiCanvas = canvasObj.AddComponent<Canvas>();
                uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(1920, 1080);
                canvasObj.AddComponent<GraphicRaycaster>();
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            //View 窗口根目录
            GameObject windowView = new GameObject("Empty WindowView");
            Undo.RegisterCreatedObjectUndo(windowView, "Empty WindowView");
            Vector2 windowSize = uiCanvas.GetComponent<CanvasScaler>().referenceResolution;
            windowView.AddComponent<RectTransform>().sizeDelta = Vector2.zero;
            //Window目录
            GameObject window = new GameObject("Window");
            window.AddComponent<RectTransform>().sizeDelta = windowSize;
            window.AddComponent<CanvasGroup>();
            //背景
            GameObject background = new GameObject("Background");
            background.AddComponent<Image>().rectTransform.sizeDelta = Vector2.zero;
            windowView.transform.SetParent(uiCanvas.transform);
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

        [MenuItem("GameObject/生成 /@(Alt+V) 绑定UI类型  &v", false, 0)]
        public static void BindComponent()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null)
            {
                BindUiType bindUiType = uiObj.GetComponent<BindUiType>();
                Button button = uiObj.GetComponent<Button>();
                Text text = uiObj.GetComponent<Text>();
                TextMeshProUGUI textMeshProUGUI = uiObj.GetComponent<TextMeshProUGUI>();
                Toggle toggle = uiObj.GetComponent<Toggle>();
                RawImage rawImage = uiObj.GetComponent<RawImage>();
                Scrollbar scrollbar = uiObj.GetComponent<Scrollbar>();
                Dropdown dropdown = uiObj.GetComponent<Dropdown>();
                TMP_Dropdown tmpDropdown = uiObj.GetComponent<TMP_Dropdown>();
                InputField inputField = uiObj.GetComponent<InputField>();
                TMP_InputField tmpInputField = uiObj.GetComponent<TMP_InputField>();
                ScrollRect scrollRect = uiObj.GetComponent<ScrollRect>();
                VideoPlayer videoPlayer = uiObj.GetComponent<VideoPlayer>();
                Slider slider = uiObj.GetComponent<Slider>();
                ChildBaseWindow childBaseWindow = uiObj.transform.GetComponentInChildren<ChildBaseWindow>(true);
                Image image = uiObj.GetComponent<Image>();


                if (bindUiType == null)
                {
                    Undo.AddComponent<BindUiType>(uiObj);
                    bindUiType = uiObj.GetComponent<BindUiType>();
                }
                else
                {
                    if (bindUiType.type != UiType.GameObject)
                    {
                        bindUiType.type = UiType.GameObject;
                        return;
                    }
                }

                if (button != null)
                {
                    bindUiType.type = UiType.Button;
                    bindUiType.eventTriggerType = UIEventTriggerType.PointerClick;
                }
                else if (text != null)
                {
                    bindUiType.type = UiType.Text;
                }
                else if (textMeshProUGUI != null)
                {
                    bindUiType.type = UiType.TextMeshProUGUI;
                }
                else if (toggle != null)
                {
                    bindUiType.type = UiType.Toggle;
                }
                else if (rawImage != null)
                {
                    bindUiType.type = UiType.RawImage;
                }
                else if (scrollbar != null)
                {
                    bindUiType.type = UiType.Scrollbar;
                }
                else if (dropdown != null)
                {
                    bindUiType.type = UiType.Dropdown;
                }
                else if (tmpDropdown != null)
                {
                    bindUiType.type = UiType.TMP_Dropdown;
                }
                else if (inputField != null)
                {
                    bindUiType.type = UiType.InputField;
                }
                else if (tmpInputField != null)
                {
                    bindUiType.type = UiType.TMP_InputField;
                }
                else if (scrollRect != null)
                {
                    bindUiType.type = UiType.ScrollRect;
                }
                else if (videoPlayer != null)
                {
                    bindUiType.type = UiType.VideoPlayer;
                }
                else if (slider != null)
                {
                    bindUiType.type = UiType.Slider;
                }
                else if (childBaseWindow != null)
                {
                    bindUiType.type = UiType.ChildList;
                    bindUiType.childType = childBaseWindow.GetType().ToString();
                }
                else if (image != null)
                {
                    bindUiType.type = UiType.Image;
                }
                else
                {
                    bindUiType.type = UiType.GameObject;
                }
            }
        }
    }
}
#endif