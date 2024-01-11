using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DltFramework
{
    public static partial class RuntimeGlobal
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

        //获得设备存储路径
        public static string GetDeviceStoragePath(bool read = false)
        {
            string path = String.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    path = Application.dataPath + "/UnStreamingAssets";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    path = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WSAPlayerARM:
                case RuntimePlatform.Android:
                    if (read)
                    {
                        path = "file://" + Application.persistentDataPath;
                    }
                    else
                    {
                        path = Application.persistentDataPath;
                    }

                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.persistentDataPath;
                    break;
            }

            return path;
        }

        [LabelText("DltFramework路径")] public static string DltFrameworkPath = "Assets/DltFramework/Runtime/";
        [LabelText("BaseWindow模板地址")] public static string BaseWindowTemplatePath = DltFrameworkPath + "Model/Template/BaseWindowTemplate.cs";
        [LabelText("ChildBaseWindow模板地址")] public static string ChildBaseWindowTemplatePath = DltFrameworkPath + "Model/Template/ChildBaseWindowTemplate.cs";

        [LabelText("ListenerComponentData模板地址")]
        public static string ListenerComponentDataTemplatePath = DltFrameworkPath + "Model/Template/ListenerComponentDataTemplate.cs";

        [LabelText("SceneLoadComponent模板地址")] public static string SceneComponentTemplatePath = DltFrameworkPath + "Model/Template/SceneComponentTemplate.cs";

        [LabelText("SceneLoadComponentInit模板地址")]
        public static string SceneComponentInitTemplatePath = DltFrameworkPath + "Model/Template/SceneComponentInitTemplate.cs";

        [LabelText("AnimatorControllerParameterData模板地址")]
        public static string AnimatorControllerParameterDataTemplatePath = DltFrameworkPath + "Model/Template/AnimatorControllerParameterDataTemplate.cs";

        [LabelText("存放路径根路径")] public static string assetRootPath = "Assets/Config/";
        [LabelText("音频配置存放路径")] public static string customAudioDataPath = assetRootPath + "CustomAudioData.asset";
        [LabelText("框架配置存放路径")] public static string frameComponentEditorDataPath = assetRootPath + "FrameComponentEditorData.asset";
    }
}