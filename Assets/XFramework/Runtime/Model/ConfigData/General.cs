using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace XFramework
{
    public static partial class General
    {

        [LabelText("获得网页跟目录地址")]
        public static string GetUrlRootPath()
        {
            string url = Application.absoluteURL;
            return url;
        }

        [LabelText("框架组件初始化顺序")] public static List<Type> frameComponentType = new List<Type>()
        {
            typeof(SceneLoadFrameComponent),
            typeof(RuntimeDataFrameComponent),
            typeof(EntityFrameComponent),
            typeof(ListenerFrameComponent),
            typeof(AudioFrameComponent),
            typeof(HttpFrameComponent),
            // typeof(MouseFrameComponent),
            // typeof(TimeFrameComponent),
            typeof(ViewFrameComponent),
            typeof(UniTaskFrameComponent),
#if HybridCLR
            typeof(HotFixFrameComponent),
#endif
        };

        public static string GetDeviceStoragePath()
        {
            string path = String.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    path = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.Android:
                    path = Application.persistentDataPath;
                    break;
            }

            return path;
        }

        [LabelText("XFramework路径")] public static string XFrameworkPath = "Assets/XFramework/Runtime/";
        [LabelText("BaseWindow模板地址")] public static string BaseWindowTemplatePath = XFrameworkPath + "Model/Template/BaseWindowTemplate.cs";
        [LabelText("ChildBaseWindow模板地址")] public static string ChildBaseWindowTemplatePath = XFrameworkPath + "Model/Template/ChildBaseWindowTemplate.cs";

        [LabelText("ListenerComponentData模板地址")]
        public static string ListenerComponentDataTemplatePath = XFrameworkPath + "Model/Template/ListenerComponentDataTemplate.cs";

        [LabelText("SceneLoadComponent模板地址")] public static string SceneComponentTemplatePath = XFrameworkPath + "Model/Template/SceneComponentTemplate.cs";

        [LabelText("SceneLoadComponentInit模板地址")]
        public static string SceneComponentInitTemplatePath = XFrameworkPath + "Model/Template/SceneComponentInitTemplate.cs";

        [LabelText("AnimatorControllerParameterData模板地址")]
        public static string AnimatorControllerParameterDataTemplatePath = XFrameworkPath + "Model/Template/AnimatorControllerParameterDataTemplate.cs";

        [LabelText("存放路径根路径")] public static string assetRootPath = "Assets/Config/";
        [LabelText("音频配置存放路径")] public static string customAudioDataPath = assetRootPath + "CustomAudioData.asset";
        [LabelText("框架配置存放路径")] public static string frameComponentEditorDataPath = assetRootPath + "FrameComponentEditorData.asset";
    }
}