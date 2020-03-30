using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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

        [MenuItem("GameObject/绑定UI /@(Alt+B) 绑定Button  &b", false, 0)]
        public static void BindButton()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.Button;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Button;
            }

            if (uiObj != null && !uiObj.GetComponent<UnityEngine.UI.Button>())
            {
                uiObj.AddComponent<UnityEngine.UI.Button>();
            }
        }

        [MenuItem("GameObject/绑定UI /@(Shift+Alt+B) 绑定ButtonList  #&b", false, 0)]
        public static void BindListButton()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.LButton;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.LButton;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Ctrl+I) 绑定Image  %i", false, 0)]
        public static void BindImage()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.Image;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Image;
            }

            if (uiObj != null && !uiObj.GetComponent<Image>())
            {
                uiObj.AddComponent<Image>();
            }
        }

        [MenuItem("GameObject/绑定UI /@(Shift+Ctrl+I) 绑定ImageList  #%i", false, 0)]
        public static void BindListImage()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.LImage;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.LImage;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Ctrl+T) 绑定Text  %t", false, 0)]
        public static void BindText()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.Text;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Text;
            }

            if (uiObj != null && !uiObj.GetComponent<Text>())
            {
                uiObj.AddComponent<Text>();
            }
        }

        [MenuItem("GameObject/绑定UI /@(Shift+Ctrl+T) 绑定TextList  #%t", false, 0)]
        public static void BindListText()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.LText;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.LText;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Alt+T) 绑定Toggle   &t", false, 0)]
        public static void BindToggle()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.Toggle;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Toggle;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Shift+Alt+T) 绑定ToggleList  #&t", false, 0)]
        public static void BindListToggle()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.LToggle;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.LToggle;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Alt+I) 绑定InputField   &i", false, 0)]
        public static void BindInputField()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.Input;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Input;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Shift+Alt+I) 绑定InputFieldList  #&i", false, 0)]
        public static void BindListInputField()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.LInput;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.LInput;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Ctrl+G) GameObject  %g", false, 0)]
        public static void BindGameObject()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.GameObject;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.GameObject;
            }
        }

        [MenuItem("GameObject/绑定UI /@(Shift+Ctrl+G) GameObject  #%g", false, 0)]
        public static void BindListGameObject()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            if (uiObj != null && !uiObj.GetComponent<BindUiType>())
            {
                uiObj.AddComponent<BindUiType>().type = BindUiType.UiType.LGameObject;
            }
            else
            {
                if (uiObj != null) uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.LGameObject;
            }
        }
    }
}