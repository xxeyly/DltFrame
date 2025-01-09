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
            typeof(UniTaskFrameComponent),
#if HybridCLR
            typeof(HotFixFrameComponent),
#endif
            typeof(ViewFrameComponent),
        };

        //获得设备存储路径
        public static string GetDeviceStoragePath(bool read = false)
        {
            string path = String.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    path = DataFrameComponent.String_BuilderString(Application.dataPath, "/UnStreamingAssets");
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
                        path = DataFrameComponent.String_BuilderString("file://", Application.persistentDataPath);
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
        [LabelText("BaseWindow模板地址")] public static string BaseWindowTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/BaseWindowTemplate.cs");
        [LabelText("ChildBaseWindow模板地址")] public static string ChildBaseWindowTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/ChildBaseWindowTemplate.cs");

        [LabelText("ListenerComponentData模板地址")]
        public static string ListenerComponentDataTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/ListenerComponentDataTemplate.cs");

        [LabelText("SceneLoadComponent模板地址")] public static string SceneComponentTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/SceneComponentTemplate.cs");

        [LabelText("SceneLoadComponentInit模板地址")]
        public static string SceneComponentInitTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/SceneComponentInitTemplate.cs");
        [LabelText("EntityItem模板地址")]
        public static string EntityItemTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/EntityItemTemplate.cs");

        [LabelText("AnimatorControllerParameterData模板地址")]
        public static string AnimatorControllerParameterDataTemplatePath = DataFrameComponent.String_BuilderString(DltFrameworkPath, "Model/Template/AnimatorControllerParameterDataTemplate.cs");

        [LabelText("存放路径根路径")] public static string assetRootPath = "Assets/Config/";
        [LabelText("音频配置存放路径")] public static string customAudioDataPath = DataFrameComponent.String_BuilderString(assetRootPath, "CustomAudioData.asset");
        [LabelText("框架配置存放路径")] public static string frameComponentEditorDataPath = DataFrameComponent.String_BuilderString(assetRootPath, "FrameComponentEditorData.asset");
    }
}